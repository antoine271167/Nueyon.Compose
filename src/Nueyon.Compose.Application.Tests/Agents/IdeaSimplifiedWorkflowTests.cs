using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Domain;
using Xunit;

#pragma warning disable MAAI001

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     The Simplified Idea workflow through the real MAF LoopAgent, evaluator, judge and parser with fake chat clients.
///     These tests prove the budget, isolation and fail-closed behavior. They cannot show that a real model detects
///     any particular source problem.
/// </summary>
public sealed class IdeaSimplifiedWorkflowTests
{
    private const string Source = "A proposal, with no measured results.";
    private const string Accepted = """{"accepted":true,"blockingIssues":[]}""";

    private static string Generation(string title = "Valid Idea", string description = "A valid description") =>
        JsonSerializer.Serialize(new
        {
            ideas = new[]
            {
                new
                {
                    title, description, audience = "Readers", rationale = "Good rationale", evidence = "The proposal."
                }
            }
        });

    private static string StructuralInvalid => Generation(title: "");

    private static string Rejected(string problem = "The title claims a result.", string category = "unsupportedClaim",
        string field = "title") =>
        JsonSerializer.Serialize(new
        {
            accepted = false,
            blockingIssues = new[]
            {
                new
                {
                    category, field, problem, sourceEvidence = Source,
                    constraint = "The source establishes a proposal only."
                }
            }
        });

