using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     StoryWorkflow represents the application-level workflow for creating a story.
///     Owns the Microsoft Agent Framework executor construction and workflow assembly.
/// </summary>
public sealed class StoryWorkflow : IStoryWorkflow
{
    /// <summary>
    ///     Initializes a new instance of the StoryWorkflow with the specified agent dependencies.
    ///     Internally constructs and manages the Microsoft Agent Framework executors.
    /// </summary>
    public StoryWorkflow(
        IAgent<StoryInput, IReadOnlyList<Idea>> ideaAgent,
        IAgent<ResearchInput, ResearchResult> researchAgent,
        IAgent<SynthesisInput, SynthesisResult> synthesizer,
        IAgent<NarrativeInput, NarrativeResult> narrativeAgent,
        IAgent<ComposeInput, ComposeResult> composeAgent)
    {
        ArgumentNullException.ThrowIfNull(ideaAgent);
        ArgumentNullException.ThrowIfNull(researchAgent);
        ArgumentNullException.ThrowIfNull(synthesizer);
        ArgumentNullException.ThrowIfNull(narrativeAgent);
        ArgumentNullException.ThrowIfNull(composeAgent);

        _ideaAgent = ideaAgent;
        _researchAgent = researchAgent;
        _synthesizer = synthesizer;
        _narrativeAgent = narrativeAgent;
        _composeAgent = composeAgent;
    }

    private readonly IAgent<ComposeInput, ComposeResult> _composeAgent;

    private readonly IAgent<StoryInput, IReadOnlyList<Idea>> _ideaAgent;
    private readonly IAgent<NarrativeInput, NarrativeResult> _narrativeAgent;
    private readonly IAgent<ResearchInput, ResearchResult> _researchAgent;
    private readonly IAgent<SynthesisInput, SynthesisResult> _synthesizer;

    /// <summary>
    ///     Executes the story workflow with the provided input.
    /// </summary>
    public async Task<StoryWorkflowResult> RunAsync(
        StoryInput input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        var executionContext = new AgentExecutionContext(Guid.NewGuid());

        var workflow = Build(executionContext);

        var run = await InProcessExecution.RunAsync(
            workflow,
            input,
            cancellationToken: cancellationToken);

        return StoryWorkflowResultExtractor.Extract(input, run);
    }

    /// <summary>
    ///     Builds a new MAF workflow for each execution.
    /// </summary>
    private Workflow Build(AgentExecutionContext executionContext)
    {
        var ideaExecutor = CreateIdeaExecutor(executionContext);
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor(executionContext);
        var synthesisExecutor = CreateSynthesisExecutor(executionContext);
        var narrativeExecutor = CreateNarrativeExecutor(executionContext);
        var composeExecutor = CreateComposeExecutor(executionContext);

        var builder = new WorkflowBuilder(ideaExecutor);

        builder.AddEdge(ideaExecutor, ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);

        builder.AddEdge(researchExecutor, synthesisExecutor);
        builder.AddEdge(synthesisExecutor, narrativeExecutor);
        builder.AddEdge(narrativeExecutor, composeExecutor);

        return builder.Build();
    }

    /// <summary>
    ///     Creates the Idea generation executor.
    /// </summary>
    private FunctionExecutor<StoryInput, Idea[]> CreateIdeaExecutor(AgentExecutionContext executionContext) =>
        new(
            "idea",
            async (input, context, cancellationToken) =>
            {
                await context.QueueStateUpdateAsync(
                    StoryWorkflowState.StoryInputKey,
                    input,
                    StoryWorkflowState.ScopeName,
                    cancellationToken);

                var ideas = await _ideaAgent.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);

                return ideas.ToArray();
            });

    /// <summary>
    ///     Creates the Idea selection executor (deterministically selects the first idea).
    /// </summary>
    private static FunctionExecutor<Idea[], SelectedIdea> CreateIdeaSelectionExecutor()
    {
        return new FunctionExecutor<Idea[], SelectedIdea>(
            "idea-selection",
            Handle);

        static SelectedIdea Handle(
            Idea[] ideas,
            IWorkflowContext context,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(ideas);

            if (ideas.Length == 0)
            {
                throw new InvalidOperationException(
                    "Cannot select an idea because no ideas were generated.");
            }

            return new SelectedIdea(ideas[0]);
        }
    }

    /// <summary>
    ///     Creates the Research executor.
    /// </summary>
    private FunctionExecutor<SelectedIdea, ResearchResult> CreateResearchExecutor(
        AgentExecutionContext executionContext) =>
        new(
            "research",
            async (selectedIdea, context, cancellationToken) =>
            {
                var input = await context.ReadStateAsync<StoryInput>(
                                StoryWorkflowState.StoryInputKey,
                                StoryWorkflowState.ScopeName,
                                cancellationToken)
                            ?? throw new InvalidOperationException(
                                "StoryInput was not found in the StoryWorkflow state.");

                var researchInput = new ResearchInput(
                    input,
                    selectedIdea);

                return await _researchAgent.ExecuteAsync(
                    executionContext,
                    researchInput,
                    cancellationToken);
            });

    private FunctionExecutor<ResearchResult, SynthesisResult> CreateSynthesisExecutor(
        AgentExecutionContext executionContext) =>
        new(
            "synthesis",
            async (research, _, cancellationToken) =>
            {
                var synthesisResearch = SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content);
                var input = new SynthesisInput(new ResearchForSynthesis(synthesisResearch));

                return await _synthesizer.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);
            });

    private FunctionExecutor<SynthesisResult, NarrativeResult> CreateNarrativeExecutor(
        AgentExecutionContext executionContext) =>
        new(
            "narrative",
            async (synthesis, _, cancellationToken) =>
            {
                var input = new NarrativeInput(new SynthesisForNarrative(synthesis.Content));

                return await _narrativeAgent.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);
            });

    private FunctionExecutor<NarrativeResult, ComposeResult> CreateComposeExecutor(
        AgentExecutionContext executionContext) =>
        new(
            "compose",
            async (narrative, _, cancellationToken) =>
            {
                var input = new ComposeInput(new NarrativeForCompose(narrative.Content), ContentFormat.Article);

                return await _composeAgent.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);
            });
}