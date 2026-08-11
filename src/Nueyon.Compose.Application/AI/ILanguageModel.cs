namespace Nueyon.Compose.Application.AI;

public interface ILanguageModel
{
    Task<string> CompleteAsync(
        string prompt,
        CancellationToken cancellationToken = default);
}
