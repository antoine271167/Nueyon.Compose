namespace Nueyon.Compose.Application.Prompts;

/// <summary>
///     Temporarily selects a complete disk prompt set for the current asynchronous execution.
/// </summary>
public static class PromptDirectory
{
    private static readonly AsyncLocal<string?> DirectorySlot = new();
    internal static string? Current => DirectorySlot.Value;

    public static IDisposable Use(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        var previous = DirectorySlot.Value;
        DirectorySlot.Value = Path.GetFullPath(directory);
        return new Scope(previous);
    }

    private sealed class Scope(string? previous) : IDisposable
    {
        public void Dispose() => DirectorySlot.Value = previous;
    }
}