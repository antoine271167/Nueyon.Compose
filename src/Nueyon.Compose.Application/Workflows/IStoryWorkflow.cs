using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public interface IStoryWorkflow
{
    Task<StoryWorkflowResult> RunAsync(
        ChatInput input,
        CancellationToken cancellationToken = default);
}