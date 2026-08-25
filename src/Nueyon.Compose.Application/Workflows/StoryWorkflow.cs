using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     StoryWorkflow represents the application-level workflow for creating a story.
/// </summary>
public sealed class StoryWorkflow : IStoryWorkflow
{
    /// <summary>
    ///     Initializes a new instance of the StoryWorkflow with the specified executors.
    /// </summary>
    public StoryWorkflow(
        FunctionExecutor<ChatInput, Idea[]> ideaExecutor,
        FunctionExecutor<Idea[], SelectedIdea> ideaSelectionExecutor,
        FunctionExecutor<SelectedIdea, ResearchResult> researchExecutor)
    {
        _ideaExecutor = ideaExecutor ??
                        throw new ArgumentNullException(nameof(ideaExecutor));

        _ideaSelectionExecutor = ideaSelectionExecutor ??
                                 throw new ArgumentNullException(nameof(ideaSelectionExecutor));

        _researchExecutor = researchExecutor ??
                            throw new ArgumentNullException(nameof(researchExecutor));
    }

    private readonly FunctionExecutor<ChatInput, Idea[]> _ideaExecutor;

    private readonly FunctionExecutor<Idea[], SelectedIdea> _ideaSelectionExecutor;

    private readonly FunctionExecutor<SelectedIdea, ResearchResult> _researchExecutor;

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
        var builder = new WorkflowBuilder(_ideaExecutor);

        builder.AddEdge(_ideaExecutor, _ideaSelectionExecutor);
        builder.AddEdge(_ideaSelectionExecutor, _researchExecutor);

        return builder.Build();
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