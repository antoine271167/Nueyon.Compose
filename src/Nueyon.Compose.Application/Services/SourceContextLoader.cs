using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Services;

/// <summary>
///     Loads Markdown files from a directory and combines them into source material for the workflow.
/// </summary>
public sealed class SourceContextLoader : ISourceContextLoader
{
    /// <summary>
    ///     Loads all Markdown files from the specified directory and combines them into a single StoryInput.
    ///     Files are sorted deterministically by name using ordinal comparison.
    ///     File boundaries are preserved with clear markers.
    /// </summary>
    /// <param name="directory">The directory path to search for Markdown files.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A StoryInput containing the combined content of all Markdown files with boundaries.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no Markdown files are found in the directory.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public async Task<StoryInput> LoadAsync(
        string directory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(directory);

        cancellationToken.ThrowIfCancellationRequested();

        // Validate directory exists
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directory}");
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Find all markdown files, sorted deterministically by ordinal comparison
        var markdownFiles = Directory
            .GetFiles(directory, "*.md", SearchOption.TopDirectoryOnly)
            .OrderBy(f => Path.GetFileName(f), StringComparer.Ordinal)
            .ToList();

        // Validate at least one markdown file exists
        if (markdownFiles.Count == 0)
        {
            throw new InvalidOperationException(
                $"No Markdown files (*.md) found in directory: {directory}");
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Load all files and combine them with boundaries
        var combinedContent = new System.Text.StringBuilder();

        foreach (var filePath in markdownFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fileName = Path.GetFileName(filePath);
            var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);

            combinedContent.AppendLine($"--- BEGIN SOURCE: {fileName} ---");
            combinedContent.AppendLine();
            combinedContent.Append(fileContent);
            combinedContent.AppendLine();
            combinedContent.AppendLine($"--- END SOURCE: {fileName} ---");
            combinedContent.AppendLine();
        }

        cancellationToken.ThrowIfCancellationRequested();

        return new StoryInput(combinedContent.ToString());
    }
}
