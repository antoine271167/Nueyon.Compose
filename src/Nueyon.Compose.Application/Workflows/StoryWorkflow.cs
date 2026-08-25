using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
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
        IAgent<ChatInput, IReadOnlyList<Idea>> ideaAgent,
        IAgent<ResearchInput, ResearchResult> researchAgent)
    {
        ArgumentNullException.ThrowIfNull(ideaAgent);
        ArgumentNullException.ThrowIfNull(researchAgent);

        _ideaAgent = ideaAgent;
        _researchAgent = researchAgent;
    }

    private readonly IAgent<ChatInput, IReadOnlyList<Idea>> _ideaAgent;
    private readonly IAgent<ResearchInput, ResearchResult> _researchAgent;

    /// <summary>
    ///     Executes the story workflow with the provided input.
    /// </summary>
    public async Task<StoryWorkflowResult> RunAsync(
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        var workflow = Build();

        var run = await InProcessExecution.RunAsync(
            workflow,
            input,
            cancellationToken: cancellationToken);

        return ExtractResult(input, run);
    }

    /// <summary>
    ///     Builds a new MAF workflow for each execution.
    /// </summary>
    private Workflow Build()
    {
        var ideaExecutor = CreateIdeaExecutor();
        var ideaSelectionExecutor = CreateIdeaSelectionExecutor();
        var researchExecutor = CreateResearchExecutor();

        var builder = new WorkflowBuilder(ideaExecutor);

        builder.AddEdge(ideaExecutor, ideaSelectionExecutor);
        builder.AddEdge(ideaSelectionExecutor, researchExecutor);

        return builder.Build();
    }

    /// <summary>
    ///     Creates the Idea generation executor.
    /// </summary>
    private FunctionExecutor<ChatInput, Idea[]> CreateIdeaExecutor()
    {
        return new FunctionExecutor<ChatInput, Idea[]>(
            "idea",
            async (input, context, cancellationToken) =>
            {
                await context.QueueStateUpdateAsync(
                    StoryWorkflowState.ChatInputKey,
                    input,
                    StoryWorkflowState.ScopeName,
                    cancellationToken);

                var executionContext = new AgentExecutionContext(Guid.NewGuid());

                var ideas = await _ideaAgent.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);

                return ideas.ToArray();
            });
    }

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
    private FunctionExecutor<SelectedIdea, ResearchResult> CreateResearchExecutor()
    {
        return new FunctionExecutor<SelectedIdea, ResearchResult>(
            "research",
            async (selectedIdea, context, cancellationToken) =>
            {
                var input = await context.ReadStateAsync<ChatInput>(
                                StoryWorkflowState.ChatInputKey,
                                StoryWorkflowState.ScopeName,
                                cancellationToken)
                            ?? throw new InvalidOperationException(
                                "ChatInput was not found in the StoryWorkflow state.");

                var researchInput = new ResearchInput
                {
                    Input = input,
                    SelectedIdea = selectedIdea
                };

                var executionContext = new AgentExecutionContext(Guid.NewGuid());

                return await _researchAgent.ExecuteAsync(
                    executionContext,
                    researchInput,
                    cancellationToken);
            });
    }

    private static StoryWorkflowResult ExtractResult(
        ChatInput input,
        Run run)
    {
        SelectedIdea? selectedIdea = null;
        ResearchResult? research = null;

        foreach (var @event in run.OutgoingEvents)
        {
            if (@event is not ExecutorCompletedEvent completedEvent)
            {
                continue;
            }

            switch (completedEvent)
            {
                case
                {
                    ExecutorId: "idea-selection",
                    Data: SelectedIdea result
                }:
                    selectedIdea = result;
                    break;

                case
                {
                    ExecutorId: "research",
                    Data: ResearchResult result
                }:
                    research = result;
                    break;
            }
        }

        if (selectedIdea is null)
        {
            throw new InvalidOperationException(
                "The Story Workflow completed without producing a selected Idea result.");
        }

        if (research is null)
        {
            throw new InvalidOperationException(
                "The Story Workflow completed without producing a Research result.");
        }

        return new StoryWorkflowResult
        {
            Input = input,
            SelectedIdea = selectedIdea,
            Research = research
        };
    }
}