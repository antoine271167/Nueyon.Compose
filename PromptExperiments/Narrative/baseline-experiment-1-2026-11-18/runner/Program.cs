using System.ClientModel;
using System.ClientModel.Primitives;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents.Narrative;
using Nueyon.Compose.Domain;
using OpenAI;

var directory = Path.GetFullPath(args[0]);
var run = args[1];
var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
string Read(string path) => File.ReadAllText(Path.Combine(directory, path));

foreach (var item in JsonSerializer.Deserialize<Dictionary<string, string>>(Read("freeze.json"))!)
{
    var hash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(directory, item.Key))));
    if (hash != item.Value)
        throw new InvalidOperationException($"Freeze mismatch: {item.Key}");
}

foreach (var name in new[] { "system", "user" })
{
    var filename = $"Narrative.{name}.prompt.md";
    using var resource = typeof(NarrativeAgent).Assembly.GetManifestResourceStream(
        $"Nueyon.Compose.Application.Prompts.Narrative.{filename}")!;
    using var buffer = new MemoryStream();
    resource.CopyTo(buffer);
    if (!buffer.ToArray().SequenceEqual(File.ReadAllBytes(Path.Combine(directory, "prompts", "Narrative", filename))))
        throw new InvalidOperationException($"Embedded prompt mismatch: {filename}");
}

if (run == "preflight")
{
    Console.WriteLine("Frozen files and embedded prompts verified.");
    return;
}

var config = JsonSerializer.Deserialize<ExperimentConfig>(Read("config.json"), json)!;
if (!config.Schedule.Contains(run))
    throw new ArgumentException("Run ID is outside the fixed schedule.");

var inputPath = $"cases/{run.Split('-')[0]}/input.json";
var input = JsonSerializer.Deserialize<NarrativeInput>(Read(inputPath), json)!;
var system = NarrativeAgent.GetSystemInstructions();
var runDirectory = Path.Combine(directory, "calls", run);
using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};

if (args.Length == 3 && args[2] == "--replay")
{
    using var response = JsonDocument.Parse(File.ReadAllText(Path.Combine(runDirectory, "response-raw.json")));
    var content = response.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;
    using var replay = new ReplayClient(content);
    var result = await Execute(replay);
    if (JsonSerializer.Serialize(result, json) != File.ReadAllText(Path.Combine(runDirectory, "result.json")))
        throw new InvalidOperationException($"Replay differs: {run}");
    Console.WriteLine($"{run}: replay verified");
    return;
}

var secretsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json");
var key = Environment.GetEnvironmentVariable("OpenAI__ApiKey");
if (string.IsNullOrWhiteSpace(key))
{
    using var secrets = JsonDocument.Parse(File.ReadAllText(secretsPath));
    var root = secrets.RootElement;
    key = root.TryGetProperty("OpenAI:ApiKey", out var flat)
        ? flat.GetString()
        : root.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
}
var credential = new ApiKeyCredential(key!);
Directory.CreateDirectory(runDirectory);
using (var marker = new FileStream(Path.Combine(runDirectory, "attempt.json"), FileMode.CreateNew))
    JsonSerializer.Serialize(marker, new { run, startedUtc = DateTimeOffset.UtcNow, config.Model, config.MaxOutputTokens }, json);

File.Copy(Path.Combine(directory, inputPath), Path.Combine(runDirectory, "input.json"));
using var http = new HttpClient(new CaptureHandler(runDirectory, config.Model)) { Timeout = TimeSpan.FromMinutes(5) };
var client = new OpenAIClient(credential, new OpenAIClientOptions
{
    RetryPolicy = new ClientRetryPolicy(0),
    Transport = new HttpClientPipelineTransport(http)
});
using var cappedClient = new CappedClient(client.GetChatClient(config.Model).AsIChatClient(), config.MaxOutputTokens);
try
{
    Console.WriteLine($"{run}: started");
    var result = await Execute(cappedClient);
    File.WriteAllText(Path.Combine(runDirectory, "result.json"), JsonSerializer.Serialize(result, json));
    File.WriteAllText(Path.Combine(runDirectory, "content.md"), result.Content);
    Console.WriteLine($"{run}: complete");
}
catch (Exception error)
{
    File.WriteAllText(Path.Combine(runDirectory, "error.txt"), $"{error.GetType().Name}: {error.Message}");
    throw;
}

Task<NarrativeResult> Execute(IChatClient chatClient) =>
    new NarrativeAgent(chatClient.AsAIAgent(system, "OpenAIAgent"), NullLogger<NarrativeAgent>.Instance)
        .ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), input, cancellation.Token);

sealed record ExperimentConfig(string Model, int MaxOutputTokens, string[] Schedule);

sealed class CappedClient(IChatClient inner, int maxTokens) : DelegatingChatClient(inner)
{
    public override Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new();
        options.MaxOutputTokens = maxTokens;
        return base.GetResponseAsync(messages, options, cancellationToken);
    }
}

sealed class CaptureHandler(string directory, string model) : DelegatingHandler(new HttpClientHandler { AllowAutoRedirect = false })
{
    private int _count;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Interlocked.Increment(ref _count) != 1)
            throw new InvalidOperationException("Only one HTTP request is allowed.");
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);
        File.WriteAllText(Path.Combine(directory, "request.json"), body);
        var response = await base.SendAsync(request, cancellationToken);
        try
        {
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            File.WriteAllText(Path.Combine(directory, "response-raw.json"), raw);
            File.WriteAllText(Path.Combine(directory, "http.json"),
                JsonSerializer.Serialize(new { status = (int)response.StatusCode, endedUtc = DateTimeOffset.UtcNow }));
            if (response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(raw);
                var root = document.RootElement;
                if (root.GetProperty("model").GetString() != model ||
                    root.GetProperty("choices")[0].GetProperty("finish_reason").GetString() != "stop")
                    throw new InvalidOperationException("Unexpected model or finish reason.");
            }
            return response;
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }
}

sealed class ReplayClient(string content) : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant, content)]));

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();

    public object? GetService(Type serviceType, object? serviceKey = null) => null;
    public void Dispose() { }
}

