using System.Reflection;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Host.Console;
using Nueyon.Compose.Infrastructure.Agents;
using Nueyon.Compose.Infrastructure.Options;

if (args.Length != 4 || args[0] is not ("offline" or "live"))
    throw new ArgumentException("Usage: JudgeReplay <offline|live> <prompt dir> <idea-input.json> <idea-result.json>");

var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
var input = JsonSerializer.Deserialize<StoryInput>(File.ReadAllText(args[2]), json)!;
var selected = JsonSerializer.Deserialize<Idea[]>(File.ReadAllText(args[3]), json)![0];
using var scope = PromptDirectory.Use(args[1]);
var instructions = IdeaSemanticJudge.GetSystemInstructions();
var config = new ConfigurationBuilder()
    .SetBasePath(Path.GetFullPath("src/Nueyon.Compose.Host.Console"))
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Cli).Assembly, true)
    .Build();
var options = new OpenAiOptions { ApiKey = config["OpenAI:ApiKey"] ?? "", Model = config["OpenAI:Model"] ?? "" };
if (options.Model != "gpt-5-mini")
    throw new InvalidOperationException("Expected the effective model configuration to be gpt-5-mini.");

var offline = args[0] == "offline";
using var probe = new OfflineProbe(input.Content, selected, instructions, json);
if (!offline)
    options.Validate();

var agent = offline
    ? probe.AsAIAgent(instructions, "OpenAIAgent")
    : OpenAiAgentFactory.CreateOpenAiAgent(options.ApiKey, options.Model, instructions);
var judge = new IdeaSemanticJudge(agent);
var judgeMethod = typeof(IdeaSemanticJudge).GetMethod("FindBlockingIssuesAsync",
    BindingFlags.Instance | BindingFlags.NonPublic)
    ?? throw new MissingMethodException(nameof(IdeaSemanticJudge), "FindBlockingIssuesAsync");
var verdict = (Task)judgeMethod.Invoke(judge, [input.Content, selected, CancellationToken.None])!;
await verdict;
var issues = verdict.GetType().GetProperty("Result")!.GetValue(verdict);
Console.WriteLine(JsonSerializer.Serialize(new
{
    mode = args[0],
    modelAlias = options.Model,
    modelSnapshot = (string?)null,
    credentialsConfigured = !string.IsNullOrWhiteSpace(options.ApiKey),
    blockingIssues = issues
}, json));

// The offline response exercises transport wiring and parsing only. It is not a semantic verdict.
sealed class OfflineProbe(string source, Idea idea, string instructions, JsonSerializerOptions json) : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        var sent = messages.ToArray();
        if (options?.Instructions != instructions &&
            !sent.Any(message => message.Role == ChatRole.System && message.Text == instructions))
            throw new InvalidOperationException("Expected the selected disk instructions.");
        using var payload = JsonDocument.Parse(sent.Single(message => message.Role == ChatRole.User).Text);
        if (payload.RootElement.GetProperty("source").GetString() != source ||
            payload.RootElement.GetProperty("idea").Deserialize<Idea>(json) != idea)
            throw new InvalidOperationException("Expected unchanged source content and first idea.");
        if (options?.ResponseFormat is not ChatResponseFormatJson { Schema: not null })
            throw new InvalidOperationException("Expected the production verdict JSON schema.");
        return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant,
            """{"blockingIssues":[{"problem":"Offline probe only","correction":"No model verdict"}]}""")));
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default) =>
        throw new NotSupportedException();

    public object? GetService(Type serviceType, object? serviceKey = null) => null;

    public void Dispose() { }
}
