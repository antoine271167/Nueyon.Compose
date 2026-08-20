using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     Represents the application-level workflow contract for story/article creation.
///     This interface abstracts the workflow implementation details and serves as the boundary
///     between the application layer and the workflow engine.
/// </summary>
public interface IStoryWorkflow
{
    /// <summary>
    ///     Executes the story workflow with the provided input.
    /// </summary>
    /// <param name="input">The chat input to process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the generated ideas.</returns>
    /// <exception cref="ArgumentNullException">Thrown when input is null.</exception>
    Task<IReadOnlyList<Idea>> RunAsync(
        ChatInput input,
        CancellationToken cancellationToken = default);
}