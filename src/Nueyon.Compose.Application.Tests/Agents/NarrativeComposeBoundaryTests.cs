using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents.Compose;
using Nueyon.Compose.Application.Agents.Narrative;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests verifying that NarrativeAgent and ComposeAgent receive only the semantic material
///     produced by the preceding pipeline stage, without the workflow adding additional
///     semantic material of its own.
/// </summary>
public sealed class NarrativeComposeBoundaryTests
{
    [Fact]
    public async Task NarrativeAgent_ExecuteAsync_ReceivesSynthesisContentInUserMessage()
    {
        // Arrange
        const string distinctiveSynthesisText =
            "Distinctive synthesis marker: the rename occurred in a single unrecorded commit.";

        var synthesisResult = new SynthesisResult(distinctiveSynthesisText);

        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "NarrativeAgent");
        var narrativeAgent = new NarrativeAgent(aiAgent, new LogCapture<NarrativeAgent>());

        var input = new NarrativeInput(synthesisResult);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await narrativeAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        Assert.Contains(distinctiveSynthesisText, userMessage.Text, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ComposeAgent_ExecuteAsync_ReceivesNarrativeContentInUserMessage()
    {
        // Arrange
        const string distinctiveNarrativeText =
            "Distinctive narrative marker: the migration finished before the audit began.";

        var narrativeResult = new NarrativeResult(distinctiveNarrativeText);

        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "ComposeAgent");
        var composeAgent = new ComposeAgent(aiAgent, new LogCapture<ComposeAgent>());

        var input = new ComposeInput(narrativeResult, ContentFormat.Article);
        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await composeAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        Assert.Contains(distinctiveNarrativeText, userMessage.Text, StringComparison.Ordinal);
    }

    private sealed class MessageCapturingChatClient : IChatClient
    {
        public List<ChatMessage> CapturedMessages { get; } = new();

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            CapturedMessages.Clear();
            CapturedMessages.AddRange(messages);

            const string responseJson =
                """
                {
                  "content": "Test generated content."
                }
                """;

            var message = new ChatMessage(ChatRole.Assistant, responseJson);
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

    private sealed class LogCapture<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }
    }
}