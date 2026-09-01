using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Tests.Agents;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Workflows;

public sealed class StoryWorkflowTests
{
    [Fact]
    public async Task RunAsync_WithValidInput_ExecutesSuccessfully()
    {
        // Arrange
        var expectedIdea = new Idea
        {
            Title = "Test Idea",
            Description = "A test idea for verification",
            Audience = "Test Audience",
            Rationale = "To verify workflow execution"
        };

        var expectedResearch = new ResearchResult
        {
            Content = "Test research content"
        };

        var ideaAgent = new CapturingFakeAgent(expectedIdea);
        var researchAgent = new FakeResearchAgent(expectedResearch);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(input, result.Input);

        Assert.NotNull(result.SelectedIdea);
        Assert.Equal(expectedIdea.Title, result.SelectedIdea.Idea.Title);
        Assert.Equal(expectedIdea.Description, result.SelectedIdea.Idea.Description);
        Assert.Equal(expectedIdea.Audience, result.SelectedIdea.Idea.Audience);
        Assert.Equal(expectedIdea.Rationale, result.SelectedIdea.Idea.Rationale);

        Assert.NotNull(result.Research);
        Assert.Equal(expectedResearch.Content, result.Research.Content);
    }

    [Fact]
    public async Task RunAsync_WithValidInput_PassesInputToAgent()
    {
        // Arrange
        var expectedIdea = new Idea
        {
            Title = "Captured",
            Description = "Description",
            Audience = "Audience",
            Rationale = "Rationale"
        };

        var ideaAgent = new CapturingFakeAgent(expectedIdea);

        var researchResult = new ResearchResult
        {
            Content = "Test research content"
        };

        var researchAgent = new CapturingFakeResearchAgent(researchResult);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer);

        const string expectedContent = "This is the user's input";
        var input = new ChatInput
        {
            Content = expectedContent
        };

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(researchAgent.CapturedInput);

        Assert.NotNull(researchAgent.CapturedInput.Input);
        Assert.Equal(expectedContent, researchAgent.CapturedInput.Input.Content);

        Assert.NotNull(researchAgent.CapturedInput.SelectedIdea);
        Assert.Equal(
            expectedIdea.Title,
            researchAgent.CapturedInput.SelectedIdea.Idea.Title);
    }

    [Fact]
    public async Task RunAsync_WithCancellationToken_PropagatesTokenToAgent()
    {
        // Arrange
        var ideaAgent = new CapturingFakeAgent(new Idea
        {
            Title = "Captured",
            Description = "Description",
            Audience = "Audience",
            Rationale = "Rationale"
        });

        var researchAgent = new CapturingFakeResearchAgent(
            new ResearchResult
            {
                Content = "Test research content"
            });
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer);

        var input = new ChatInput
        {
            Content = "Test input"
        };

        using var cts = new CancellationTokenSource();

        // Act
        await workflow.RunAsync(input, cts.Token);

        // Assert
        Assert.NotNull(researchAgent.CapturedCancellationToken);

        Assert.NotEqual(
            CancellationToken.None,
            researchAgent.CapturedCancellationToken.Value);

        Assert.False(
            researchAgent.CapturedCancellationToken.Value.IsCancellationRequested);
    }

    [Fact]
    public async Task RunAsync_WhenAgentFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var ideaAgent = new FailingFakeAgent(
            new InvalidOperationException("Test agent failure"));

        var researchAgent = new FakeResearchAgent(
            new ResearchResult
            {
                Content = "Test research content"
            });

        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer);

        var input = new ChatInput
        {
            Content = "Test input"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.RunAsync(input));
    }

    /// <summary>
    ///     Integration test with the real StoryWorkflow, real MAF workflow,
    ///     and fake agents. Verifies the complete execution path from
    ///     ChatInput through idea generation, idea selection, and research.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithFakeAgent_ExecutesEndToEnd()
    {
        // Arrange
        var ideaAgent = new FakeIdeaAgent();

        var researchResult = new ResearchResult
        {
            Content = "Example research content"
        };

        var researchAgent = new FakeResearchAgent(researchResult);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer);

        var input = new ChatInput
        {
            Content = "Compose a story about a curious cat"
        };

        // Act
        var result = await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(input, result.Input);

        Assert.NotNull(result.SelectedIdea);
        Assert.NotNull(result.SelectedIdea.Idea);
        Assert.Equal("Example Idea", result.SelectedIdea.Idea.Title);
        Assert.NotNull(result.SelectedIdea.Idea.Description);
        Assert.NotNull(result.SelectedIdea.Idea.Audience);
        Assert.NotNull(result.SelectedIdea.Idea.Rationale);

        Assert.NotNull(result.Research);
        Assert.Equal("Example research content", result.Research.Content);
    }

    private sealed class CapturingFakeAgent(Idea? ideaToReturn = null) : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea>? _ideaToReturn = ideaToReturn is not null
            ? new List<Idea> { ideaToReturn }.AsReadOnly()
            : [];

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            AgentExecutionContext executionContext,
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(_ideaToReturn!);
        }
    }

    private sealed class FakeSynthesizerAgent : IAgent<SynthesisInput, SynthesisResult>
    {
        private readonly SynthesisResult _result;

        public FakeSynthesizerAgent(SynthesisResult result) => _result = result;

        public Task<SynthesisResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            SynthesisInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(_result);
        }
    }

    private sealed class FakeResearchAgent(
        ResearchResult researchResult) : IAgent<ResearchInput, ResearchResult>
    {
        public Task<ResearchResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ResearchInput input,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(researchResult);
    }

    private sealed class CapturingFakeResearchAgent(
        ResearchResult researchResult) : IAgent<ResearchInput, ResearchResult>
    {
        public ResearchInput? CapturedInput { get; private set; }

        public CancellationToken? CapturedCancellationToken { get; private set; }

        public Task<ResearchResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ResearchInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedInput = input;
            CapturedCancellationToken = cancellationToken;

            return Task.FromResult(researchResult);
        }
    }

    private sealed class FailingFakeAgent(Exception exceptionToThrow)
        : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly Exception _exceptionToThrow =
            exceptionToThrow ?? throw new ArgumentNullException(nameof(exceptionToThrow));

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            AgentExecutionContext executionContext,
            ChatInput input,
            CancellationToken cancellationToken = default) =>
            Task.FromException<IReadOnlyList<Idea>>(_exceptionToThrow);
    }
}