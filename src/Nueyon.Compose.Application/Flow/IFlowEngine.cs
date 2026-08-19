using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Flow;

public interface IFlowEngine
{
    Task<StoryWorkspace> ExecuteAsync(
        FlowExecutionContext executionContext,
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default);
}
