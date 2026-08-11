using Nueyon.Compose.Application.Agents;

namespace Nueyon.Compose.Application.Harness;

public interface IAgentHarness<TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(
        TInput input,
        CancellationToken cancellationToken = default);
}
