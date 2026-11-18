using Nueyon.Compose.Application.Agents.Compose;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Application.Prompts;
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


foreach (var (stage, folder) in new[] { ("Compose", "Compose") })
{
    foreach (var file in Directory.GetFiles(Path.Combine(directory, "prompts", folder), "*.md"))
    {
        using var resource = typeof(ComposeAgent).Assembly.GetManifestResourceStream(
            $"Nueyon.Compose.Application.Prompts.{stage}.{Path.GetFileName(file)}")!;
        using var buffer = new MemoryStream();
        resource.CopyTo(buffer);
        if (!buffer.ToArray().SequenceEqual(File.ReadAllBytes(file)))
            throw new InvalidOperationException($"Embedded prompt mismatch: {file}");
    }
}

if (run == "preflight")
{
    Console.WriteLine("Frozen files and embedded prompts verified.");
    return;
}

var config = JsonSerializer.Deserialize<ExperimentConfig>(Read("config.json"), json)!;
if (!config.Schedule.Contains(run))
    throw new ArgumentException("Run ID is outside the fixed schedule.");


var fixture = run[..run.LastIndexOf("-c", StringComparison.Ordinal)];
var brief = JsonSerializer.Deserialize<NarrativeResult>(Read($"fixtures/{fixture}/result.json"), json)!;
if (brief.Content != Read($"fixtures/{fixture}/content.md"))
    throw new InvalidOperationException("Frozen brief differs from its result.");
var inputJson = JsonSerializer.Serialize(new ComposeInput(
    new NarrativeForCompose(brief.Content), new CompositionSpec(ContentFormat.LinkedInPost)), json);
using var promptScope = run.EndsWith("-B", StringComparison.Ordinal)
    ? PromptDirectory.Use(Path.Combine(directory, "variant"))
    : null;
var runDirectory = Path.Combine(directory, "calls", run);
using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};


var replay = args.Length == 3 && args[2] == "--replay";
if (replay && inputJson != Read($"calls/{run}/input.json"))
    throw new InvalidOperationException($"Input mismatch: {run}");

var credential = replay ? new ApiKeyCredential("offline-replay") : LoadCredential();
if (!replay)
{
    Directory.CreateDirectory(runDirectory);
    using (var marker = new FileStream(Path.Combine(runDirectory, "attempt.json"), FileMode.CreateNew))
        JsonSerializer.Serialize(marker, new { run, startedUtc = DateTimeOffset.UtcNow, config.Model }, json);
    File.WriteAllText(Path.Combine(runDirectory, "input.json"), inputJson);
}

static ApiKeyCredential LoadCredential()
{
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

return new ApiKeyCredential(key!);
}

using var http = new HttpClient(new CaptureHandler(runDirectory, config.Model, replay)) { Timeout = TimeSpan.FromMinutes(5) };
var client = new OpenAIClient(credential, new OpenAIClientOptions
{
    RetryPolicy = new ClientRetryPolicy(0),
    Transport = new HttpClientPipelineTransport(http)
});

using var chatClient = client.GetChatClient(config.Model).AsIChatClient();
try
{
    Console.WriteLine($"{run}: {(replay ? "replay" : "started")}");
    var context = new AgentExecutionContext(Guid.NewGuid());
    var agent = new ComposeAgent(chatClient.AsAIAgent(ComposeAgent.GetSystemInstructions(), "OpenAIAgent"),
        NullLogger<ComposeAgent>.Instance);
    var result = await agent.ExecuteAsync(context, JsonSerializer.Deserialize<ComposeInput>(inputJson, json)!,
        cancellation.Token);
    var output = (Json: JsonSerializer.Serialize(result, json), Content: result.Content);

    if (replay)
    {
        if (output.Json != Read($"calls/{run}/result.json") || output.Content != Read($"calls/{run}/content.md"))
            throw new InvalidOperationException($"Replay differs: {run}");
    }
    else
    {
        File.WriteAllText(Path.Combine(runDirectory, "content.md"), output.Content);
        File.WriteAllText(Path.Combine(runDirectory, "result.tmp"), output.Json);
        File.Move(Path.Combine(runDirectory, "result.tmp"), Path.Combine(runDirectory, "result.json"));
    }
    Console.WriteLine($"{run}: complete");
}
catch (Exception error)
{
    if (!replay)
        File.WriteAllText(Path.Combine(runDirectory, "error.txt"), $"{error.GetType().Name}: {error.Message}");
    throw;
}

sealed record ExperimentConfig(string Model, string[] Schedule);

sealed class CaptureHandler(string directory, string model, bool replay) : DelegatingHandler(new HttpClientHandler { AllowAutoRedirect = false })
{
    private int _count;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Interlocked.Increment(ref _count) != 1)
            throw new InvalidOperationException("Only one HTTP request is allowed.");
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        var requestPath = Path.Combine(directory, "request.json");
        if (replay)
        {
            if (body != File.ReadAllText(requestPath))
                throw new InvalidOperationException("Replay request differs.");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(File.ReadAllText(Path.Combine(directory, "response-raw.json")),
                    System.Text.Encoding.UTF8, "application/json")
            };
        }
        File.WriteAllText(requestPath, body);
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

