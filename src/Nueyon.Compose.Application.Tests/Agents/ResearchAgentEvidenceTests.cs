using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests for ResearchAgent Evidence handling.
///     Verifies that:
///     - ResearchAgent receives the Evidence from SelectedIdea
///     - Evidence is included in the prompt sent to the LLM
///     - Evidence is used to anchor the research process
/// </summary>
public sealed class ResearchAgentEvidenceTests
{
    [Fact]
    public async Task Experiment9_RawResponsesReplayExactlyThroughProductionParserAndFilter()
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root is not null && !Directory.Exists(Path.Combine(root.FullName, "PromptExperiments")))
            root = root.Parent;
        Assert.NotNull(root);
        var calls = Path.Combine(root.FullName, "PromptExperiments", "Research",
            "editorial-acceptance-experiment-9-2026-11-16", "calls");
        var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web)
            { WriteIndented = true, PropertyNameCaseInsensitive = true };
        var directories = Directory.GetDirectories(calls);
        Assert.Equal(24, directories.Length);
        foreach (var directory in directories)
        {
            using var raw = System.Text.Json.JsonDocument.Parse(
                await File.ReadAllTextAsync(Path.Combine(directory, "response-raw.json")));
            var text = raw.RootElement.GetProperty("choices")[0].GetProperty("message")
                .GetProperty("content").GetString()!;
            var input = System.Text.Json.JsonSerializer.Deserialize<ResearchInput>(
                await File.ReadAllTextAsync(Path.Combine(directory, "input.json")), options)!;
            var client = new MessageCapturingChatClient { ResponseJson = text };
            var agent = new ResearchAgent(client.AsAIAgent("OfflineReplay"), new LogCapture());
            var result = await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), input);
            Assert.Equal(await File.ReadAllTextAsync(Path.Combine(directory, "result.json")),
                System.Text.Json.JsonSerializer.Serialize(result, options));
            Assert.Equal(await File.ReadAllTextAsync(Path.Combine(directory, "projection.md")),
                Nueyon.Compose.Application.Agents.Synthesis.SynthesisResearchFilter.ExtractFactsAndUnknowns(result.Content));
        }
    }

    /// <summary>
    ///     Test: ResearchAgent includes Evidence from SelectedIdea in the prompt
    ///     Expected: The user message sent to the LLM contains the Evidence value
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithSelectedIdea_IncludesEvidenceInPrompt()
    {
        // Arrange
        var logCapture = new LogCapture();
        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "ResearchAgent");
        var researchAgent = new ResearchAgent(aiAgent, logCapture);

        const string evidenceText = "The author explicitly decided to rename the product from StoryFlow to Compose.";
        const string ideaTitle = "Product Naming Decision";
        const string ideaDescription = "The shift from StoryFlow to Compose reveals a change in product positioning.";
        const string ideaAudience = "Product strategists";
        const string ideaRationale = "Understanding naming decisions shows product evolution.";

        var idea = new Idea(
            ideaTitle,
            ideaDescription,
            ideaAudience,
            ideaRationale,
            evidenceText);

        var selectedIdea = new SelectedIdea(idea);
        var input = new ResearchInput(
            new StoryInput("Source material describing the product journey."),
            selectedIdea);

        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await researchAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        Assert.NotNull(messageCaptureClient.CapturedMessages);
        Assert.NotEmpty(messageCaptureClient.CapturedMessages);

        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        var messageText = userMessage.Text;
        Assert.Contains(evidenceText, messageText, StringComparison.Ordinal);
        Assert.Contains(ideaTitle, messageText, StringComparison.Ordinal);
        Assert.Contains(ideaDescription, messageText, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Test: ResearchAgent receives Evidence and includes it in prompt for verification
    ///     Expected: The prompt contains the EVIDENCE VERIFICATION section
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithEvidence_IncludesEvidenceVerificationSection()
    {
        // Arrange
        var logCapture = new LogCapture();
        var messageCaptureClient = new MessageCapturingChatClient();
        var aiAgent = messageCaptureClient.AsAIAgent("Test", "ResearchAgent");
        var researchAgent = new ResearchAgent(aiAgent, logCapture);

        const string evidenceText = "The source shows that orchestrating multiple agents proved more challenging than individual agent optimization.";

        var idea = new Idea(
            "Orchestration Challenge",
            "Building an AI system revealed that orchestrating multiple capabilities is harder than making one agent smarter.",
            "Architects",
            "Insights about AI system design",
            evidenceText);

        var selectedIdea = new SelectedIdea(idea);
        var input = new ResearchInput(
            new StoryInput("Technical report on AI system architecture."),
            selectedIdea);

        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        // Act
        await researchAgent.ExecuteAsync(executionContext, input, CancellationToken.None);

        // Assert
        Assert.NotNull(messageCaptureClient.CapturedMessages);

        var userMessage = messageCaptureClient.CapturedMessages
            .FirstOrDefault(m => m.Role == ChatRole.User);

        Assert.NotNull(userMessage);
        var messageText = userMessage.Text;
        // Verify the Evidence Verification section is present
        Assert.Contains("EVIDENCE VERIFICATION", messageText, StringComparison.Ordinal);
        Assert.Contains("Verify every relevant claim against the source", messageText, StringComparison.Ordinal);
        Assert.Contains("FACT", messageText, StringComparison.Ordinal);
        Assert.Contains("INTERPRETATION", messageText, StringComparison.Ordinal);
        Assert.Contains("UNKNOWN", messageText, StringComparison.Ordinal);
    }

    private sealed class MessageCapturingChatClient : IChatClient
    {
        public string? ResponseJson { get; init; }
        public List<ChatMessage> CapturedMessages { get; } = new();

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            // Capture all messages
            CapturedMessages.Clear();
            CapturedMessages.AddRange(messages);

            // Return minimal valid research response
            const string responseJson =
                """
                {
                  "content": "Test research content based on evidence analysis."
                }
                """;

            var message = new ChatMessage(ChatRole.Assistant, ResponseJson ?? responseJson);
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

    private sealed class LogCapture : ILogger<ResearchAgent>
    {
        public bool HasErrorLogs { get; private set; }

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
        }
    }
}
