using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Experiments;
using Xunit;

namespace Nueyon.Compose.Experiments.Tests;

public sealed class ExperimentTests
{
    private const string Source = "A proposal.";
    private const string Idea = """{"ideas":[{"title":"T","description":"D","audience":"A","rationale":"R","evidence":"E"}]}""";
    private const string Reject = """{"accepted":false,"blockingIssues":[{"category":"scope","field":"title","problem":"problem","sourceEvidence":"A proposal.","constraint":"constraint"}]}""";
    private const string Accept = """{"accepted":true,"blockingIssues":[]}""";
    private static string Draft => JsonSerializer.Serialize(new
    {
        ideas = new[] { new { title = "T", description = "D", audience = "A", rationale = "R", evidence = "E" } },
        grounding = new[] { "title", "description", "audience", "rationale", "evidence" }.Select(name =>
            new { field = name, claim = "c", sourceEvidence = Source, inference = "i", constraints = "c" })
    });

    [Fact]
    public async Task ProductionMafLoopRejectsThenAcceptsAndRecordsEveryRequest()
    {
        using var fixture = new Fixture();
        using var fake = new FakeClient(Idea, Reject, Idea.Replace("\"T\"", "\"T2\""), Accept);
        var cell = new CellState("test", "A", 1);
        Assert.Equal("success", await fixture.Runner(fake).RunAsync(cell, false));
        Assert.Equal(4, fake.Calls);
        Assert.All(fake.Options, o => { Assert.Equal(6000, o.MaxOutputTokens); Assert.Null(o.Temperature); Assert.IsType<ChatResponseFormatJson>(o.ResponseFormat); });
        Assert.Equal(4, fixture.Ledger.Count(cell));
        var dir = cell.DirectoryPath(fixture.Root);
        var attempts = File.ReadAllText(Path.Combine(dir, "attempts.jsonl"));
        Assert.Contains("\"status\":\"rejected\"", attempts);
        Assert.Contains("\"status\":\"accepted\"", attempts);
        using var final = JsonDocument.Parse(File.ReadAllText(cell.FinalPath(fixture.Root)));
        Assert.Equal("T2", final.RootElement.GetProperty("ideas")[0].GetProperty("title").GetString());
        for (var i = 1; i <= 4; i++)
        {
            var call = Path.Combine(dir, $"call-{i:D3}");
            using var request = JsonDocument.Parse(File.ReadAllText(Path.Combine(call, "request.json")));
            Assert.NotNull(request.RootElement.GetProperty("options").GetProperty("responseFormat").GetProperty("schema").GetProperty("properties").ToString());
            Assert.True(File.Exists(Path.Combine(call, "response.txt")));
            Assert.True(File.Exists(Path.Combine(call, "metadata.json")));
        }
        Assert.Contains(Source, fake.Messages[0].Single(m => m.Role == ChatRole.User).Text);
        Assert.DoesNotContain(fake.Messages[2], m => m.Role == ChatRole.Assistant);
    }

