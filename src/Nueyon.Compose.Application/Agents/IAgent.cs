namespace Nueyon.Compose.Application.Agents;

public interface IAgent<TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(
        TInput input,
        CancellationToken cancellationToken = default);
}
