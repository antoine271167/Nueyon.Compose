using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests verifying that SynthesizerAgent, when given research content that has already
///     been filtered through <see cref="SynthesisResearchFilter" />, only receives Facts and
///     Unknowns and not Interpretations, Development Sequence, or Editorial Relevance.
/// </summary>
public sealed class SynthesizerAgentBoundaryTests
{
    [Fact]
    public async Task ExecuteAsync_WithFilteredResearch_PromptContainsOnlyFactsAndUnknowns()
    {
        // Arrange
        const string fullResearch =
            """
            ### Facts

            The author renamed the product from StoryFlow to Compose.

            ### Interpretations

            The rename reflects a shift in product positioning.

            ### Unknowns

            The exact date of the rename is not established.

            ### Development sequence

            The rename occurred after the initial launch.

            ### Editorial relevance

            Facts 1 is relevant to the Selected Idea.
            """;

        var filteredResearch = SynthesisResearchFilter.ExtractFactsAndUnknowns(fullResearch);

        var logCapture = new LogCapture();
        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "SynthesizerAgent");
        var synthesizerAgent = new SynthesizerAgent(aiAgent, logCapture);

        var selectedIdea = new Idea(
            "Compose Rename",
            "The product was renamed from StoryFlow to Compose",
            "Editorial Team",
            "To illustrate naming decisions",
            "Documentation shows the name change."
        );
        var input = new SynthesisInput(selectedIdea, new ResearchForSynthesis(filteredResearch));
        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await synthesizerAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        var messageText = userMessage.Text;

        Assert.Contains(
            "The author renamed the product from StoryFlow to Compose.",
            messageText,
            StringComparison.Ordinal);
        Assert.Contains(
            "The exact date of the rename is not established.",
            messageText,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "The rename reflects a shift in product positioning.",
            messageText,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "The rename occurred after the initial launch.",
            messageText,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "Facts 1 is relevant to the Selected Idea.",
            messageText,
            StringComparison.Ordinal);

        Assert.Contains("### Facts", messageText, StringComparison.Ordinal);
        Assert.Contains("### Unknowns", messageText, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_IncludesSelectedIdeaInUserMessage()
    {
        // Arrange
        const string distinctIdeaTitle = "Distinctive Idea Title";
        const string distinctDescription = "Distinctive Description Content";
        const string distinctAudience = "Marketing Leaders";
        const string distinctRationale = "Distinctive Rationale Content";
        const string distinctEvidence = "Source provides evidence.";
        const string researchContent = "### Facts\nSome research fact.";

        var logCapture = new LogCapture();
        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "SynthesizerAgent");
        var synthesizerAgent = new SynthesizerAgent(aiAgent, logCapture);

        var selectedIdea = new Idea(distinctIdeaTitle, distinctDescription, distinctAudience, distinctRationale, distinctEvidence);
        var input = new SynthesisInput(selectedIdea, new ResearchForSynthesis(researchContent));
        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await synthesizerAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        var messageText = userMessage.Text;

        // Verify the Selected Idea section exists and is separate from Research
        Assert.Contains("## SELECTED IDEA", messageText, StringComparison.Ordinal);
        Assert.Contains("## RESEARCH MATERIAL", messageText, StringComparison.Ordinal);

        // Verify the Selected Idea appears before Research Material
        var selectedIdeaIndex = messageText.IndexOf("## SELECTED IDEA", StringComparison.Ordinal);
        var researchMaterialIndex = messageText.IndexOf("## RESEARCH MATERIAL", StringComparison.Ordinal);
        Assert.True(selectedIdeaIndex < researchMaterialIndex, "Selected Idea should appear before Research Material");

        // Verify all Selected Idea fields are present with their actual values
        Assert.Contains(distinctIdeaTitle, messageText, StringComparison.Ordinal);
        Assert.Contains(distinctDescription, messageText, StringComparison.Ordinal);
        Assert.Contains(distinctAudience, messageText, StringComparison.Ordinal);
        Assert.Contains(distinctRationale, messageText, StringComparison.Ordinal);
        Assert.Contains(distinctEvidence, messageText, StringComparison.Ordinal);

        // Verify field labels are present
        Assert.Contains("**Title:**", messageText, StringComparison.Ordinal);
        Assert.Contains("**Description:**", messageText, StringComparison.Ordinal);
        Assert.Contains("**Audience:**", messageText, StringComparison.Ordinal);
        Assert.Contains("**Rationale:**", messageText, StringComparison.Ordinal);
        Assert.Contains("**Evidence:**", messageText, StringComparison.Ordinal);

        // Verify Research content is still present
        Assert.Contains(researchContent, messageText, StringComparison.Ordinal);
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
                  "content": "### Central insight\nTest insight."
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

    private sealed class LogCapture : ILogger<SynthesizerAgent>
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
            if (logLevel == LogLevel.Error)
            {
            }
        }
    }
}