    [Fact]
    public async Task BAndCUseExactDraftAndFreshReviewAndResume()
    {
        using var fixture = new Fixture();
        using var fake = new FakeClient(Draft, """{"review":[],"revised":""" + Draft + "}");
        var runner = fixture.Runner(fake);
        var b = new CellState("test", "B", 1);
        var c = new CellState("test", "C", 1);
        Assert.Equal("success", await runner.RunAsync(b, false));
        Assert.Equal("success", await runner.RunAsync(c, false));
        Assert.Equal("skipped", await runner.RunAsync(c, false));
        Assert.Equal(2, fake.Calls);
        Assert.Contains(Draft, fake.Messages[1].Single(m => m.Role == ChatRole.User).Text);
        Assert.DoesNotContain(fake.Messages[1], m => m.Role == ChatRole.Assistant);
        Assert.Equal(Draft, File.ReadAllText(Path.Combine(c.DirectoryPath(fixture.Root), "draft.json")));
        using var final = JsonDocument.Parse(File.ReadAllText(c.FinalPath(fixture.Root)));
        Assert.Equal("T", final.RootElement.GetProperty("revised").GetProperty("ideas")[0].GetProperty("title").GetString());
        var started = new CellState("test", "B", 2);
        Directory.CreateDirectory(started.DirectoryPath(fixture.Root));
        File.WriteAllText(started.StartedPath(fixture.Root), "{}");
        await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunAsync(started, false));
        Assert.Equal(2, fake.Calls);
    }

    [Fact]
    public async Task FailedBBlocksCWithoutDispatch()
    {
        using var fixture = new Fixture();
        using var fake = new FakeClient("{}");
        var runner = fixture.Runner(fake);
        await Assert.ThrowsAnyAsync<Exception>(() => runner.RunAsync(new CellState("test", "B", 1), false));
        Assert.Equal("blocked", await runner.RunAsync(new CellState("test", "C", 1), false));
        Assert.Equal(1, fake.Calls);
    }

    [Fact]
    public async Task InvalidPersistedDraftBlocksReview()
    {
        using var fixture = new Fixture();
        using var fake = new FakeClient(Draft);
        var runner = fixture.Runner(fake);
        var b = new CellState("test", "B", 1);
        await runner.RunAsync(b, false);
        File.WriteAllText(Path.Combine(b.DirectoryPath(fixture.Root), "draft.json"), "{}");
        Assert.Equal("blocked", await runner.RunAsync(new CellState("test", "C", 1), false));
        Assert.Equal(1, fake.Calls);
    }

    [Fact]
    public async Task ReservationIsDurableBeforeFailingTransport()
    {
        using var fixture = new Fixture();
        var cell = new CellState("test", "B", 1);
        using var fake = new FakeClient(() =>
        {
            Assert.Equal(1, new CallLedger(fixture.LedgerPath).Count(cell));
            Assert.True(File.Exists(Path.Combine(cell.DirectoryPath(fixture.Root), "call-001", "request.json")));
            throw new InvalidOperationException("transport failed");
        });
        await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Runner(fake).RunAsync(cell, false));
        Assert.True(File.Exists(Path.Combine(cell.DirectoryPath(fixture.Root), "call-001", "error.json")));
        using var final = JsonDocument.Parse(File.ReadAllText(cell.FinalPath(fixture.Root)));
        Assert.Equal("failed", final.RootElement.GetProperty("status").GetString());
        Assert.Equal(1, final.RootElement.GetProperty("callCount").GetInt32());
    }

    [Fact]
    public void BudgetPersistsAndEnforcesBothCaps()
    {
        using var fixture = new Fixture();
        var a = new CellState("test", "A", 1);
        for (var i = 0; i < 6; i++) fixture.Ledger.Reserve(a);
        Assert.Throws<InvalidOperationException>(() => fixture.Ledger.Reserve(a));
        var b = new CellState("test", "B", 1);
        fixture.Ledger.Reserve(b);
        Assert.Throws<InvalidOperationException>(() => fixture.Ledger.Reserve(b));
        for (var i = 0; i < 105; i++) fixture.Ledger.Reserve(new CellState("case" + i, "C", 1));
        var reloaded = new CallLedger(fixture.LedgerPath);
        Assert.Equal(6, reloaded.Count(a));
        Assert.Throws<InvalidOperationException>(() => reloaded.Reserve(new CellState("last", "B", 1)));
        Assert.Equal(112, File.ReadAllLines(fixture.LedgerPath).Length);
    }

    [Fact]
    public async Task ExhaustedBudgetDoesNotReachTransport()
    {
        using var fixture = new Fixture();
        var cell = new CellState("test", "B", 1);
        fixture.Ledger.Reserve(cell);
        using var fake = new FakeClient(Draft);
        using var recorder = new RecordingChatClient(fake, fixture.Ledger, cell, fixture.Root);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            recorder.GetResponseAsync([new ChatMessage(ChatRole.User, "test")]));
        Assert.Equal(0, fake.Calls);
        Assert.Single(File.ReadAllLines(fixture.LedgerPath));
    }

    [Fact]
    public void StrictDraftAndReviewValidation()
    {
        Assert.Single(ExperimentValidator.ParseDraft(Draft, Source).Ideas);
        Assert.Empty(ExperimentValidator.ParseReview("""{"review":[],"revised":""" + Draft + "}", Source).Review);
        var invalid = new[]
        {
            "```json\n" + Draft + "\n```", Draft.Replace("\"T\"", "\" \""),
            Draft.Replace("\"title\":\"T\",", ""), Draft.Replace("\"title\":\"T\"", "\"title\":\"T\",\"title\":\"T\""),
            Draft.Replace("\"title\":\"T\"", "\"title\":\"T\",\"extra\":1"),
            Draft.Replace("\"field\":\"title\"", "\"field\":\"other\""),
            Draft.Replace(Source, "wrong quote"), Draft.Replace("\"ideas\":[{", "\"ideas\":[{").Replace("\"grounding\":[", "\"grounding\":[] , \"unused\":["),
            Draft.Replace("\"field\":\"evidence\"", "\"field\":\"title\""),
            """{"ideas":[],"grounding":[]}"""
        };
        foreach (var value in invalid)
            Assert.ThrowsAny<Exception>(() => ExperimentValidator.ParseDraft(value, Source));
        foreach (var item in new[]
        {
            """{"field":"bad","issue":"i","sourceEvidence":"A proposal.","action":"keep"}""",
            """{"field":"title","issue":" ","sourceEvidence":"A proposal.","action":"keep"}""",
            """{"field":"title","issue":"i","sourceEvidence":"wrong","action":"keep"}""",
            """{"field":"title","issue":"i","sourceEvidence":"A proposal.","action":"change"}"""
        })
            Assert.ThrowsAny<Exception>(() => ExperimentValidator.ParseReview("""{"review":[""" + item + """],"revised":""" + Draft + "}", Source));
        Assert.ThrowsAny<Exception>(() => ExperimentValidator.ParseReview("""{"review":[],"revised":""" + Idea + "}", Source));
    }

    [Fact]
    public void ValidatorEnforcesCardinalityAndStrictReview()
    {
        using var doc = JsonDocument.Parse(Draft);
        var idea = doc.RootElement.GetProperty("ideas")[0].GetRawText();
        var grounding = doc.RootElement.GetProperty("grounding").EnumerateArray().Select(e => e.GetRawText()).ToArray();
        string Payload(int ideaCount, int groundingCount) =>
            "{\"ideas\":[" + string.Join(",", Enumerable.Repeat(idea, ideaCount)) +
            "],\"grounding\":[" + string.Join(",", Enumerable.Range(0, groundingCount).Select(i => grounding[i % grounding.Length])) + "]}";
        Assert.Equal(3, ExperimentValidator.ParseDraft(Payload(3, 12), Source).Ideas.Count);
        foreach (var (ideas, records) in new[] { (0, 5), (4, 5), (1, 0), (1, 13) })
            Assert.Throws<InvalidDataException>(() => ExperimentValidator.ParseDraft(Payload(ideas, records), Source));
        foreach (var review in new[]
        {
            """{"review":[],"review":[],"revised":""" + Draft + "}",
            """{"review":[],"extra":1,"revised":""" + Draft + "}",
            """{"review":[],"revised":""" + Draft.Replace("\"claim\":\"c\"", "\"claim\":\" \"") + "}"
        })
            Assert.ThrowsAny<Exception>(() => ExperimentValidator.ParseReview(review, Source));
    }

    [Fact]
    public void SchemasRequireAllFieldsAndForbidExtras()
    {
        var draft = ExperimentValidator.Schema(false);
        var review = ExperimentValidator.Schema(true);
        Assert.False(draft.GetProperty("additionalProperties").GetBoolean());
        Assert.Equal(new[] { "ideas", "grounding" }, draft.GetProperty("required").EnumerateArray().Select(x => x.GetString()));
        var properties = draft.GetProperty("properties");
        Assert.Equal(3, properties.GetProperty("ideas").GetProperty("maxItems").GetInt32());
        var grounding = properties.GetProperty("grounding");
        Assert.Equal(12, grounding.GetProperty("maxItems").GetInt32());
        Assert.False(grounding.GetProperty("items").GetProperty("additionalProperties").GetBoolean());
        Assert.Equal(new[] { "review", "revised" }, review.GetProperty("required").EnumerateArray().Select(x => x.GetString()));
        Assert.False(review.GetProperty("properties").GetProperty("revised").GetProperty("additionalProperties").GetBoolean());
    }

    [Fact]
    public void FreezeRejectsTamperAndAcceptsExtraMetadata()
    {
        var root = Path.Combine(AppContext.BaseDirectory, "freeze-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "verification"));
        try
        {
            File.WriteAllText(Path.Combine(root, "file.txt"), "original");
            var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(Path.Combine(root, "file.txt")))).ToLowerInvariant();
            var process = Process.Start(new ProcessStartInfo("git", "rev-parse HEAD") { WorkingDirectory = root, RedirectStandardOutput = true, UseShellExecute = false })!;
            var head = process.StandardOutput.ReadToEnd().Trim(); process.WaitForExit(); Assert.Equal(0, process.ExitCode);
            File.WriteAllText(Path.Combine(root, "verification", "frozen.json"), JsonSerializer.Serialize(new { files = new Dictionary<string, string> { ["file.txt"] = hash }, head, budget = 112, frozenUtc = DateTime.UtcNow }));
            FreezeVerifier.Verify(root);
            File.WriteAllText(Path.Combine(root, "file.txt"), "tampered");
            Assert.Throws<InvalidDataException>(() => FreezeVerifier.Verify(root));
        }
        finally { Directory.Delete(root, true); }
    }

    private sealed class Fixture : IDisposable
    {
        public string Root { get; } = Path.Combine(Path.GetTempPath(), "experiment-test-" + Guid.NewGuid().ToString("N"));
        public string LedgerPath => Path.Combine(Root, "verification", "call-ledger.jsonl");
        public CallLedger Ledger { get; }
        public Fixture()
        {
            Directory.CreateDirectory(Path.Combine(Root, "prompts"));
            File.WriteAllText(Path.Combine(Root, "cases.json"), """[{"id":"test","split":"test","source":"source.txt"}]""");
            File.WriteAllText(Path.Combine(Root, "source.txt"), Source);
            File.WriteAllText(Path.Combine(Root, "prompts", "evidence.system.txt"), "Generate");
            File.WriteAllText(Path.Combine(Root, "prompts", "evidence.user.txt"), "{{SourceContent}}");
            File.WriteAllText(Path.Combine(Root, "prompts", "review.system.txt"), "Review");
            File.WriteAllText(Path.Combine(Root, "prompts", "review.user.txt"), "{{SourceContent}} {{Draft}}");
            Ledger = new CallLedger(LedgerPath);
        }
        public ExperimentRunner Runner(IChatClient client) => new(Root, client, Ledger);
        public void Dispose() => Directory.Delete(Root, true);
    }

    private sealed class FakeClient : IChatClient
    {
        private readonly Queue<string> _responses;
        private readonly Action? _onCall;
        public int Calls { get; private set; }
        public List<ChatOptions> Options { get; } = [];
        public List<ChatMessage[]> Messages { get; } = [];
        public FakeClient(params string[] responses) => _responses = new Queue<string>(responses);
        public FakeClient(Action onCall) { _responses = new Queue<string>(); _onCall = onCall; }
        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            Calls++; Options.Add(options!); Messages.Add(messages.ToArray()); _onCall?.Invoke();
            return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, _responses.Dequeue())));
        }
        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();
        public object? GetService(Type serviceType, object? serviceKey = null) => null;
        public void Dispose() { }
    }
}
