using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents;

public interface IAgent<in TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(
        FlowExecutionContext executionContext,
        TInput input,
        CancellationToken cancellationToken = default);
}
