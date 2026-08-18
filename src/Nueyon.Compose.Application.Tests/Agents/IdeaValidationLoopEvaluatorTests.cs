using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Xunit;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
/// Behavioral tests for the LoopAgent + IdeaValidationLoopEvaluator composition.
/// 
/// These tests exercise the REAL Microsoft Agent Framework pipeline:
/// - REAL LoopAgent
/// - REAL AIAgent
/// - FAKE IChatClient (deterministic test double)
/// - REAL IdeaValidationLoopEvaluator
/// - REAL IdeaValidator
/// 
/// This architecture ensures we test our integration with the framework
/// without contacting OpenAI, while still exercising the real framework components.
/// </summary>
public sealed class IdeaValidationLoopEvaluatorBehavioralTests
{
    private readonly IIdeaValidator _validator;
    private readonly IdeaValidationLoopEvaluator _evaluator;

    public IdeaValidationLoopEvaluatorBehavioralTests()
    {
        _validator = new IdeaValidator();
        _evaluator = new IdeaValidationLoopEvaluator(_validator);
    }

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

    /// <summary>
    /// Test: Valid Idea on first attempt
    /// Expected: 1 invocation, operation succeeds
    /// 
    /// This proves that when the agent produces valid ideas on the first try,
    /// the loop stops immediately without retrying.
    /// </summary>
    [Fact]
    public async Task LoopAgent_ValidIdeaOnFirstAttempt_StopsAfterOneInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 10 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(1, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    /// Test: Invalid then Valid
    /// Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_InvalidThenValid_InvokesAgentTwice()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 10 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    /// Test: Invalid, Invalid, then Valid
    /// Expected: 3 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_TwoInvalidThenValid_InvokesAgentThreeTimes()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        // Use a high MaxIterations so that the loop won't cut off before the third call
        var loopOptions = new LoopAgentOptions { MaxIterations = 10 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(3, chatClient.InvocationCount);
        Assert.NotNull(result);
    }

    /// <summary>
    /// Test: Three invalid attempts result in failure
    /// Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidAttempts_ThrowsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 4 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    /// Test: Verify loop respects MaxIterations limit
    /// Expected: Throws InvalidOperationException after exactly 3 iterations, no fourth attempt
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidAttempts_StopsBeforeFourthInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson, InvalidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 4 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    /// Test: Invalid JSON then Valid
    /// Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_InvalidJsonThenValid_RetriesAndSucceeds()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidJson, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 10 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    /// Test: Empty response then Valid
    /// Expected: 2 invocations, operation succeeds
    /// </summary>
    [Fact]
    public async Task LoopAgent_EmptyResponseThenValid_RetriesAndSucceeds()
    {
        // Arrange
        var chatClient = new FakeChatClient(EmptyResponse, ValidIdeasJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 10 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await loopAgent.RunAsync(input.Content, null, null, CancellationToken.None);

        // Assert
        Assert.Equal(2, chatClient.InvocationCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Text);
    }

    /// <summary>
    /// Test: Three invalid JSON responses result in failure
    /// Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeInvalidJsonResponses_FailsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(InvalidJson, InvalidJson, InvalidJson, InvalidJson);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 4 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

        Assert.Equal(3, chatClient.InvocationCount);
    }

    /// <summary>
    /// Test: Three empty responses result in failure
    /// Expected: Throws InvalidOperationException with specific message after exactly 3 attempts
    /// </summary>
    [Fact]
    public async Task LoopAgent_ThreeEmptyResponses_FailsAfterThirdInvocation()
    {
        // Arrange
        var chatClient = new FakeChatClient(EmptyResponse, EmptyResponse, EmptyResponse, EmptyResponse);
        var agent = chatClient.AsAIAgent("Test instructions", "TestIdeaAgent");

        var loopOptions = new LoopAgentOptions { MaxIterations = 4 };
        var loopAgent = new LoopAgent(agent, _evaluator, loopOptions);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => loopAgent.RunAsync(input.Content, null, null, CancellationToken.None));

        Assert.Contains(
            "Idea agent failed to produce valid ideas after 3 attempts.",
            exception.Message);

        Assert.Equal(3, chatClient.InvocationCount);
    }
}

/// <summary>
/// A deterministic fake IChatClient for testing.
/// Returns predefined responses in sequence and tracks invocation count.
/// 
/// This is the test seam - it replaces OpenAI's chat client with deterministic responses
/// while keeping the real AIAgent, LoopAgent, and evaluator intact.
/// </summary>
internal sealed class FakeChatClient : IChatClient
{
    private readonly Queue<string> _responses;
    private int _invocationCount;

    public int InvocationCount => _invocationCount;

    /// <summary>
    /// Creates a new fake chat client with predefined responses.
    /// </summary>
    /// <param name="responses">The sequence of model responses to return.</param>
    public FakeChatClient(params string[] responses)
    {
        ArgumentNullException.ThrowIfNull(responses);
        _responses = new Queue<string>(responses);
        _invocationCount = 0;
    }

    /// <summary>
    /// Returns the next predefined response as a chat response.
    /// </summary>
    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _invocationCount++;

        if (!_responses.TryDequeue(out var responseText))
        {
            throw new InvalidOperationException(
                $"FakeChatClient ran out of predefined responses after {_invocationCount - 1} invocations.");
        }

        // Create a ChatResponse with the predefined response text
        var message = new ChatMessage(ChatRole.Assistant, responseText);
        var response = new ChatResponse(new[] { message });

        return await Task.FromResult(response);
    }

    /// <summary>
    /// Streaming is not used in these tests - throws NotImplementedException if called.
    /// </summary>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Streaming is not used in LoopAgent tests.");

        // Unreachable, but required for IAsyncEnumerable compilation
#pragma warning disable CS0162
        await Task.CompletedTask;
        yield break;
#pragma warning restore CS0162
    }

    /// <summary>
    /// Gets client information for debugging/logging.
    /// </summary>
    public ChatClientMetadata? Metadata => null;

    /// <summary>
    /// Gets a service from this client (not used in tests).
    /// </summary>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return null;
    }

    /// <summary>
    /// Disposes resources (test client has none).
    /// </summary>
    public void Dispose()
    {
    }
}

#pragma warning restore MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.
