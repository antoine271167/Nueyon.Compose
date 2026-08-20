using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents;

public interface IAgent<in TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(
        AgentExecutionContext executionContext,
        TInput input,
        CancellationToken cancellationToken = default);
}
