using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Workflows;

public sealed class StoryWorkflowResultExtractorTests
{
    private static readonly Idea _sampleIdea = new(
        "Test Idea",
        "A test idea for verification",
        "Test Audience",
        "To verify extractor behavior",
        "The source material provides concrete support for this idea.");

    private static readonly SelectedIdea _sampleSelectedIdea = new(_sampleIdea);
    private static readonly ResearchResult _sampleResearch = new("research content");
    private static readonly SynthesisResult _sampleSynthesis = new("synthesis content");
    private static readonly NarrativeResult _sampleNarrative = new("narrative content");
    private static readonly ComposeResult _sampleCompose = new("compose content");

    [Fact]
    public async Task Extract_WithAllRequiredCompletionEvents_ReturnsCompleteResult()
    {
        // Arrange
        var input = new StoryInput("Test input");
        var run = await RunFullWorkflowAsync(input);

        // Act
        var result = StoryWorkflowResultExtractor.Extract(input, run);

        // Assert
        Assert.Equal(input, result.Input);
        Assert.Equal(_sampleSelectedIdea, result.SelectedIdea);
        Assert.Equal(_sampleResearch, result.Research);
        Assert.Equal(_sampleSynthesis, result.Synthesis);
        Assert.Equal(_sampleNarrative, result.Narrative);
        Assert.Equal(_sampleCompose, result.Compose);
    }

    [Fact]
    public async Task Extract_WithNonExecutorCompletedEvents_IgnoresThemAndStillReturnsCompleteResult()
    {
        // Arrange
        // A real InProcessExecution run naturally emits other event types in addition to
        // ExecutorCompletedEvent (e.g. workflow lifecycle events), so a normal successful
        // run exercises the "ignore non-completion events" path.
        var input = new StoryInput("Test input");
        var run = await RunFullWorkflowAsync(input);

        Assert.Contains(run.OutgoingEvents, e => e is not ExecutorCompletedEvent);

        // Act
        var result = StoryWorkflowResultExtractor.Extract(input, run);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Extract_WithMissingSelectedIdea_ThrowsInvalidOperationException()
    {
        // Arrange: a workflow with no matching executors at all, so no required results are produced.
        var input = new StoryInput("Test input");
        var noopExecutor = new FunctionExecutor<StoryInput, StoryInput>(
            "noop",
            (value, _, _) => new ValueTask<StoryInput>(value));

        var builder = new WorkflowBuilder(noopExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    [Fact]
    public async Task Extract_WithMissingResearch_ThrowsInvalidOperationException()
    {
        // Arrange: only the idea-selection stage runs.
        var input = new StoryInput("Test input");
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();

        var builder = new WorkflowBuilder(ideaSelectionExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    [Fact]
    public async Task Extract_WithMissingSynthesis_ThrowsInvalidOperationException()
    {
        // Arrange: idea-selection and research stages run, but synthesis does not.
        var input = new StoryInput("Test input");
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor();

        var builder = new WorkflowBuilder(ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    [Fact]
    public async Task Extract_WithMissingNarrative_ThrowsInvalidOperationException()
    {
        // Arrange: idea-selection, research, and synthesis stages run, but narrative does not.
        var input = new StoryInput("Test input");
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor();
        var synthesisExecutor = CreateSynthesisExecutor();

        var builder = new WorkflowBuilder(ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);
        builder.AddEdge(researchExecutor, synthesisExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    [Fact]
    public async Task Extract_WithMissingCompose_ThrowsInvalidOperationException()
    {
        // Arrange: every stage runs except compose.
        var input = new StoryInput("Test input");
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor();
        var synthesisExecutor = CreateSynthesisExecutor();
        var narrativeExecutor = CreateNarrativeExecutor();

        var builder = new WorkflowBuilder(ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);
        builder.AddEdge(researchExecutor, synthesisExecutor);
        builder.AddEdge(synthesisExecutor, narrativeExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    [Fact]
    public async Task Extract_WithIncompatibleDataTypeForIdeaSelectionExecutorId_ThrowsInvalidOperationException()
    {
        // Arrange: the "idea-selection" executor completes with an incompatible payload type,
        // which must be treated as an invalid/missing result rather than a usable one.
        var input = new StoryInput("Test input");
        var corruptIdeaSelectionExecutor = new FunctionExecutor<StoryInput, string>(
            "idea-selection",
            (_, _, _) => new ValueTask<string>("not-a-selected-idea"));

        var builder = new WorkflowBuilder(corruptIdeaSelectionExecutor);
        var run = await InProcessExecution.RunAsync(builder.Build(), input);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => StoryWorkflowResultExtractor.Extract(input, run));
    }

    private static async Task<Run> RunFullWorkflowAsync(StoryInput input)
    {
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor();
        var synthesisExecutor = CreateSynthesisExecutor();
        var narrativeExecutor = CreateNarrativeExecutor();
        var composeExecutor = CreateComposeExecutor();

        var builder = new WorkflowBuilder(ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);
        builder.AddEdge(researchExecutor, synthesisExecutor);
        builder.AddEdge(synthesisExecutor, narrativeExecutor);
        builder.AddEdge(narrativeExecutor, composeExecutor);

        return await InProcessExecution.RunAsync(builder.Build(), input);
    }

    private static FunctionExecutor<StoryInput, SelectedIdea> CreateIdeaSelectionExecutor() =>
        new(
            "idea-selection",
            (_, _, _) => new ValueTask<SelectedIdea>(_sampleSelectedIdea));

    private static FunctionExecutor<SelectedIdea, ResearchResult> CreateResearchExecutor() =>
        new(
            "research",
            (_, _, _) => new ValueTask<ResearchResult>(_sampleResearch));

    private static FunctionExecutor<ResearchResult, SynthesisResult> CreateSynthesisExecutor() =>
        new(
            "synthesis",
            (_, _, _) => new ValueTask<SynthesisResult>(_sampleSynthesis));

    private static FunctionExecutor<SynthesisResult, NarrativeResult> CreateNarrativeExecutor() =>
        new(
            "narrative",
            (_, _, _) => new ValueTask<NarrativeResult>(_sampleNarrative));

    private static FunctionExecutor<NarrativeResult, ComposeResult> CreateComposeExecutor() =>
        new(
            "compose",
            (_, _, _) => new ValueTask<ComposeResult>(_sampleCompose));
}