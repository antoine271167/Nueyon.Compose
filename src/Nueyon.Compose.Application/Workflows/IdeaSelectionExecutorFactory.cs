using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public static class IdeaSelectionExecutorFactory
{
    public static FunctionExecutor<Idea[], SelectedIdea> CreateIdeaSelectionExecutor()
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
}