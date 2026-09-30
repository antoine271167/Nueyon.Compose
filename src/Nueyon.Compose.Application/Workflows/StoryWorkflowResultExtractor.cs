using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     Extracts a StoryWorkflowResult from the MAF events produced by a StoryWorkflow run.
/// </summary>
internal static class StoryWorkflowResultExtractor
{
    /// <summary>
    ///     Extracts the domain results from the executor completion events of the given run.
    /// </summary>
    public static StoryWorkflowResult Extract(
        StoryInput input,
        Run run)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(run);

        SelectedIdea? selectedIdea = null;
        ResearchResult? research = null;
        SynthesisResult? synthesis = null;
        NarrativeResult? narrative = null;
        ComposeResult? compose = null;

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
                    Data: SelectedIdea selected
                }:
                    selectedIdea = selected;
                    break;

                case
                {
                    ExecutorId: "research",
                    Data: ResearchResult researchResult
                }:
                    research = researchResult;
                    break;
                case
                {
                    ExecutorId: "synthesis",
                    Data: SynthesisResult synthesisResult
                }:
                    synthesis = synthesisResult;
                    break;
                case
                {
                    ExecutorId: "narrative",
                    Data: NarrativeResult narrativeResult
                }:
                    narrative = narrativeResult;
                    break;
                case
                {
                    ExecutorId: "compose",
                    Data: ComposeResult composeResult
                }:
                    compose = composeResult;
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

        if (synthesis is null)
        {
            throw new InvalidOperationException(
                "The Story Workflow completed without producing a Synthesis result.");
        }

        if (narrative is null)
        {
            throw new InvalidOperationException(
                "The Story Workflow completed without producing a Narrative result.");
        }

        if (compose is null)
        {
            throw new InvalidOperationException(
                "The Story Workflow completed without producing a Compose result.");
        }

        return new StoryWorkflowResult(
            input,
            selectedIdea,
            research,
            synthesis,
            narrative,
            compose);
    }
}