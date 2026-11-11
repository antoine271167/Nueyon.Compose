using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Nueyon.Compose.Experiments;

public static class OfflineChecks
{
    public static async Task RunAsync()
    {
        var root = Path.Combine(Path.GetTempPath(), "nueyon-offline-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "prompts"));
            File.WriteAllText(Path.Combine(root, "cases.json"), """[{"id":"test","split":"test","source":"source.txt"}]""");
            File.WriteAllText(Path.Combine(root, "source.txt"), "A proposal.");
            File.WriteAllText(Path.Combine(root, "prompts", "evidence.system.txt"), "Generate.");
            File.WriteAllText(Path.Combine(root, "prompts", "evidence.user.txt"), "{{SourceContent}}");
            File.WriteAllText(Path.Combine(root, "prompts", "review.system.txt"), "Review.");
            File.WriteAllText(Path.Combine(root, "prompts", "review.user.txt"), "{{SourceContent}} {{Draft}}");
            var draft = JsonSerializer.Serialize(new
            {
                ideas = new[] { new { title = "T", description = "D", audience = "A", rationale = "R", evidence = "E" } },
                grounding = new[] { "title", "description", "audience", "rationale", "evidence" }.Select(field =>
                    new { field, claim = "c", sourceEvidence = "A proposal.", inference = "i", constraints = "c" })
            });
            var idea = """{"ideas":[{"title":"T","description":"D","audience":"A","rationale":"R","evidence":"E"}]}""";
            var reject = """{"accepted":false,"blockingIssues":[{"category":"scope","field":"title","problem":"problem","sourceEvidence":"A proposal.","constraint":"constraint"}]}""";
            var accept = """{"accepted":true,"blockingIssues":[]}""";
            using var fake = new OfflineClient(idea, reject, idea, accept, draft, """{"review":[],"revised":""" + draft + "}");
            var ledger = new CallLedger(Path.Combine(root, "verification", "call-ledger.jsonl"));
            var runner = new ExperimentRunner(root, fake, ledger);
            foreach (var condition in new[] { "A", "B", "C" })
                if (await runner.RunAsync(new CellState("test", condition, 1), false) != "success")
                    throw new InvalidOperationException("Offline runner check failed.");
            if (fake.Calls != 6 || ledger.Count(new CellState("test", "A", 1)) != 4)
                throw new InvalidOperationException("Offline MAF retry check failed.");
            var attempts = File.ReadAllText(Path.Combine(root, "runs", "test", "1", "A", "attempts.jsonl"));
            if (!attempts.Contains("\"status\":\"rejected\"") || !attempts.Contains("\"status\":\"accepted\""))
                throw new InvalidOperationException("Offline observer check failed.");
            if (await runner.RunAsync(new CellState("test", "C", 1), false) != "skipped")
                throw new InvalidOperationException("Offline resume check failed.");
            var started = new CellState("test", "B", 2);
            Directory.CreateDirectory(started.DirectoryPath(root));
            File.WriteAllText(started.StartedPath(root), "{}");
            try { await runner.RunAsync(started, false); throw new Exception("Retried started cell."); }
            catch (InvalidOperationException) { }
            var failureCell = new CellState("test", "B", 3);
            using var failing = new FailingClient(() =>
            {
                if (new CallLedger(Path.Combine(root, "verification", "call-ledger.jsonl")).Count(failureCell) != 1 ||
                    !Directory.GetFiles(failureCell.DirectoryPath(root), "request.json", SearchOption.AllDirectories).Any())
                    throw new Exception("Reservation or request was not durable before dispatch.");
            });
            var failedRunner = new ExperimentRunner(root, failing, ledger);
            try { await failedRunner.RunAsync(failureCell, false); throw new Exception("Transport failure not propagated."); }
            catch (InvalidOperationException) { }
            if (await failedRunner.RunAsync(new CellState("test", "C", 3), false) != "blocked" || failing.Calls != 1)
                throw new InvalidOperationException("Offline B dependency check failed.");
            foreach (var invalid in new[] { "{}", draft.Replace("A proposal.", "not a quote"), draft.Replace("\"title\":\"T\"", "\"title\":\"T\",\"title\":\"T\"") })
            {
                try { ExperimentValidator.ParseDraft(invalid, "A proposal."); throw new Exception("Invalid draft accepted."); }
                catch (Exception ex) when (ex is JsonException or InvalidDataException) { }
            }
            try { ExperimentValidator.ParseReview("""{"review":[],"revised":{}}""", "A proposal."); throw new Exception("Invalid review accepted."); }
            catch (Exception ex) when (ex is JsonException or InvalidDataException) { }
            var schema = ExperimentValidator.Schema(false);
            if (schema.GetProperty("additionalProperties").GetBoolean() ||
                schema.GetProperty("properties").GetProperty("ideas").GetProperty("maxItems").GetInt32() != 3 ||
                ExperimentValidator.Schema(true).GetProperty("properties").GetProperty("revised").GetProperty("additionalProperties").GetBoolean())
                throw new InvalidOperationException("Offline schema check failed.");
            var capLedger = new CallLedger(Path.Combine(root, "cap", "ledger.jsonl"));
            for (var i = 0; i < CallLedger.Limit; i++) capLedger.Reserve(new CellState("case" + i, "B", 1));
            try { capLedger.Reserve(new CellState("overflow", "B", 1)); throw new Exception("Budget exceeded."); }
            catch (InvalidOperationException) { }
            if (File.ReadAllLines(Path.Combine(root, "cap", "ledger.jsonl")).Length != CallLedger.Limit)
                throw new InvalidOperationException("Offline ledger check failed.");
            var cellLedger = new CallLedger(Path.Combine(root, "cell-cap", "ledger.jsonl"));
            var a = new CellState("case", "A", 1);
            for (var i = 0; i < 6; i++) cellLedger.Reserve(a);
            try { cellLedger.Reserve(a); throw new Exception("Cell budget exceeded."); }
            catch (InvalidOperationException) { }
        }
        finally { Directory.Delete(root, true); }
    }

    private sealed class OfflineClient(params string[] responses) : IChatClient
    {
        public int Calls { get; private set; }
        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (options?.MaxOutputTokens != 6000 || options.Temperature is not null || options.ResponseFormat is not ChatResponseFormatJson)
                throw new InvalidOperationException("Incorrect offline request options.");
            return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, responses[Calls++])));
        }
        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();
        public object? GetService(Type serviceType, object? serviceKey = null) => null;
        public void Dispose() { }
    }

    private sealed class FailingClient(Action verify) : IChatClient
    {
        public int Calls { get; private set; }
        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            Calls++;
            verify();
            throw new InvalidOperationException("Offline transport failure.");
        }
        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();
        public object? GetService(Type serviceType, object? serviceKey = null) => null;
        public void Dispose() { }
    }
}
