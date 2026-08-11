using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Flow;

public interface IFlowEngine
{
    Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default);
}
