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
using Nueyon.Compose.Infrastructure.Options;

var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
if (args is ["preflight", var configDir])
{
    var configuration = new ConfigurationBuilder().SetBasePath(Path.GetFullPath(configDir))
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddEnvironmentVariables().AddUserSecrets(typeof(Cli).Assembly, true).Build();
    var options = new OpenAiOptions { ApiKey = configuration["OpenAI:ApiKey"] ?? "", Model = configuration["OpenAI:Model"] ?? "" };
    options.Validate();
    Require(options.Model == "gpt-5-mini", "Expected effective CLI model gpt-5-mini.");
    Console.WriteLine(JsonSerializer.Serialize(new { model = options.Model, credentialsConfigured = true }, json));
    return 0;
}

if (args.Length != 4 || args[0] is not ("offline" or "replay"))
    throw new ArgumentException("Usage: FeedbackLoop <offline|replay> <prompts> <input> <fixture|CLI trace>");

using var promptScope = PromptDirectory.Use(args[1]);
var input = Read<StoryInput>(args[2]);
var replay = args[0] == "replay";
var fixture = replay ? null : Read<Fixture>(args[3]);
var saved = replay ? Read<CliTrace>(args[3]).Attempts : null;
var generatorInstructions = IdeaAgent.GetSystemInstructions();
var judgeInstructions = IdeaSemanticJudge.GetSystemInstructions();
var originalTask = File.ReadAllText(Path.Combine(args[1], "Idea", "Idea.user.prompt.txt"))
    .Replace("{{SourceContent}}", input.Content, StringComparison.Ordinal);
var trace = new List<IdeaValidationAttempt>();
var requests = new List<Request>();
var generatorCalls = 0;
var judgeCalls = 0;
var incompleteVerdict = """{"blockingIssues":null}""";
using var generatorProbe = new Probe((messages, options) =>
{
    var index = generatorCalls++;
    var users = Capture("generator", index + 1, messages, options, generatorInstructions);
    Require(users.Length == (index == 0 ? 1 : 2) && users[0].Text == originalTask,
        "Expected a clean session with the exact original task and source.");
    if (index > 0)
    {
        var previous = trace.Last();
        var priorIdea = ReadFirstIdea(previous.Response);
        var parts = users[1].Text.Split("\nPrevious first idea: ", StringSplitOptions.None);
        Require(parts.Length == 2 &&
                parts[0].Contains("Preserve the original editorial purpose", StringComparison.Ordinal) &&
                parts[0].Contains("Do not add new claims or a research agenda", StringComparison.Ordinal) &&
                parts[0].Contains("Decide how to fix the problems using the original source", StringComparison.Ordinal),
            "Expected the retry purpose, source and independent repair instructions.");
        var diagnosis = parts[1].Split("\nDiagnosis: ", StringSplitOptions.None);
        Require(diagnosis.Length == 2 && JsonSerializer.Deserialize<Idea>(diagnosis[0], json) == priorIdea &&
                diagnosis[1] == previous.Feedback, "Expected only the latest diagnosis and previous first idea.");
    }

    if (saved is not null)
        return saved[index].Response;

    var alternative = fixture!.Ideas[2] with { Title = "Unselected control" };
    return JsonSerializer.Serialize(new { ideas = new[] { fixture.Ideas[index], alternative } }, json);
});
using var judgeProbe = new Probe((messages, options) =>
{
    judgeCalls++;
    var users = Capture("judge", generatorCalls, messages, options, judgeInstructions);
    using var payload = JsonDocument.Parse(users.Single().Text);
    Require(payload.RootElement.EnumerateObject().Count() == 2 &&
            payload.RootElement.GetProperty("source").GetString() == input.Content &&
            payload.RootElement.GetProperty("idea").Deserialize<Idea>(json) == ReadFirstIdea(trace.Last().Response),
        "Expected only the original source and current first idea in the judge request.");
    if (saved is null)
        return fixture!.Verdicts[judgeCalls - 1].GetRawText();
    var attempt = saved[generatorCalls - 1];
    return attempt.Status switch
    {
        "accepted" => """{"accepted":true,"blockingIssues":[]}""",
        "rejected" => $"{{\"accepted\":false,\"blockingIssues\":{attempt.Feedback}}}",
        _ => incompleteVerdict
    };
});
var judge = new IdeaSemanticJudge(judgeProbe.AsAIAgent(judgeInstructions, "OpenAIAgent"));
var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), judge);
var agent = new IdeaAgent(evaluator.Wrap(generatorProbe.AsAIAgent(generatorInstructions, "OpenAIAgent")),
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

var attempts = trace.GroupBy(entry => entry.Attempt).Select(group => group.Last()).ToArray();
var passed = saved is not null
    ? attempts.SequenceEqual(saved) && generatorCalls == saved.Length
    : attempts.Select(attempt => attempt.Status).SequenceEqual(fixture!.Expected) &&
      generatorCalls == fixture.Expected.Length && judgeCalls == fixture.Expected.Length &&
      (fixture.Error is null
          ? error is null && result?[0] == fixture.Ideas[fixture.Expected.Length - 1]
          : result is null && error == fixture.Error);
Console.WriteLine(JsonSerializer.Serialize(new
{
    mode = args[0], passed, runId, generatorCalls, judgeCalls, requests, attempts, result, error
}, json));
return passed ? 0 : 1;

T Read<T>(string path) => JsonSerializer.Deserialize<T>(File.ReadAllText(path), json)!;

Idea? ReadFirstIdea(string response)
{
    using var document = JsonDocument.Parse(response);
    return document.RootElement.GetProperty("ideas")[0].Deserialize<Idea>(json);
}

ChatMessage[] Capture(string agent, int attempt, ChatMessage[] messages, ChatOptions? options, string instructions)
{
    Require(options?.Instructions == instructions ||
            messages.Any(message => message.Role == ChatRole.System && message.Text == instructions),
        "Expected the exact frozen system instructions.");
    Require(options?.ResponseFormat is ChatResponseFormatJson { Schema: not null }, "Expected a response schema.");
    var format = (ChatResponseFormatJson)options!.ResponseFormat!;
    Require(messages.All(message => message.Role == ChatRole.User || message.Role == ChatRole.System),
        "Unexpected session history in prompt.");
    requests.Add(new Request(agent, attempt, options.Instructions,
        messages.Select(message => new Message(message.Role.Value, message.Text)).ToArray(),
        format.SchemaName, format.Schema!.Value));
    return messages.Where(message => message.Role == ChatRole.User).ToArray();
}

static void Require(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}

sealed record Fixture(Idea[] Ideas, string[] Expected, JsonElement[] Verdicts, string? Error);
sealed record CliTrace(IdeaValidationAttempt[] Attempts);
sealed record Message(string Role, string Text);
sealed record Request(string Agent, int Attempt, string? Instructions, Message[] Messages,
    string? SchemaName, JsonElement Schema);

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
