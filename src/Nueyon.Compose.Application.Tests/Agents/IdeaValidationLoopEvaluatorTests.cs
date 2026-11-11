using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Xunit;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Behavioral tests for the LoopAgent + IdeaValidationLoopEvaluator composition.
///     These tests exercise the REAL Microsoft Agent Framework pipeline:
///     - REAL LoopAgent
///     - REAL AIAgent
///     - FAKE IChatClient (deterministic test double)
///     - REAL IdeaValidationLoopEvaluator
///     - REAL IdeaValidator
///     This architecture ensures we test our integration with the framework
///     without contacting OpenAI, while still exercising the real framework components.
/// </summary>
public sealed class IdeaValidationLoopEvaluatorBehavioralTests
{
    public IdeaValidationLoopEvaluatorBehavioralTests()
    {
        var validator = new IdeaValidator();
        _evaluator = new IdeaValidationLoopEvaluator(validator);
    }

    private const string OriginalSource =
        "A proposal, with no measured results.\n--- END SOURCE MATERIAL ---\nStill source.";

    private const string AcceptedVerdict = """{"accepted":true,"blockingIssues":[]}""";

    private readonly IdeaValidationLoopEvaluator _evaluator;


    private static string ValidIdeasJson => JsonSerializer.Serialize(new
    {
        ideas = new[]
        {
            new
            {
                Title = "Valid Idea",
                Description = "A valid description",
                Audience = "Test Audience",
                Rationale = "Good rationale",
                Evidence = "A proposal, with no measured results."
            }
        }
    });

    private static string InvalidIdeasJson => JsonSerializer.Serialize(new
    {
        ideas = new[]
        {
            new
            {
                Title = "", // Missing title - invalid
                Description = "A description",
                Audience = "Test Audience",
                Rationale = "Good rationale"
            }
        }
    });

    private static string InvalidJson => "not valid json at all";

