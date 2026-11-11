using System.Reflection;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Host.Console;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;

var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
var (mode, promptDirectory, caseFile, id, output) = (args[0], args[1], args[2], args[3], args[4]);
var selected = JsonSerializer.Deserialize<Fixture[]>(File.ReadAllText(caseFile), json)!.Single(item => item.Id == id);
var source = File.ReadAllText(Path.Combine(Path.GetDirectoryName(caseFile)!, "sources", selected.Source));
var embedded = IdeaSemanticJudge.GetSystemInstructions();
using var scope = PromptDirectory.Use(promptDirectory);
var instructions = IdeaSemanticJudge.GetSystemInstructions();
if (Path.GetFileName(promptDirectory) == "baseline" && instructions != embedded)
    throw new InvalidOperationException("Baseline snapshot differs from the embedded prompt.");

var offline = mode == "offline";
IChatClient? liveClient = null;
const string alias = "gpt-5-mini";
if (!offline)
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Path.GetFullPath("src/Nueyon.Compose.Host.Console"))
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddEnvironmentVariables()
        .AddUserSecrets(typeof(Cli).Assembly, true)
        .Build();
    if (config["OpenAI:Model"] != alias || string.IsNullOrWhiteSpace(config["OpenAI:ApiKey"]))
        throw new InvalidOperationException("Expected configured gpt-5-mini and credentials.");
    var client = new OpenAIClient(new ApiKeyCredential(config["OpenAI:ApiKey"]!),
        new OpenAIClientOptions { RetryPolicy = new ClientRetryPolicy(0), NetworkTimeout = TimeSpan.FromSeconds(90) });
    liveClient = client.GetChatClient(alias).AsIChatClient();
}

var method = typeof(IdeaSemanticJudge).GetMethod("FindBlockingIssuesAsync",
    BindingFlags.Instance | BindingFlags.NonPublic)!;
async Task RunAsync(string? response, bool valid, string label)
{
    using var probe = new Probe(source, selected.Idea, instructions, json, output, liveClient, response);
    var judge = new IdeaSemanticJudge(probe.AsAIAgent(instructions, "OpenAIAgent"));
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(100));
    var task = (Task)method.Invoke(judge, [source, selected.Idea, timeout.Token])!;
    try
    {
        await task;
    }
    catch (InvalidOperationException ex) when (!valid && ex.Message.StartsWith("Idea semantic validation returned an invalid verdict"))
    {
        Console.WriteLine($"Rejected {label}: {ex.Message}");
        return;
    }
    if (!valid) throw new InvalidOperationException($"Accepted malformed verdict {label}.");
    if (probe.Calls != 1) throw new InvalidOperationException("Expected exactly one judge call.");
    Console.WriteLine($"Valid verdict for {id}: {label}");
}

if (!offline)
{
    await RunAsync(null, true, alias);
    return;
}

const string blocker = """{"category":"scope","field":"rationale","problem":"Offline probe","sourceEvidence":"Offline probe","constraint":"Offline probe"}""";
await RunAsync("""{"accepted":true,"blockingIssues":[]}""", true, "accept");
await RunAsync("""{"accepted":false,"blockingIssues":[""" + blocker + "]}", true, "block");
await RunAsync("""{"accepted":true,"blockingIssues":[""" + blocker + "]}", false, "inconsistent");
await RunAsync("""{"accepted":false,"blockingIssues":[""" + string.Join(",", Enumerable.Repeat(blocker, 4)) + "]}", false, "four blockers");
await RunAsync("""{"accepted":false,"blockingIssues":[{"problem":"x","correction":"y"}]}""", false, "old schema");
await RunAsync("""{"accepted":false,"blockingIssues":[""" + blocker.Replace("Offline probe", " ") + "]}", false, "blank fields");
await RunAsync("""{"accepted":true,"accepted":true,"blockingIssues":[]}""", false, "duplicate field");
await RunAsync("""{"accepted":true}""", false, "missing array");

sealed record Fixture(string Id, string Source, Idea Idea);

sealed class Probe(string source, Idea idea, string instructions, JsonSerializerOptions json, string output,
    IChatClient? liveClient, string? offlineResponse) : IChatClient
{
    public int Calls { get; private set; }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (++Calls != 1) throw new InvalidOperationException("Additional model call blocked.");
        var sent = messages.ToArray();
        if (options?.Instructions != instructions &&
            !sent.Any(message => message.Role == ChatRole.System && message.Text == instructions))
            throw new InvalidOperationException("Prompt was not delivered.");
        var user = sent.Single(message => message.Role == ChatRole.User).Text;
        using var payload = JsonDocument.Parse(user);
        if (payload.RootElement.GetProperty("source").GetString() != source ||
            payload.RootElement.GetProperty("idea").Deserialize<Idea>(json) != idea ||
            payload.RootElement.EnumerateObject().Count() != 2)
            throw new InvalidOperationException("Judge must receive only the unchanged source and selected idea.");
        if (options?.ResponseFormat is not ChatResponseFormatJson { Schema: { } schema })
            throw new InvalidOperationException("Missing response schema.");
        var properties = schema.GetProperty("properties");
        var fields = properties.GetProperty("blockingIssues").GetProperty("items").GetProperty("properties");
        if (!properties.EnumerateObject().Select(p => p.Name).Order().SequenceEqual(new[] { "accepted", "blockingIssues" }) ||
            !fields.EnumerateObject().Select(p => p.Name).Order().SequenceEqual(new[] { "category", "constraint", "field", "problem", "sourceEvidence" }))
            throw new InvalidOperationException("Unexpected response schema.");

        if (liveClient is null)
        {
            File.WriteAllText(output + ".schema.json", schema.GetRawText());
            return new ChatResponse(new ChatMessage(ChatRole.Assistant, offlineResponse!));
        }
        File.WriteAllText(output + ".input.json", user);
        var result = await liveClient.GetResponseAsync(sent, options, cancellationToken);
        File.WriteAllText(output + ".verdict.json", result.Text);
        File.WriteAllText(output + ".metadata.json", JsonSerializer.Serialize(new
        {
            result.ModelId, result.ResponseId, result.Usage, finishReason = result.FinishReason?.ToString()
        }, json));
        return result;
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();
    public object? GetService(Type serviceType, object? serviceKey = null) => null;
    public void Dispose() { }
}
