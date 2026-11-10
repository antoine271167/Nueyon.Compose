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

    private const string AcceptedVerdict = """{"blockingIssues":[]}""";

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
                Rationale = "Good rationale"
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
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task IdeaAgent_ReturnsOnlyTheAcceptedGeneration(int acceptedAttempt)
    {
        var generations = Enumerable.Range(1, 3)
            .Select(attempt => ValidIdeasJson.Replace("Valid Idea", $"Idea {attempt}"))
            .ToArray();
        var generator = new FakeChatClient(generations);
        var judge = new FakeChatClient(Enumerable.Repeat(RejectedVerdict(3), acceptedAttempt - 1)
            .Append(AcceptedVerdict).ToArray());
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var ideas = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));

        Assert.Equal($"Idea {acceptedAttempt}", Assert.Single(ideas).Title);
        Assert.Equal(acceptedAttempt, generator.InvocationCount);
        Assert.Equal(acceptedAttempt, judge.InvocationCount);
        foreach (var (request, index) in judge.Requests.Select((messages, index) => (messages, index)))
        {
            using var payload = JsonDocument.Parse(Assert.Single(request, m => m.Role == ChatRole.User).Text);
            Assert.Equal(OriginalSource, payload.RootElement.GetProperty("source").GetString());
            Assert.False(payload.RootElement.TryGetProperty("ideas", out _));
            Assert.Equal($"Idea {index + 1}", payload.RootElement.GetProperty("idea").GetProperty("title").GetString());
        }

        if (acceptedAttempt > 1)
        {
            var feedback = Assert.Single(generator.Requests[1], m =>
                m.Role == ChatRole.User && m.Text.Contains("Address these blocking issues", StringComparison.Ordinal));
            Assert.Contains("Overclaim 1", feedback.Text);
            Assert.Contains("Overclaim 2", feedback.Text);
            Assert.Contains("Overclaim 3", feedback.Text);
            Assert.Contains("Qualify 1", feedback.Text);
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

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource)));

        Assert.Contains("after 3 attempts", error.Message);
        Assert.Contains("Address these blocking issues", error.Message);
        Assert.Contains("Overclaim 1", error.Message);
        Assert.Equal(3, generator.InvocationCount);
        Assert.Equal(3, judge.InvocationCount);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("""{"blockingIssues":[null]}""")]
    [InlineData("""{"blockingIssues":[{"problem":"Overclaim","correction":" "}]}""")]
    [InlineData(
        """{"blockingIssues":[{"problem":"1","correction":"1"},{"problem":"2","correction":"2"},{"problem":"3","correction":"3"},{"problem":"4","correction":"4"}]}""")]
    public async Task IdeaAgent_InvalidJudgeVerdict_FailsClosed(string verdict)
    {
        var generator = new FakeChatClient(ValidIdeasJson, ValidIdeasJson);
        var judge = new FakeChatClient(verdict);
        var agent = CreateSemanticallyValidatedAgent(generator, judge);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource)));

        Assert.Contains("invalid verdict", error.Message);
        Assert.Equal(1, generator.InvocationCount);
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
            new FakeChatClient(InvalidIdeasJson, ValidIdeasJson), new FakeChatClient(AcceptedVerdict), events.Add);
        await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource));
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
            new FakeChatClient(AcceptedVerdict), events.Add);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            exhausted.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource)));
        Assert.Equal(3, events.Last().Attempt);
        Assert.Equal("rejected", events.Last().Status);

        events.Clear();
        var failedJudge = CreateSemanticallyValidatedAgent(
            new FakeChatClient(ValidIdeasJson), new FakeChatClient("{}"), events.Add);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            failedJudge.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), new StoryInput(OriginalSource)));
        Assert.Equal("incomplete", events.Last().Status);
        Assert.Equal(ValidIdeasJson, events.Last().Response);
    }

    private static IdeaAgent CreateSemanticallyValidatedAgent(FakeChatClient generator, FakeChatClient judge,
        Action<IdeaValidationAttempt>? observer = null)
    {
        var semanticJudge = new IdeaSemanticJudge(judge.AsAIAgent("Check source fidelity", "TestIdeaJudge"));
        var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), semanticJudge);
        if (observer is not null)
        {
            evaluator.Observe(observer);
        }

        return new IdeaAgent(evaluator.Wrap(generator.AsAIAgent("Generate ideas", "TestIdeaAgent")),
            NullLogger<IdeaAgent>.Instance);
    }

    private static string RejectedVerdict(int issueCount) => JsonSerializer.Serialize(new
    {
        BlockingIssues = Enumerable.Range(1, issueCount).Select(number => new
        {
            Problem = $"Overclaim {number}",
            Correction = $"Qualify {number}"
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
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

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
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

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
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

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
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

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