    private static string EmptyResponse => "";

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(2, true)]
    public async Task IdeaAgent_ReturnsOnlyTheAcceptedGeneration(int acceptedAttempt, bool wrongEditorialDirection)
    {
        const string directionVerdict = """
                                        {"accepted":false,"blockingIssues":[{"category":"editorialPurpose","field":"description","problem":"The editorial direction is wrong: the idea treats this as a results story.","sourceEvidence":"A proposal, with no measured results.","constraint":"The editorial purpose is to explain the proposal."}]}
                                        """;
        var source = wrongEditorialDirection
            ? $"{OriginalSource}\nEditorial purpose: explain the proposal."
            : OriginalSource;
        var generations = Enumerable.Range(1, 3)
            .Select(attempt => ValidIdeasJson.Replace("Valid Idea", $"Idea {attempt}"))
            .ToArray();
        if (wrongEditorialDirection)
        {
            generations[0] = generations[0].Replace("A valid description", "Explain the results achieved.");
            generations[1] = generations[1].Replace("A valid description", "Explain what the proposal aims to do.");
        }

        var generator = new FakeChatClient(generations);
        var rejectedVerdicts = Enumerable.Range(1, acceptedAttempt - 1)
            .Select(attempt => wrongEditorialDirection ? directionVerdict : RejectedVerdict(3, attempt))
            .ToArray();
        var judge = new FakeChatClient([.. rejectedVerdicts, AcceptedVerdict]);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);
        var attempts = new List<IdeaValidationAttempt>();

        var ideas = await agent.ExecuteAsync(Observed(attempts.Add), new StoryInput(source));

        Assert.Equal($"Idea {acceptedAttempt}", Assert.Single(ideas).Title);
        Assert.Equal(acceptedAttempt, generator.InvocationCount);
        Assert.Equal(acceptedAttempt, judge.InvocationCount);
        foreach (var (request, index) in judge.Requests.Select((messages, index) => (messages, index)))
        {
            using var payload = JsonDocument.Parse(Assert.Single(request, m => m.Role == ChatRole.User).Text);
            Assert.Equal(source, payload.RootElement.GetProperty("source").GetString());
            Assert.Equal(["source", "idea"], payload.RootElement.EnumerateObject().Select(p => p.Name));
            Assert.DoesNotContain(request, m => m.Role == ChatRole.Assistant);
            Assert.Equal($"Idea {index + 1}", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());
        }

        var initialTask = Assert.Single(generator.Requests[0], m => m.Role == ChatRole.User).Text;
        Assert.Contains(source, initialTask);
        Assert.Contains("# TASK", initialTask);
        for (var index = 1; index < acceptedAttempt; index++)
        {
            var request = generator.Requests[index];
            Assert.DoesNotContain(request, m => m.Role == ChatRole.Assistant);
            var userMessages = request.Where(m => m.Role == ChatRole.User).Select(m => m.Text).ToArray();
            Assert.Equal(2, userMessages.Length);
            Assert.Equal(initialTask, userMessages[0]);

            var retry = userMessages[1];
            Assert.Contains("Preserve the original editorial purpose and use the original source as your evidence.", retry);
            Assert.Contains("revise it in first place", retry);
            Assert.Contains("Preserve all valid aspects and change only what is needed to fix the diagnosed problems.", retry);
            Assert.Contains("Do not add new claims or a research agenda.", retry);
            Assert.Contains("Change the editorial angle only if the diagnosis explicitly identifies the previous editorial direction as wrong.", retry);
            Assert.Contains("make the smallest source-supported change that restores the original editorial purpose.", retry);

            var lines = retry.Split('\n');
            Assert.StartsWith("Previous first idea: ", lines[^2]);
            using var previous = JsonDocument.Parse(lines[^2]["Previous first idea: ".Length..]);
            using var previousJudgeInput = JsonDocument.Parse(
                Assert.Single(judge.Requests[index - 1], m => m.Role == ChatRole.User).Text);
            Assert.Equal(previousJudgeInput.RootElement.GetProperty("idea").GetRawText(), previous.RootElement.GetRawText());
            Assert.StartsWith("Diagnosis: ", lines[^1]);
            var diagnosis = lines[^1]["Diagnosis: ".Length..];
            Assert.Equal(Assert.Single(attempts, a => a.Attempt == index && a.Status == "rejected").Feedback, diagnosis);
            using var expected = JsonDocument.Parse(rejectedVerdicts[index - 1]);
            Assert.Equal(expected.RootElement.GetProperty("blockingIssues").GetRawText(), diagnosis);
        }
    }

    [Theory]
    [InlineData("Second", """{"title":"Second","description":"d","audience":"a","rationale":"r","evidence":"e"}""")]
    [InlineData("", """{"title":"","description":"d","audience":" ","rationale":"r","evidence":"e"}""")]
    [InlineData(null, """{"title":42,"description":["x"],"audience":null,"rationale":{},"evidence":false}""")]
    public async Task IdeaAgent_JudgesOnlyTheFirstIdea_AndKeepsAlternatives(string? secondTitle, string second)
    {
        var generation = $$"""
                           {"ideas":[{"title":"First","description":"d","audience":"a","rationale":"r","evidence":"e"},{{second}}]}
                           """;
        var generator = new FakeChatClient(generation);
        var judge = new FakeChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var ideas = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Equal(secondTitle is null ? ["First"] : ["First", secondTitle], ideas.Select(i => i.Title));
        Assert.Equal(1, generator.InvocationCount);
        using var payload =
            JsonDocument.Parse(Assert.Single(Assert.Single(judge.Requests), m => m.Role == ChatRole.User).Text);
        Assert.Equal(OriginalSource, payload.RootElement.GetProperty("source").GetString());
        Assert.Equal("First", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());
        Assert.DoesNotContain("Second", payload.RootElement.GetRawText());
    }

    [Theory]
    [InlineData("""{"title":42,"description":"d","audience":"a","rationale":"r"}""")]
    [InlineData("null")]
    public async Task IdeaAgent_InvalidFirstIdeaWithValidSecond_RetriesWithoutFallback(string first)
    {
        var invalidFirst =
            $$"""{"ideas":[{{first}},{"title":"Second","description":"d","audience":"a","rationale":"r"}]}""";
        var generator = new FakeChatClient(invalidFirst, ValidIdeasJson);
        var judge = new FakeChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var ideas = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Equal("Valid Idea", Assert.Single(ideas).Title);
        Assert.Equal(2, generator.InvocationCount);
        var retry = Assert.Single(generator.Requests[1], m => m.Role == ChatRole.User &&
                                                            m.Text.Contains("Previous first idea:")).Text;
        Assert.Contains("If no previous first idea is available, generate the list from the original task and source", retry);
        Assert.Contains("Previous first idea: null", retry);
        using var payload =
            JsonDocument.Parse(Assert.Single(Assert.Single(judge.Requests), m => m.Role == ChatRole.User).Text);
        Assert.Equal("Valid Idea", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());
    }

    [Fact]
    public async Task IdeaAgent_StructurallyEmptyFirstIdeaWithValidSecond_IsRejected()
    {
        const string emptyFirst =
            """{"ideas":[{"title":"","description":"d","audience":"a","rationale":"r"},{"title":"Second","description":"d","audience":"a","rationale":"r"}]}""";
        var generator = new FakeChatClient(emptyFirst, ValidIdeasJson);
        var judge = new FakeChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var ideas = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Equal("Valid Idea", Assert.Single(ideas).Title);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task IdeaAgent_ThreeSemanticRejections_ThrowsBeforeFourthGeneration()
    {
        var generator = new FakeChatClient(ValidIdeasJson, ValidIdeasJson, ValidIdeasJson, ValidIdeasJson);
        var rejected = RejectedVerdict(1);
        var judge = new FakeChatClient(rejected, rejected, rejected, AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);
        var attempts = new List<IdeaValidationAttempt>();

        var error = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            agent.ExecuteAsync(Observed(attempts.Add), new StoryInput(OriginalSource)));

        Assert.Equal("Idea agent failed to produce valid ideas after 3 attempts.", error.Message);
        Assert.Equal(3, error.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Semantic, error.Kind);
        Assert.Contains("Overclaim 1", attempts[^1].Feedback);
        Assert.Equal(3, generator.InvocationCount);
        Assert.Equal(3, judge.InvocationCount);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("""{"blockingIssues":[]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[]}""")]
    [InlineData("""{"accepted":false,"accepted":true,"blockingIssues":[]}""")]
    [InlineData("""{"accepted":true,"blockingIssues":null}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[null]}""")]
    [InlineData("""{"accepted":true,"blockingIssues":[],"suggestedTitle":"Replacement title"}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":"No results","constraint":"Proposal only","correction":"Use this title"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":" ","constraint":"Proposal only"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","constraint":"Proposal only"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":"No results"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":"No results","constraint":" "}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":"No results","sourceEvidence":"Published results","constraint":"Proposal only"}]}""")]
    [InlineData("""{"accepted":false,"blockingIssues":[{"category":"claim","field":"title","problem":"Overclaim","sourceEvidence":"No results","constraint":"Results supported","constraint":"Proposal only"}]}""")]
    public Task IdeaAgent_InvalidJudgeVerdict_FailsClosed(string verdict) => AssertInvalidVerdict(verdict);

    [Theory]
    [InlineData("suggestedTitle", "The added prompt rule caused the score drop")]
    [InlineData("newAngle", "Narrative is the first intervention")]
    [InlineData("researchAgenda", "Evaluate route-card distribution with museum staff")]
    public Task IdeaAgent_RejectsPrescriptiveFieldsFromV3FailurePatterns(string field, string value)
    {
        var verdict = JsonSerializer.SerializeToNode(new
        {
            accepted = false,
            blockingIssues = new[]
            {
                new
                {
                    category = "unsupportedClaim",
                    field = "description",
                    problem = "The claim exceeds the source.",
                    sourceEvidence = OriginalSource,
                    constraint = "Preserve the scope of the evidence."
                }
            }
        })!;
        verdict["blockingIssues"]![0]![field] = value;
        return AssertInvalidVerdict(verdict.ToJsonString());
    }

    private static async Task AssertInvalidVerdict(string verdict)
    {
        var generator = new FakeChatClient(ValidIdeasJson, ValidIdeasJson);
        var judge = new FakeChatClient(verdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(1, true)]
    public Task IdeaAgent_RejectsOverLimitAndContradictoryVerdicts(int issueCount, bool accepted) =>
        AssertInvalidVerdict(RejectedVerdict(issueCount, accepted: accepted));

    [Fact]
    public async Task SemanticJudge_RequestsClosedDiagnosisSchema()
    {
        var judge = new FakeChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(new FakeChatClient(ValidIdeasJson), judge);
        await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        var format = Assert.IsType<ChatResponseFormatJson>(Assert.Single(judge.Options)!.ResponseFormat);
        var schema = Assert.IsType<JsonElement>(format.Schema);
        AssertRequiredFields(schema, ["accepted", "blockingIssues"]);
        var properties = schema.GetProperty("properties");
        Assert.Equal("boolean", properties.GetProperty("accepted").GetProperty("type").GetString());
        var issueSchema = properties.GetProperty("blockingIssues").GetProperty("items");
        AssertRequiredFields(issueSchema, ["category", "field", "problem", "sourceEvidence", "constraint"]);

        static void AssertRequiredFields(JsonElement schema, string[] fields)
        {
            Assert.False(schema.GetProperty("additionalProperties").GetBoolean());
            Assert.Equal(fields, schema.GetProperty("properties").EnumerateObject().Select(p => p.Name));
            Assert.Equal(fields, schema.GetProperty("required").EnumerateArray().Select(p => p.GetString()));
        }
    }

    [Fact]
    public async Task IdeaAgent_ValidatesStructureBeforeCallingJudge()
    {
        var generator = new FakeChatClient(InvalidIdeasJson, ValidIdeasJson);
        var judge = new FakeChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var ideas = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Single(ideas);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task IdeaAgent_CancellationDuringSemanticJudge_PropagatesWithoutRetry()
    {
        using var cts = new CancellationTokenSource();
        var generator = new FakeChatClient(ValidIdeasJson, ValidIdeasJson);
        var judge = new FakeChatClient(AcceptedVerdict)
        {
            OnGetResponse = token =>
            {
                cts.Cancel();
                token.ThrowIfCancellationRequested();
            }
        };
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource), cts.Token));

        Assert.Equal(1, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task Observer_RecordsRejectionAndAcceptanceWithoutChangingRetry()
    {
        var events = new List<IdeaValidationAttempt>();
        var agent = CreateSemanticallyValidatedAgent(
            new FakeChatClient(InvalidIdeasJson, ValidIdeasJson), new FakeChatClient(AcceptedVerdict));
        await agent.ExecuteAsync(Observed(events.Add), new StoryInput(OriginalSource));
        var completed = events.Where(e => e.Status != "incomplete").ToArray();
        Assert.Equal(["rejected", "accepted"], completed.Select(e => e.Status));
        Assert.Equal([0, 1], completed.Select(e => e.Retry));
        Assert.Equal(InvalidIdeasJson, completed[0].Response);
        Assert.NotNull(completed[0].Feedback);
    }

    [Fact]
    public async Task Observer_RecordsExhaustionAndIncompleteJudgeFailure()
    {
        var events = new List<IdeaValidationAttempt>();
        var exhausted = CreateSemanticallyValidatedAgent(
            new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson),
            new FakeChatClient(AcceptedVerdict));
        var failure = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            exhausted.ExecuteAsync(Observed(events.Add),
                new StoryInput(OriginalSource)));
        Assert.Equal(3, failure.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Structural, failure.Kind);
        Assert.Equal(3, events.Last().Attempt);
        Assert.Equal("rejected", events.Last().Status);

        events.Clear();
        var failedJudge = CreateSemanticallyValidatedAgent(
            new FakeChatClient(ValidIdeasJson), new FakeChatClient("{}"));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            failedJudge.ExecuteAsync(Observed(events.Add),
                new StoryInput(OriginalSource)));
        Assert.Equal("incomplete", events.Last().Status);
        Assert.Equal(ValidIdeasJson, events.Last().Response);
    }

    [Fact]
    public async Task Observer_KeepsConcurrentRunsOfTheSameAgentSeparate()
    {
        var generator = new OverlappingChatClient();
        var judge = new StaticChatClient(AcceptedVerdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);
        var first = new List<IdeaValidationAttempt>();
        var second = new List<IdeaValidationAttempt>();

        await Task.WhenAll(
            agent.ExecuteAsync(Observed(first.Add), new StoryInput("source-A")),
            agent.ExecuteAsync(Observed(second.Add), new StoryInput("source-B")));

        Assert.True(generator.Overlapped);
        Assert.All(first, a => Assert.Contains("source-A", a.Response));
        Assert.All(second, a => Assert.Contains("source-B", a.Response));
        Assert.Equal(["rejected", "accepted"], first.Where(a => a.Status != "incomplete").Select(a => a.Status));
        Assert.Equal(["rejected", "accepted"], second.Where(a => a.Status != "incomplete").Select(a => a.Status));
    }

    [Fact]
    public async Task RunWithoutObserver_DoesNotReportToAnotherRunsObserver()
    {
        var events = new List<IdeaValidationAttempt>();
        var agent = CreateSemanticallyValidatedAgent(
            new FakeChatClient(ValidIdeasJson, ValidIdeasJson), new FakeChatClient(AcceptedVerdict, AcceptedVerdict));

        await agent.ExecuteAsync(Observed(events.Add), new StoryInput(OriginalSource));
        var observed = events.Count;
        await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Equal(observed, events.Count);
    }

    private static AgentExecutionContext Observed(Action<IdeaValidationAttempt> observer) =>
        new(Guid.NewGuid())
        {
            AdditionalProperties = new Dictionary<string, object?>
                { [IdeaValidationLoopEvaluator.ObserverKey] = observer }
        };

    internal static IdeaAgent CreateSemanticallyValidatedAgent(IChatClient generator, IChatClient judge)
    {
        var semanticJudge = new IdeaSemanticJudge(judge.AsAIAgent("Check source fidelity", "TestIdeaJudge"));
        var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), semanticJudge);

        return new IdeaAgent(evaluator.Wrap(generator.AsAIAgent("Generate ideas", "TestIdeaAgent")),
            NullLogger<IdeaAgent>.Instance);
    }

    private sealed class StaticChatClient(string response) : IChatClient
    {
        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant, response)]));

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }

    // Holds each run's first generation until both runs are in flight, then rejects it once and accepts the retry.
    private sealed class OverlappingChatClient : IChatClient
    {
        private readonly TaskCompletionSource _bothStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly Dictionary<string, int> _calls = [];
        private int _started;

        public bool Overlapped => _bothStarted.Task.IsCompletedSuccessfully;

        public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var text = string.Join("\n", messages.Select(m => m.Text));
            var source = text.Contains("source-A", StringComparison.Ordinal) ? "source-A" : "source-B";
            int call;
            lock (_calls)
            {
                call = _calls[source] = _calls.GetValueOrDefault(source) + 1;
            }

            if (call == 1)
            {
                if (Interlocked.Increment(ref _started) == 2)
                {
                    _bothStarted.SetResult();
                }

                await _bothStarted.Task.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            }

            var title = call == 1 ? "" : "Valid Idea";
            var json = JsonSerializer.Serialize(new
            {
                ideas = new[]
                {
                    new { Title = title, Description = source, Audience = "Test Audience", Rationale = "Good" }
                }
            });
            return new ChatResponse([new ChatMessage(ChatRole.Assistant, json)]);
        }

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
            ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }

    internal static string RejectedVerdict(int issueCount, int attempt = 1, bool accepted = false) => JsonSerializer.Serialize(new
    {
        accepted,
        blockingIssues = Enumerable.Range(1, issueCount).Select(number => new
        {
            category = "unsupportedClaim",
            field = "title",
            problem = $"Overclaim {number} in attempt {attempt}",
            sourceEvidence = "A proposal, with no measured results.",
            constraint = "The source establishes proposal status only."
        })
    });

    /// <summary>
    ///     Test: Valid Idea on first attempt
    ///     Expected: 1 invocation, operation succeeds
    ///     This proves that when the agent produces valid ideas on the first try,
    ///     the loop stops immediately without retrying.
    /// </summary>
    [Fact]
    public async Task LoopAgent_ValidIdeaOnFirstAttempt_StopsAfterOneInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(1, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    ///     Test: Invalid then Valid
    ///     Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_InvalidThenValid_InvokesAgentTwice()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    ///     Test: Invalid, Invalid, then Valid
    ///     Expected: 3 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_TwoInvalidThenValid_InvokesAgentThreeTimes()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        // Use a high MaxIterations so that the loop won't cut off before the third call
        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(3, chatClient.InvocationCount);
        Assert.NotNull(result);
    }

    /// <summary>
    ///     Test: Three invalid attempts result in failure
    ///     Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidAttempts_ThrowsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);
        Assert.Equal(3, exception.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Structural, exception.Kind);

        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    ///     Test: Verify loop respects MaxIterations limit
    ///     Expected: Throws InvalidOperationException after exactly 3 iterations, no fourth attempt
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidAttempts_StopsBeforeFourthInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act & Assert
        var failure = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Equal(3, failure.AttemptCount);
        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    ///     Test: Invalid JSON then Valid
    ///     Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_InvalidJsonThenValid_RetriesAndSucceeds()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    ///     Test: Empty response then Valid
    ///     Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_EmptyResponseThenValid_RetriesAndSucceeds()
    {
        // Arrange
        var chatClient = new FakeChatClient(EmptyResponse, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    ///     Test: Three invalid JSON responses result in failure
    ///     Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidJsonResponses_FailsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidJson, InvalidJson, InvalidJson, InvalidJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);
        Assert.Equal(3, exception.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Structural, exception.Kind);

        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    ///     Test: Three empty responses result in failure
    ///     Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeEmptyResponses_FailsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(EmptyResponse, EmptyResponse, EmptyResponse, EmptyResponse);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopAgent = _evaluator.Wrap(agent);

        var input = new StoryInput("Test input");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<IdeaValidationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);
        Assert.Equal(3, exception.AttemptCount);
        Assert.Equal(IdeaValidationFailureKind.Structural, exception.Kind);

        Assert.Equal(3, chatClient.InvocationCount);
    }
}

