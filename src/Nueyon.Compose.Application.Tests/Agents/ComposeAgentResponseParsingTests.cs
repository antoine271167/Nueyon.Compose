using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents.Compose;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests for ComposeAgent response parsing and error handling.
///     Verifies that:
///     - Invalid JSON raises clear exceptions
///     - Empty content responses are rejected
///     - Null responses are rejected
/// </summary>
public sealed class ComposeAgentResponseParsingTests
{
    /// <summary>
    ///     Test: Invalid JSON response
    ///     Expected: throws InvalidOperationException with clear message
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithInvalidJsonResponse_ThrowsInvalidOperationException()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string invalidJson = "{ invalid json }";
        var chatClient = new FakeChatClient(invalidJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        Assert.Contains("Failed to parse agent response as JSON", ex.Message);
    }

    /// <summary>
    ///     Test: Empty content field in response
    ///     Expected: throws InvalidOperationException with clear message
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithEmptyContentField_ThrowsInvalidOperationException()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson =
            """
            {
              "content": ""
            }
            """;
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        Assert.Contains("content is empty", ex.Message);
    }

    /// <summary>
    ///     Test: Whitespace-only content field in response
    ///     Expected: throws InvalidOperationException with clear message
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithWhitespaceOnlyContent_ThrowsInvalidOperationException()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson =
            """
            {
              "content": "   "
            }
            """;
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        Assert.Contains("content is empty", ex.Message);
    }

    /// <summary>
    ///     Test: Null response from deserialization
    ///     Expected: throws InvalidOperationException
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithNullDeserializedResponse_ThrowsInvalidOperationException()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson = "null";
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        Assert.Contains("Deserialization resulted in null", ex.Message);
    }

    /// <summary>
    ///     Test: Missing content field in response
    ///     Expected: throws InvalidOperationException (deserializes to null content)
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithMissingContentField_ThrowsInvalidOperationException()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson = "{}";
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        Assert.Contains("content is empty", ex.Message);
    }

    /// <summary>
    ///     Test: Valid response with content
    ///     Expected: returns ComposeResult with content
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithValidResponse_ReturnsComposeResult()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson =
            """
            {
              "content": "This is a complete article with title, introduction, body, and conclusion."
            }
            """;
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var composeAgent = new ComposeAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ComposeInput(
            new NarrativeResult("Test narrative"),
            ContentFormat.Article);

        // Act
        var result = await composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("This is a complete article with title, introduction, body, and conclusion.", result.Content);
    }

    private sealed class FakeChatClient(params string[] responses) : IChatClient
    {
        private readonly Queue<string> _responses = new(responses);

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (!_responses.TryDequeue(out var responseText))
            {
                throw new InvalidOperationException("FakeChatClient ran out of responses.");
            }

            var message = new ChatMessage(ChatRole.Assistant, responseText);
            var response = new ChatResponse([message]);
            return Task.FromResult(response);
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }

    private sealed class LogCapture : ILogger<ComposeAgent>
    {
        public bool HasInfoLogs { get; private set; }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Information)
            {
                HasInfoLogs = true;
            }
        }
    }
}