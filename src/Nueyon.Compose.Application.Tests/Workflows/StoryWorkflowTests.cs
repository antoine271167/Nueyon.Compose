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
        var expectedIdea = new Idea(
            "Test Idea",
            "A test idea for verification",
            "Test Audience",
            "To verify workflow execution",
            "The source material provides concrete support for this idea."
        );

        var expectedResearch = new ResearchResult(
            "Test research content"
        );

        var ideaAgent = new CapturingFakeAgent(expectedIdea);
        var researchAgent = new FakeResearchAgent(expectedResearch);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Test input");

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

        Assert.NotNull(result.Synthesis);
        Assert.Equal("synthesis content", result.Synthesis.Content);

        Assert.NotNull(result.Narrative);
        Assert.Equal("narrative content", result.Narrative.Content);

        Assert.NotNull(result.Compose);
        Assert.Equal("complete article content", result.Compose.Content);
    }

    [Fact]
    public async Task RunAsync_WithValidInput_PassesInputToAgent()
    {
        // Arrange
        var expectedIdea = new Idea(
            "Captured",
            "Description",
            "Audience",
            "Rationale",
            "Evidence from source material."
        );

        var ideaAgent = new CapturingFakeAgent(expectedIdea);

        var researchResult = new ResearchResult("Test research content");

        var researchAgent = new CapturingFakeResearchAgent(researchResult);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        const string expectedContent = "This is the user's input";
        var input = new StoryInput(expectedContent);

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
    public async Task RunAsync_WithFullResearchSections_PassesOnlyFactsAndUnknownsToSynthesizer()
    {
        // Arrange
        var ideaAgent = new CapturingFakeAgent(new Idea(
            "Captured",
            "Description",
            "Audience",
            "Rationale",
            "Evidence from source."));

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

        var researchAgent = new FakeResearchAgent(new ResearchResult(fullResearch));
        var synthesizer = new CapturingFakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(synthesizer.CapturedInput);
        var synthesisResearchContent = synthesizer.CapturedInput.Research.Content;

        Assert.Contains(
            "The author renamed the product from StoryFlow to Compose.",
            synthesisResearchContent,
            StringComparison.Ordinal);
        Assert.Contains(
            "The exact date of the rename is not established.",
            synthesisResearchContent,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "The rename reflects a shift in product positioning.",
            synthesisResearchContent,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "The rename occurred after the initial launch.",
            synthesisResearchContent,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "Facts 1 is relevant to the Selected Idea.",
            synthesisResearchContent,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task RunAsync_WithCancellationToken_PropagatesTokenToAgent()
    {
        // Arrange
        var ideaAgent = new CapturingFakeAgent(new Idea(
            "Captured",
            "Description",
            "Audience",
            "Rationale",
            "Evidence from source."));

        var researchAgent = new CapturingFakeResearchAgent(
            new ResearchResult(
                "Test research content"
            ));

        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput(
            "Test input"
        );

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
            new ResearchResult("Test research content"));

        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Test input");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.RunAsync(input));
    }

    /// <summary>
    ///     Integration test with the real StoryWorkflow, real MAF workflow,
    ///     and fake agents. Verifies the complete execution path from
    ///     StoryInput through idea generation, idea selection, and research.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithFakeAgent_ExecutesEndToEnd()
    {
        // Arrange
        var ideaAgent = new FakeIdeaAgent();

        var researchResult = new ResearchResult("Example research content");

        var researchAgent = new FakeResearchAgent(researchResult);
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Compose a story about a curious cat");

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

        Assert.NotNull(result.Synthesis);
        Assert.Equal("synthesis content", result.Synthesis.Content);

        Assert.NotNull(result.Narrative);
        Assert.Equal("narrative content", result.Narrative.Content);

        Assert.NotNull(result.Compose);
        Assert.Equal("complete article content", result.Compose.Content);
    }

    [Fact]
    public async Task RunAsync_VerifiesNarrativeToBoundary()
    {
        // Arrange
        const string narrativeContent = "narrative content";

        var ideaAgent = new CapturingFakeAgent(new Idea(
            "Test Idea",
            "Test description",
            "Test audience",
            "Test rationale",
            "Test evidence."));

        var researchAgent = new FakeResearchAgent(new ResearchResult("research content"));
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult(narrativeContent));
        var composeAgent = new CapturingFakeComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(composeAgent.CapturedInput);
        Assert.Equal(narrativeContent, composeAgent.CapturedInput.Narrative.Content);
        Assert.Equal(ContentFormat.Article, composeAgent.CapturedInput.Composition.Format);
    }

    [Fact]
    public async Task RunAsync_VerifiesSynthesisToNarrativeBoundary()
    {
        // Arrange
        const string synthesisContent = "distinctive synthesis content";

        var ideaAgent = new CapturingFakeAgent(new Idea(
            "Test Idea",
            "Test description",
            "Test audience",
            "Test rationale",
            "Test evidence."));

        var researchAgent = new FakeResearchAgent(new ResearchResult("research content"));
        var synthesizer = new CapturingFakeSynthesizerAgent(new SynthesisResult(synthesisContent));
        var narrativeAgent = new CapturingFakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(narrativeAgent.CapturedInput);
        Assert.Equal(
            synthesisContent,
            narrativeAgent.CapturedInput.Synthesis.Content);
    }

    [Fact]
    public async Task RunAsync_WithIdea_PreservesEvidenceInSelectedIdea()
    {
        // Arrange
        const string expectedEvidence = "The source explicitly states this important fact.";

        var expectedIdea = new Idea(
            "Test Idea",
            "Test description",
            "Test audience",
            "Test rationale",
            expectedEvidence);

        var ideaAgent = new CapturingFakeAgent(expectedIdea);
        var researchAgent = new CapturingFakeResearchAgent(new ResearchResult("research content"));
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(researchAgent.CapturedInput);
        Assert.NotNull(researchAgent.CapturedInput.SelectedIdea);
        Assert.Equal(expectedEvidence, researchAgent.CapturedInput.SelectedIdea.Idea.Evidence);
    }

    [Fact]
    public async Task RunAsync_WithIdea_SelectedIdeaContainsAllProperties()
    {
        // Arrange
        const string title = "Evidence Test Idea";
        const string description = "Testing evidence preservation";
        const string audience = "Testing Team";
        const string rationale = "To verify the experiment";
        const string evidence = "The source provides concrete evidence: specific event X happened.";

        var expectedIdea = new Idea(title, description, audience, rationale, evidence);
        var ideaAgent = new CapturingFakeAgent(expectedIdea);
        var researchAgent = new CapturingFakeResearchAgent(new ResearchResult("research content"));
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var input = new StoryInput("Test input");

        // Act
        var result = await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(result.SelectedIdea);
        Assert.Equal(title, result.SelectedIdea.Idea.Title);
        Assert.Equal(description, result.SelectedIdea.Idea.Description);
        Assert.Equal(audience, result.SelectedIdea.Idea.Audience);
        Assert.Equal(rationale, result.SelectedIdea.Idea.Rationale);
        Assert.Equal(evidence, result.SelectedIdea.Idea.Evidence);
    }

    [Fact]
    public async Task RunAsync_WithValidInput_AllAgentsReceiveSameExecutionId()
    {
        // Arrange
        var ideaAgent = new ExecutionContextCapturingIdeaAgent(new Idea(
            "Test Idea",
            "Description",
            "Audience",
            "Rationale",
            "Evidence from source material."));

        var researchAgent = new ExecutionContextCapturingResearchAgent(new ResearchResult("research content"));
        var synthesizer = new ExecutionContextCapturingSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new ExecutionContextCapturingNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new ExecutionContextCapturingComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(ideaAgent.CapturedExecutionContext);
        Assert.NotNull(researchAgent.CapturedExecutionContext);
        Assert.NotNull(synthesizer.CapturedExecutionContext);
        Assert.NotNull(narrativeAgent.CapturedExecutionContext);
        Assert.NotNull(composeAgent.CapturedExecutionContext);

        var executionId = ideaAgent.CapturedExecutionContext.ExecutionId;

        Assert.NotEqual(Guid.Empty, executionId);
        Assert.Equal(executionId, researchAgent.CapturedExecutionContext.ExecutionId);
        Assert.Equal(executionId, synthesizer.CapturedExecutionContext.ExecutionId);
        Assert.Equal(executionId, narrativeAgent.CapturedExecutionContext.ExecutionId);
        Assert.Equal(executionId, composeAgent.CapturedExecutionContext.ExecutionId);
    }

    [Fact]
    public async Task RunAsync_CalledTwice_ProducesDifferentExecutionIds()
    {
        // Arrange
        var ideaAgent = new ExecutionContextCapturingIdeaAgent(new Idea(
            "Test Idea",
            "Description",
            "Audience",
            "Rationale",
            "Evidence from source material."));

        var researchAgent = new ExecutionContextCapturingResearchAgent(new ResearchResult("research content"));
        var synthesizer = new ExecutionContextCapturingSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new ExecutionContextCapturingNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new ExecutionContextCapturingComposeAgent(new ComposeResult("composed content"));

        var workflow = new StoryWorkflow(ideaAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);

        var input = new StoryInput("Test input");

        // Act
        await workflow.RunAsync(input);
        var firstExecutionId = ideaAgent.CapturedExecutionContext!.ExecutionId;

        await workflow.RunAsync(input);
        var secondExecutionId = ideaAgent.CapturedExecutionContext!.ExecutionId;

        // Assert
        Assert.NotEqual(firstExecutionId, secondExecutionId);
    }

    private sealed class CapturingFakeAgent(Idea? ideaToReturn = null) : IAgent<StoryInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea>? _ideaToReturn = ideaToReturn is not null
            ? new List<Idea> { ideaToReturn }.AsReadOnly()
            : null;

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            AgentExecutionContext executionContext,
            StoryInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(_ideaToReturn!);
        }
    }

    private sealed class FakeSynthesizerAgent(SynthesisResult result) : IAgent<SynthesisInput, SynthesisResult>
    {
        public Task<SynthesisResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            SynthesisInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(result);
        }
    }

    private sealed class CapturingFakeSynthesizerAgent(SynthesisResult result) : IAgent<SynthesisInput, SynthesisResult>
    {
        public SynthesisInput? CapturedInput { get; private set; }

        public Task<SynthesisResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            SynthesisInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedInput = input;

            return Task.FromResult(result);
        }
    }

    private sealed class FakeNarrativeAgent(NarrativeResult result) : IAgent<NarrativeInput, NarrativeResult>
    {
        public Task<NarrativeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            NarrativeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(result);
        }
    }

    private sealed class CapturingFakeNarrativeAgent(NarrativeResult result) : IAgent<NarrativeInput, NarrativeResult>
    {
        public NarrativeInput? CapturedInput { get; private set; }

        public Task<NarrativeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            NarrativeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedInput = input;

            return Task.FromResult(result);
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
        : IAgent<StoryInput, IReadOnlyList<Idea>>
    {
        private readonly Exception _exceptionToThrow =
            exceptionToThrow ?? throw new ArgumentNullException(nameof(exceptionToThrow));

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            AgentExecutionContext executionContext,
            StoryInput input,
            CancellationToken cancellationToken = default) =>
            Task.FromException<IReadOnlyList<Idea>>(_exceptionToThrow);
    }

    private sealed class FakeComposeAgent(ComposeResult result) : IAgent<ComposeInput, ComposeResult>
    {
        public Task<ComposeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ComposeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            return Task.FromResult(result);
        }
    }

    private sealed class CapturingFakeComposeAgent(ComposeResult result) : IAgent<ComposeInput, ComposeResult>
    {
        public ComposeInput? CapturedInput { get; private set; }

        public Task<ComposeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ComposeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedInput = input;

            return Task.FromResult(result);
        }
    }

    private sealed class ExecutionContextCapturingIdeaAgent(Idea ideaToReturn)
        : IAgent<StoryInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea> _ideaToReturn = new List<Idea> { ideaToReturn }.AsReadOnly();

        public AgentExecutionContext? CapturedExecutionContext { get; private set; }

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            AgentExecutionContext executionContext,
            StoryInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedExecutionContext = executionContext;

            return Task.FromResult(_ideaToReturn);
        }
    }

    private sealed class ExecutionContextCapturingResearchAgent(ResearchResult result)
        : IAgent<ResearchInput, ResearchResult>
    {
        public AgentExecutionContext? CapturedExecutionContext { get; private set; }

        public Task<ResearchResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ResearchInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedExecutionContext = executionContext;

            return Task.FromResult(result);
        }
    }

    private sealed class ExecutionContextCapturingSynthesizerAgent(SynthesisResult result)
        : IAgent<SynthesisInput, SynthesisResult>
    {
        public AgentExecutionContext? CapturedExecutionContext { get; private set; }

        public Task<SynthesisResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            SynthesisInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedExecutionContext = executionContext;

            return Task.FromResult(result);
        }
    }

    private sealed class ExecutionContextCapturingNarrativeAgent(NarrativeResult result)
        : IAgent<NarrativeInput, NarrativeResult>
    {
        public AgentExecutionContext? CapturedExecutionContext { get; private set; }

        public Task<NarrativeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            NarrativeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedExecutionContext = executionContext;

            return Task.FromResult(result);
        }
    }

    private sealed class ExecutionContextCapturingComposeAgent(ComposeResult result)
        : IAgent<ComposeInput, ComposeResult>
    {
        public AgentExecutionContext? CapturedExecutionContext { get; private set; }

        public Task<ComposeResult> ExecuteAsync(
            AgentExecutionContext executionContext,
            ComposeInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedExecutionContext = executionContext;

            return Task.FromResult(result);
        }
    }
}