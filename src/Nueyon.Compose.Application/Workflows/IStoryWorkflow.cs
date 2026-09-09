using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public interface IStoryWorkflow
{
    Task<StoryWorkflowResult> RunAsync(
        StoryInput input,
        CancellationToken cancellationToken = default);
}