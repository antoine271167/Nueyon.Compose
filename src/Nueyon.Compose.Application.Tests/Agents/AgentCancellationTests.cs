using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests for verifying cancellation handling consistency across agents.
///     Ensures that:
///     - OperationCanceledException from the supplied cancellation token propagates unchanged
///     - Cancellation is not logged as an error
///     - Unexpected exceptions are still logged and rethrown
/// </summary>
public sealed class AgentCancellationTests
{
    [Fact]
    public async Task IdeaAgent_WhenCancellationTokenCancelled_PropagatesCancellationWithoutLogging()
    {
        // Arrange
        var logCapture = new LogCapture();
        var chatClient = new CancellationThrowingChatClient();
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var ideaAgent = new IdeaAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ChatInput { Content = "Test input" };

        var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<OperationCanceledException>(() =>
            ideaAgent.ExecuteAsync(executionContext, input, cts.Token));

        // Verify that cancellation was not logged as an error
        Assert.False(logCapture.HasErrorLogs, "Cancellation should not be logged as error");
        Assert.IsType<OperationCanceledException>(ex);
    }

    [Fact]
    public async Task ResearchAgent_WhenCancellationTokenCancelled_PropagatesCancellationWithoutLogging()
    {
        // Arrange
        var logCapture = new LogCapture();
        var chatClient = new CancellationThrowingChatClient();
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var researchAgent = new ResearchAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = CreateTestResearchInput();

        var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<OperationCanceledException>(() =>
            researchAgent.ExecuteAsync(executionContext, input, cts.Token));

        // Verify that cancellation was not logged as an error
        Assert.False(logCapture.HasErrorLogs, "Cancellation should not be logged as error");
        Assert.IsType<OperationCanceledException>(ex);
    }

    [Fact]
    public async Task IdeaAgent_WhenUnexpectedExceptionThrown_LogsErrorAndRethrows()
    {
        // Arrange
        var logCapture = new LogCapture();
        var chatClient = new ExceptionThrowingChatClient(new InvalidOperationException("Unexpected error"));
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var ideaAgent = new IdeaAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            ideaAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        // Verify that the unexpected exception was logged as an error
        Assert.True(logCapture.HasErrorLogs, "Unexpected exception should be logged as error");
        Assert.Equal("Unexpected error", ex.Message);
    }

    [Fact]
    public async Task ResearchAgent_WhenUnexpectedExceptionThrown_LogsErrorAndRethrows()
    {
        // Arrange
        var logCapture = new LogCapture();
        var chatClient = new ExceptionThrowingChatClient(new InvalidOperationException("Unexpected error"));
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var researchAgent = new ResearchAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = CreateTestResearchInput();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            researchAgent.ExecuteAsync(executionContext, input, CancellationToken.None));

        // Verify that the unexpected exception was logged as an error
        Assert.True(logCapture.HasErrorLogs, "Unexpected exception should be logged as error");
        Assert.Equal("Unexpected error", ex.Message);
    }

    [Fact]
    public async Task IdeaAgent_WhenCompletedSuccessfully_LogsCompletionWithoutErrors()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson =
            """
            {
              "ideas": [
                {
                  "title": "Test Idea",
                  "description": "Test Description",
                  "audience": "Test Audience",
                  "rationale": "Test Rationale"
                }
              ]
            }
            """;
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var ideaAgent = new IdeaAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await ideaAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.False(logCapture.HasErrorLogs, "Successful execution should not produce error logs");
        Assert.True(logCapture.HasInfoLogs, "Successful execution should produce info logs");
    }

    [Fact]
    public async Task ResearchAgent_WhenCompletedSuccessfully_LogsCompletionWithoutErrors()
    {
        // Arrange
        var logCapture = new LogCapture();
        const string responseJson =
            """
            {
              "content": "Test research content"
            }
            """;
        var chatClient = new FakeChatClient(responseJson);
        var aiAgent = chatClient.AsAIAgent("Test", "TestAgent");
        var researchAgent = new ResearchAgent(aiAgent, logCapture);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());
        var input = CreateTestResearchInput();

        // Act
        var result = await researchAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(logCapture.HasErrorLogs, "Successful execution should not produce error logs");
        Assert.True(logCapture.HasInfoLogs, "Successful execution should produce info logs");
    }

    private static ResearchInput CreateTestResearchInput()
    {
        var idea = new Idea
        {
            Title = "Test Idea",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        return new ResearchInput
        {
            Input = new ChatInput { Content = "Test input" },
            SelectedIdea = new SelectedIdea(idea)
        };
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

    private sealed class CancellationThrowingChatClient : IChatClient
    {
        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default) =>
            throw new OperationCanceledException();

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

    private sealed class ExceptionThrowingChatClient(Exception exceptionToThrow) : IChatClient
    {
        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default) =>
            throw exceptionToThrow;

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

    private sealed class LogCapture : ILogger<IdeaAgent>, ILogger<ResearchAgent>
    {
        public bool HasErrorLogs { get; private set; }
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
            if (logLevel == LogLevel.Error)
            {
                HasErrorLogs = true;
            }

            if (logLevel == LogLevel.Information)
            {
                HasInfoLogs = true;
            }
        }
    }
}