/// <summary>
///     A deterministic fake IChatClient for testing.
///     Returns predefined responses in sequence and tracks invocation count.
///     This is the test seam - it replaces OpenAI's chat client with deterministic responses
///     while keeping the real AIAgent, LoopAgent, and evaluator intact.
/// </summary>
internal sealed class FakeChatClient : IChatClient
{
    /// <summary>
    ///     Creates a new fake chat client with predefined responses.
    /// </summary>
    /// <param name="responses">The sequence of model responses to return.</param>
    public FakeChatClient(params string[] responses)
    {
        ArgumentNullException.ThrowIfNull(responses);
        _responses = new Queue<string>(responses);
        InvocationCount = 0;
    }

    private readonly Queue<string> _responses;

    public int InvocationCount { get; private set; }
    public List<ChatMessage[]> Requests { get; } = [];
    public List<ChatOptions?> Options { get; } = [];

    /// <summary>
    ///     Optional callback invoked with the supplied token after the invocation count is incremented.
    /// </summary>
    public Action<CancellationToken>? OnGetResponse { get; init; }

    /// <summary>
    ///     Returns the next predefined response as a chat response.
    /// </summary>
    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        InvocationCount++;
        OnGetResponse?.Invoke(cancellationToken);
        Requests.Add(messages.ToArray());
        Options.Add(options);

        if (!_responses.TryDequeue(out var responseText))
        {
            throw new InvalidOperationException(
                $"FakeChatClient ran out of predefined responses after {InvocationCount - 1} invocations.");
        }

        // Create a ChatResponse with the predefined response text
        var message = new ChatMessage(ChatRole.Assistant, responseText);
        var response = new ChatResponse([message]);

        return await Task.FromResult(response);
    }

    /// <summary>
    ///     Streaming is not used in these tests - throws NotImplementedException if called.
    /// </summary>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Streaming is not used in LoopAgent tests.");

        // Unreachable, but required for IAsyncEnumerable compilation
#pragma warning disable CS0162
        await Task.CompletedTask;
        yield break;
#pragma warning restore CS0162
    }

    /// <summary>
    ///     Gets a service from this client (not used in tests).
    /// </summary>
    public object? GetService(Type serviceType, object? serviceKey = null) => null;

    /// <summary>
    ///     Disposes resources (test client has none).
    /// </summary>
    public void Dispose()
    {
    }
}

#pragma warning restore MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.