    private static IdeaAgent CreateAgent(IChatClient generator, IChatClient judge,
        IdeaWorkflowMode mode = IdeaWorkflowMode.Simplified)
    {
        var semanticJudge = new IdeaSemanticJudge(judge.AsAIAgent("Judge", "TestIdeaJudge"), mode);
        var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), semanticJudge);
        Assert.Equal(mode, evaluator.Mode);
        return new IdeaAgent(evaluator.Wrap(generator.AsAIAgent("Generate", "TestIdeaAgent")),
            NullLogger<IdeaAgent>.Instance, mode);
    }

    private static AgentExecutionContext Observed(Action<IdeaValidationAttempt> observer) => new(Guid.NewGuid())
    {
        AdditionalProperties = new Dictionary<string, object?> { [IdeaValidationLoopEvaluator.ObserverKey] = observer }
    };

    private static AgentExecutionContext Plain() => new(Guid.NewGuid());

    [Fact]
    public async Task DirectAcceptance_UsesOneGenerationAndOneJudgement()
    {
        var generator = new FakeChatClient(Generation());
        var judge = new FakeChatClient(Accepted);

        var ideas = await CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source));

        Assert.Equal("Valid Idea", Assert.Single(ideas).Title);
        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
        var task = Assert.Single(generator.Requests[0], m => m.Role == ChatRole.User).Text;
        Assert.Equal(IdeaPrompts.GeneratorUser(IdeaWorkflowMode.Simplified, Source), task);
        Assert.NotEqual(IdeaPrompts.GeneratorUser(IdeaWorkflowMode.Legacy, Source), task);
    }

    [Fact]
    public async Task SemanticRejection_RepairsOnceFromOriginalMessagesAndRejudgesInFreshContext()
    {
        var generator = new FakeChatClient(Generation("First", "Overclaims"), Generation("Second", "Revised"));
        var rejection = Rejected("The title claims a result.");
        var judge = new FakeChatClient(rejection, Accepted);
        var events = new List<IdeaValidationAttempt>();

        var ideas = await CreateAgent(generator, judge).ExecuteAsync(Observed(events.Add), new StoryInput(Source));

        Assert.Equal("Second", Assert.Single(ideas).Title);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(2, judge.InvocationCount);

        var initial = Assert.Single(generator.Requests[0], m => m.Role == ChatRole.User).Text;
        var repair = generator.Requests[1];
        Assert.DoesNotContain(repair, m => m.Role == ChatRole.Assistant);
        var users = repair.Where(m => m.Role == ChatRole.User).Select(m => m.Text).ToArray();
        Assert.Equal(2, users.Length);
        Assert.Equal(initial, users[0]);
        var lines = users[1].Split('\n');
        Assert.Contains("only if the diagnosis explicitly says the angle itself is wrong", users[1]);
        Assert.StartsWith("Previous first idea: ", lines[^2]);
        using (var previous = JsonDocument.Parse(lines[^2]["Previous first idea: ".Length..]))
        {
            Assert.Equal("First", previous.RootElement.GetProperty("title").GetString());
        }

        using var diagnosis = JsonDocument.Parse(lines[^1]["Diagnosis: ".Length..]);
        Assert.Equal("The title claims a result.",
            diagnosis.RootElement[0].GetProperty("problem").GetString());

        // The re-judgement sees only the source and the revised idea, never the earlier verdict.
        Assert.DoesNotContain(judge.Requests[1], m => m.Role == ChatRole.Assistant);
        var rejudged = Assert.Single(judge.Requests[1], m => m.Role == ChatRole.User).Text;
        using var payload = JsonDocument.Parse(rejudged);
        Assert.Equal(["source", "idea"], payload.RootElement.EnumerateObject().Select(p => p.Name));
        Assert.Equal(Source, payload.RootElement.GetProperty("source").GetString());
        Assert.Equal("Second", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());
        Assert.DoesNotContain("claims a result", rejudged);
        Assert.Equal(["rejected", "accepted"], events.Where(e => e.Status != "incomplete").Select(e => e.Status));
    }

    [Fact]
    public async Task TwoSemanticRejections_StopWithExplicitDiagnosisBeforeAThirdGeneration()
    {
        var generator = new FakeChatClient(Generation(), Generation(), Generation());
        var judge = new FakeChatClient(Rejected("First problem."), Rejected("Last problem."), Accepted);

        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Equal("Idea agent failed to produce valid ideas after 2 attempts.", error.Message);
        Assert.Equal(2, error.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Semantic, error.Kind);
        Assert.Contains("Last problem.", error.Diagnosis);
        Assert.DoesNotContain("First problem.", error.Diagnosis);
        var blocker = Assert.Single(error.Blockers);
        Assert.Equal(new IdeaValidationBlocker("unsupportedClaim", "title", "Last problem."), blocker);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(2, judge.InvocationCount);
    }

    [Fact]
    public async Task StructuralFailureThenValid_UsesTheOneRepairAndOneJudgement()
    {
        var generator = new FakeChatClient(StructuralInvalid, Generation());
        var judge = new FakeChatClient(Accepted);

        var ideas = await CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source));

        Assert.Single(ideas);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
        Assert.Contains("Previous first idea: ", generator.Requests[1].Last().Text);
    }

    [Fact]
    public async Task StructuralRepairThatIsSemanticallyRejected_GetsNoExtraBudget()
    {
        var generator = new FakeChatClient(StructuralInvalid, Generation(), Generation());
        var judge = new FakeChatClient(Rejected("Overclaim."), Accepted);

        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Equal(2, error.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Semantic, error.Kind);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task TwoStructuralFailures_NeverCallTheJudge()
    {
        var generator = new FakeChatClient(StructuralInvalid, "not json", Generation());
        var judge = new FakeChatClient(Accepted);

        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Equal(IdeaValidationFailureKind.Structural, error.Kind);
        Assert.Equal(2, error.AttemptCount);
        Assert.Empty(error.Blockers);
        Assert.NotEmpty(error.Diagnosis);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(0, judge.InvocationCount);
    }

    [Theory]
    [InlineData("unknownCategory", "title")]
    [InlineData("unsupportedClaim", "summary")]
    [InlineData("UnsupportedClaim", "title")]
    [InlineData("claim", "title")]
    public async Task SimplifiedJudge_OutsideTheAllowlistFailsClosedWithoutRetry(string category, string field)
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Rejected(category: category, field: field));

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Theory]
    [InlineData("sourceContradiction", "title")]
    [InlineData("unsupportedClaim", "description")]
    [InlineData("editorialPurpose", "audience")]
    [InlineData("unsupportedClaim", "rationale")]
    [InlineData("sourceContradiction", "evidence")]
    public async Task SimplifiedJudge_AllowlistedCategoriesAndFieldsAreAccepted(string category, string field)
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Rejected(category: category, field: field), Accepted);

        var ideas = await CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source));

        Assert.Single(ideas);
        Assert.Equal(2, generator.InvocationCount);
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("{}")]
    [InlineData("""{"accepted":true,"blockingIssues":[],"extra":1}""")]
    public async Task SimplifiedJudge_MalformedVerdictFailsClosedWithoutRetry(string verdict)
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(verdict);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
    }

    [Fact]
    public async Task LegacyJudge_KeepsFreeFormCategoriesAndFields()
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Rejected(category: "anything", field: "whatever"), Accepted);

        var ideas = await CreateAgent(generator, judge, IdeaWorkflowMode.Legacy)
            .ExecuteAsync(Plain(), new StoryInput(Source));

        Assert.Single(ideas);
        Assert.Equal(2, generator.InvocationCount);
    }

    [Fact]
    public async Task JudgeTimeout_PropagatesWithoutGeneratorRetry()
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Accepted) { OnGetResponse = _ => throw new TimeoutException("judge") };

        await Assert.ThrowsAsync<TimeoutException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task CancellationDuringRepair_PropagatesWithoutJudgingOrExtraGeneration()
    {
        using var cts = new CancellationTokenSource();
        var calls = 0;
        var cancelling = new FakeChatClient(StructuralInvalid, Generation())
        {
            OnGetResponse = token =>
            {
                if (++calls == 2)
                {
                    cts.Cancel();
                    token.ThrowIfCancellationRequested();
                }
            }
        };
        var judge = new FakeChatClient(Accepted);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            CreateAgent(cancelling, judge).ExecuteAsync(Plain(), new StoryInput(Source), cts.Token));

        Assert.Equal(2, cancelling.InvocationCount);
        Assert.Equal(0, judge.InvocationCount);
    }

    [Fact]
    public async Task CancellationDuringJudgement_PropagatesWithoutRetry()
    {
        using var cts = new CancellationTokenSource();
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Accepted)
        {
            OnGetResponse = token =>
            {
                cts.Cancel();
                token.ThrowIfCancellationRequested();
            }
        };

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source), cts.Token));

        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task SemanticRejectionThenMalformedRepair_StopsStructurallyAfterOneJudgement()
    {
        var generator = new FakeChatClient(Generation(), StructuralInvalid, Generation());
        var judge = new FakeChatClient(Rejected("Overclaim."), Accepted);

        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Equal(IdeaValidationFailureKind.Structural, error.Kind);
        Assert.Equal(2, error.AttemptCount);
        Assert.NotEmpty(error.Diagnosis);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Theory]
    [InlineData("""{"accepted":true,"blockingIssues":[{"category":"unsupportedClaim","field":"title","problem":"p","sourceEvidence":"s","constraint":"c"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[]}""")]
    [InlineData("""{"accepted":true,"accepted":true,"blockingIssues":[]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"unsupportedClaim","field":"title","problem":"p","problem":"q","sourceEvidence":"s","constraint":"c"}]}""")]
    public async Task SimplifiedJudge_InconsistentOrDuplicateVerdictsFailClosed(string verdict)
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(verdict);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task SimplifiedJudge_MoreThanThreeBlockersFailsClosed()
    {
        var blocker = """{"category":"unsupportedClaim","field":"title","problem":"p","sourceEvidence":"s","constraint":"c"}""";
        var verdict = $$"""{"accepted":false,"blockingIssues":[{{blocker}},{{blocker}},{{blocker}},{{blocker}}]}""";
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(verdict);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
    }

    [Fact]
    public async Task Alternatives_AreToleratedAndNeverJudged_AndInvalidFirstIsNotReplaced()
    {
        var withBadAlternative =
            """{"ideas":[{"title":"First","description":"d","audience":"a","rationale":"r"},{"title":42,"description":["x"]}]}""";
        var generator = new FakeChatClient(withBadAlternative);
        var judge = new FakeChatClient(Accepted);
        var ideas = await CreateAgent(generator, judge).ExecuteAsync(Plain(), new StoryInput(Source));

        Assert.Equal(["First"], ideas.Select(i => i.Title));
        using var payload = JsonDocument.Parse(Assert.Single(judge.Requests[0], m => m.Role == ChatRole.User).Text);
        Assert.Equal("First", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());

        var invalidFirst =
            """{"ideas":[{"title":"","description":"d","audience":"a","rationale":"r"},{"title":"Second","description":"d","audience":"a","rationale":"r"}]}""";
        var second = new FakeChatClient(invalidFirst, invalidFirst);
        var secondJudge = new FakeChatClient(Accepted);
        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(second, secondJudge).ExecuteAsync(Plain(), new StoryInput(Source)));
        Assert.Equal(IdeaValidationFailureKind.Structural, error.Kind);
        Assert.Equal(0, secondJudge.InvocationCount);
    }

    [Fact]
    public async Task ConcurrentRuns_KeepSourceObserverAndModeSeparate()
    {
        var barrier = new Barrier(2);
        var generator = new SourceEchoClient(barrier);
        var judge = new SourceEchoClient(null);
        var simplified = CreateAgent(generator, judge);
        var first = new List<IdeaValidationAttempt>();
        var second = new List<IdeaValidationAttempt>();

        await Task.WhenAll(
            simplified.ExecuteAsync(Observed(first.Add), new StoryInput("source-A")),
            simplified.ExecuteAsync(Observed(second.Add), new StoryInput("source-B")));

        Assert.All(first, a => Assert.Contains("source-A", a.Response));
        Assert.All(second, a => Assert.Contains("source-B", a.Response));
        Assert.Equal(["rejected", "accepted"], first.Where(a => a.Status != "incomplete").Select(a => a.Status));
        Assert.Equal(["rejected", "accepted"], second.Where(a => a.Status != "incomplete").Select(a => a.Status));

        // Each judge payload pairs a source with the idea generated from that same source.
        var judged = judge.Judged.OrderBy(j => j.Source).ToArray();
        Assert.Equal(new[] { ("source-A", "source-A"), ("source-B", "source-B") }, judged);

        // Agents of different modes sharing nothing keep their own budgets when run together.
        var legacyGenerator = new FakeChatClient(StructuralInvalid, StructuralInvalid, StructuralInvalid, Generation());
        var simplifiedGenerator = new FakeChatClient(StructuralInvalid, StructuralInvalid, Generation());
        var legacyTask = Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(legacyGenerator, new FakeChatClient(Accepted), IdeaWorkflowMode.Legacy)
                .ExecuteAsync(Plain(), new StoryInput(Source)));
        var simplifiedTask = Assert.ThrowsAsync<IdeaValidationException>(() =>
            CreateAgent(simplifiedGenerator, new FakeChatClient(Accepted)).ExecuteAsync(Plain(), new StoryInput(Source)));
        var errors = await Task.WhenAll(legacyTask, simplifiedTask);
        Assert.Equal([3, 2], errors.Select(e => e.AttemptCount));
        Assert.Equal(3, legacyGenerator.InvocationCount);
        Assert.Equal(2, simplifiedGenerator.InvocationCount);
    }

    [Fact]
    public async Task Workflow_HandsOverOriginalSourceAndAllFiveFieldsToResearch()
    {
        var generation = JsonSerializer.Serialize(new
        {
            ideas = new[]
            {
                new
                {
                    title = "T", description = "D", audience = "A", rationale = "R", evidence = "E"
                },
                new { title = "Alt", description = "d", audience = "a", rationale = "r", evidence = "e" }
            }
        });
        var research = new Stub<ResearchInput, ResearchResult>(new ResearchResult("research"));
        var workflow = new StoryWorkflow(
            CreateAgent(new FakeChatClient(generation), new FakeChatClient(Accepted)), research,
            new Stub<SynthesisInput, SynthesisResult>(new SynthesisResult("s")), new Stub<NarrativeInput, NarrativeResult>(new NarrativeResult("n")),
            new Stub<ComposeInput, ComposeResult>(new ComposeResult("c")));

        await workflow.RunAsync(new StoryInput(Source));

        var input = research.CapturedInput!;
        Assert.Equal(Source, input.Input.Content);
        var idea = input.SelectedIdea.Idea;
        Assert.Equal(new[] { "T", "D", "A", "R", "E" },
            new[] { idea.Title, idea.Description, idea.Audience, idea.Rationale, idea.Evidence });
    }

    [Fact]
    public async Task Workflow_TwoRejections_StopWithoutResearch()
    {
        var research = new Stub<ResearchInput, ResearchResult>(new ResearchResult("research"));
        var workflow = new StoryWorkflow(
            CreateAgent(new FakeChatClient(Generation(), Generation(), Generation()),
                new FakeChatClient(Rejected(), Rejected(), Accepted)), research,
            new Stub<SynthesisInput, SynthesisResult>(new SynthesisResult("s")), new Stub<NarrativeInput, NarrativeResult>(new NarrativeResult("n")),
            new Stub<ComposeInput, ComposeResult>(new ComposeResult("c")));

        var error = await Record.ExceptionAsync(() => workflow.RunAsync(new StoryInput(Source)));

        Assert.NotNull(error);
        Assert.Contains("after 2 attempts", error.ToString());
        Assert.Null(research.CapturedInput);
    }

    [Theory]
    [InlineData(IdeaWorkflowMode.Legacy, "Idea/Idea.system.prompt.txt", "Idea/Idea.user.prompt.txt",
        "Idea/IdeaValidation.system.prompt.txt")]
    [InlineData(IdeaWorkflowMode.Simplified, "Idea/Simplified/Idea.system.prompt.txt",
        "Idea/Simplified/Idea.user.prompt.txt", "Idea/Simplified/IdeaValidation.system.prompt.txt")]
    public void Prompts_AreSelectedPerModeForGeneratorSystemUserAndJudge(IdeaWorkflowMode mode, string system,
        string user, string judge)
    {
        Assert.Equal(PromptLoader.Load(system), IdeaAgent.GetSystemInstructions(mode));
        Assert.Equal(PromptLoader.Load(judge), IdeaSemanticJudge.GetSystemInstructions(mode));
        Assert.Equal(PromptLoader.Render(user, ("SourceContent", "S")), IdeaPrompts.GeneratorUser(mode, "S"));
        Assert.Equal(IdeaAgent.GetSystemInstructions(), PromptLoader.Load("Idea/Idea.system.prompt.txt"));
        Assert.Equal(IdeaSemanticJudge.GetSystemInstructions(),
            PromptLoader.Load("Idea/IdeaValidation.system.prompt.txt"));
    }

    [Fact]
    public void PromptDirectory_OverridesSimplifiedCompletelyWithoutFallback()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-simplified-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(directory, "Idea", "Simplified"));
        try
        {
            File.WriteAllText(Path.Combine(directory, "Idea", "Simplified", "Idea.system.prompt.txt"), "disk system");
            using (PromptDirectory.Use(directory))
            {
                Assert.Equal("disk system", IdeaAgent.GetSystemInstructions(IdeaWorkflowMode.Simplified));
                Assert.Throws<FileNotFoundException>(() => IdeaPrompts.GeneratorUser(IdeaWorkflowMode.Simplified, "S"));
                Assert.Throws<FileNotFoundException>(() =>
                    IdeaSemanticJudge.GetSystemInstructions(IdeaWorkflowMode.Simplified));
                Assert.Throws<FileNotFoundException>(() => IdeaAgent.GetSystemInstructions());
            }
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public void SimplifiedPrompts_KeepTheRequiredBoundaries()
    {
        var judge = IdeaSemanticJudge.GetSystemInstructions(IdeaWorkflowMode.Simplified);
        foreach (var token in new[]
                 {
                     "sourceContradiction", "unsupportedClaim", "editorialPurpose", "title", "description", "audience",
                     "rationale", "evidence"
                 })
        {
            Assert.Contains(token, judge);
        }

        var user = PromptLoader.Load("Idea/Simplified/Idea.user.prompt.txt");
        Assert.Contains("{{SourceContent}}", user);
        Assert.Contains("strongest first", user);
    }

    private sealed class Stub<TIn, TOut>(TOut result) : IAgent<TIn, TOut>
    {
        public TIn? CapturedInput { get; private set; }

        public Task<TOut> ExecuteAsync(AgentExecutionContext executionContext, TIn input,
            CancellationToken cancellationToken = default)
        {
            CapturedInput = input;
            return Task.FromResult(result);
        }
    }

    private sealed class SourceEchoClient(Barrier? barrier) : IChatClient
    {
        private readonly List<(string Source, string Description)> _judged = [];
        private readonly Dictionary<string, int> _calls = [];

        public IReadOnlyList<(string Source, string Description)> Judged
        {
            get
            {
                lock (_judged)
                {
                    return [.. _judged];
                }
            }
        }

        public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var text = string.Join("\n", messages.Select(m => m.Text));
            if (barrier is null)
            {
                // The judge sees exactly {source, idea}; record each payload separately.
                using var payload = JsonDocument.Parse(text);
                lock (_judged)
                {
                    _judged.Add((payload.RootElement.GetProperty("source").GetString()!,
                        payload.RootElement.GetProperty("idea").GetProperty("description").GetString()!));
                }

                return Reply(Accepted);
            }

            var source = text.Contains("source-A", StringComparison.Ordinal) ? "source-A" : "source-B";
            int call;
            lock (_calls)
            {
                call = _calls[source] = _calls.GetValueOrDefault(source) + 1;
            }

            if (call == 1)
            {
                await Task.Run(() => barrier.SignalAndWait(TimeSpan.FromSeconds(10)), cancellationToken);
            }

            return Reply(JsonSerializer.Serialize(new
            {
                ideas = new[]
                {
                    new
                    {
                        title = call == 1 ? "" : "Valid", description = source, audience = "a", rationale = "r"
                    }
                }
            }));
        }

        private static ChatResponse Reply(string text) => new([new ChatMessage(ChatRole.Assistant, text)]);

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }
}
