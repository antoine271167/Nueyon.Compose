using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Services;

/// <summary>
///     Loads source material from a directory of Markdown files and combines them into a StoryInput.
/// </summary>
public interface ISourceContextLoader
{
    /// <summary>
    ///     Loads all Markdown files from the specified directory and combines them into a single StoryInput.
    /// </summary>
    /// <param name="directory">The directory path to search for Markdown files.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A StoryInput containing the combined content of all Markdown files.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no Markdown files are found in the directory.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    Task<StoryInput> LoadAsync(
        string directory,
        CancellationToken cancellationToken = default);
}
