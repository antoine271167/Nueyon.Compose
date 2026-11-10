using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Host.Console;
using Nueyon.Compose.Infrastructure.Agents;
using Nueyon.Compose.Infrastructure.Options;

if (args.Length < 2 || args[0] is not ("preflight" or "offline" or "live"))
    throw new ArgumentException("Usage: FeedbackLoop preflight <CLI config dir> | <offline|live> <CLI config dir> <prompts> <input> <fixture>");

var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
var configuration = new ConfigurationBuilder().SetBasePath(Path.GetFullPath(args[1]))
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .AddEnvironmentVariables().AddUserSecrets(typeof(Cli).Assembly, true).Build();
var options = new OpenAiOptions { ApiKey = configuration["OpenAI:ApiKey"] ?? "", Model = configuration["OpenAI:Model"] ?? "" };
if (options.Model != "gpt-5-mini")
    throw new InvalidOperationException("Expected effective CLI model gpt-5-mini.");
if (args[0] == "preflight")
{
    options.Validate();
    Console.WriteLine(JsonSerializer.Serialize(new { model = options.Model, credentialsConfigured = true }, json));
    return 0;
}

if (args.Length != 5)
    throw new ArgumentException("Expected disk prompts, source input, and fixture.");
using var promptScope = PromptDirectory.Use(args[2]);
var input = JsonSerializer.Deserialize<StoryInput>(File.ReadAllText(args[3]), json)!;
var fixture = JsonSerializer.Deserialize<Fixture>(File.ReadAllText(args[4]), json)!;
var judgeInstructions = IdeaSemanticJudge.GetSystemInstructions();
var generatorInstructions = IdeaAgent.GetSystemInstructions();
var trace = new List<IdeaValidationAttempt>();
var generatorCalls = 0;
var offlineJudgeCalls = 0;
using var generatorProbe = new Probe((messages, chat) =>
{
    var index = generatorCalls++;
    CheckRequest(messages, chat, generatorInstructions);
    Require(messages.Any(message => message.Role == ChatRole.User &&
                                   message.Text.Contains(input.Content, StringComparison.Ordinal)),
        "The original source did not reach the generator.");
    if (index > 0)
    {
        var feedback = trace.Last().Feedback!;
        Require(messages.Any(message => message.Text.Contains(feedback, StringComparison.Ordinal)),
            "The preceding verdict did not reach the generator.");
    }

    var alternative = fixture.Ideas[2] with { Title = "Unselected control" };
    return JsonSerializer.Serialize(new { ideas = new[] { fixture.Ideas[index], alternative } }, json);
});
using var judgeProbe = new Probe((messages, chat) =>
{
    var index = offlineJudgeCalls++;
    CheckRequest(messages, chat, judgeInstructions);
    using var payload = JsonDocument.Parse(messages.Single(message => message.Role == ChatRole.User).Text);
    Require(payload.RootElement.EnumerateObject().Count() == 2 &&
            payload.RootElement.GetProperty("source").GetString() == input.Content &&
            payload.RootElement.GetProperty("idea").Deserialize<Idea>(json) == fixture.Ideas[index],
        "The judge must see only the full original source and the current first idea.");

    // Controlled verdicts verify the real loop and feedback transport. They are not semantic judgments.
    return index < 2
        ? """{"blockingIssues":[{"problem":"Scripted rejection","correction":"Transport probe only"}]}"""
        : """{"blockingIssues":[]}""";
});
var live = args[0] == "live";
if (live)
    options.Validate();
var judge = live
    ? OpenAiAgentFactory.CreateOpenAiAgent(options.ApiKey, options.Model, judgeInstructions)
    : judgeProbe.AsAIAgent(judgeInstructions, "JudgeProbe");
var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), new IdeaSemanticJudge(judge));
var agent = new IdeaAgent(evaluator.Wrap(generatorProbe.AsAIAgent(generatorInstructions, "GeneratorProbe")),
    NullLogger<IdeaAgent>.Instance);

var runId = Guid.NewGuid();
var context = new AgentExecutionContext(runId)
{
    AdditionalProperties = new Dictionary<string, object?>
    {
        [IdeaValidationLoopEvaluator.ObserverKey] = (Action<IdeaValidationAttempt>)trace.Add
    }
};
IReadOnlyList<Idea>? result = null;
string? error = null;
try
{
    result = await agent.ExecuteAsync(context, input);
}
catch (Exception ex)
{
    error = $"{ex.GetType().Name}: {ex.Message}";
}

var attempts = trace.GroupBy(eventEntry => eventEntry.Attempt).Select(group => group.Last()).ToArray();
var passed = error is null && attempts.Select(attempt => attempt.Status).SequenceEqual(fixture.Expected) &&
             generatorCalls == fixture.Expected.Length && (live || offlineJudgeCalls == fixture.Expected.Length) &&
             result?[0] == fixture.Ideas[2];
Console.WriteLine(JsonSerializer.Serialize(new
{
    mode = args[0], model = live ? options.Model : null, modelSnapshot = (string?)null,
    runId, passed, generatorCalls, offlineJudgeCalls, attempts, result, error
}, json));
return passed ? 0 : 1;

static void CheckRequest(ChatMessage[] messages, ChatOptions? options, string instructions)
{
    Require(options?.Instructions == instructions ||
            messages.Any(message => message.Role == ChatRole.System && message.Text == instructions),
        "Expected the candidate disk instructions.");
    Require(options?.ResponseFormat is ChatResponseFormatJson { Schema: not null }, "Expected a response schema.");
}

static void Require(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}

sealed record Fixture(Idea[] Ideas, string[] Expected);

sealed class Probe(Func<ChatMessage[], ChatOptions?, string> respond) : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, respond(messages.ToArray(), options))));
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();

    public object? GetService(Type serviceType, object? serviceKey = null) => null;
    public void Dispose() { }
}
