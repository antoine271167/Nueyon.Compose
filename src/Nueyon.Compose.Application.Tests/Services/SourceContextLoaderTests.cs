using Nueyon.Compose.Application.Services;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Services;

public sealed class SourceContextLoaderTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly ISourceContextLoader _loader = new SourceContextLoader();

    public SourceContextLoaderTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"SourceContextLoaderTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task LoadAsync_WithSingleMarkdownFile_LoadsFileContent()
    {
        // Arrange
        const string fileName = "test.md";
        const string fileContent = "# Test Content\n\nThis is a test.";
        var filePath = Path.Combine(_tempDirectory, fileName);
        await File.WriteAllTextAsync(filePath, fileContent);

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("--- BEGIN SOURCE: test.md ---", result.Content);
        Assert.Contains("--- END SOURCE: test.md ---", result.Content);
        Assert.Contains("# Test Content", result.Content);
        Assert.Contains("This is a test.", result.Content);
    }

    [Fact]
    public async Task LoadAsync_WithMultipleMarkdownFiles_CombinesAllFiles()
    {
        // Arrange
        var files = new Dictionary<string, string>
        {
            { "file1.md", "Content of file 1" },
            { "file2.md", "Content of file 2" },
            { "file3.md", "Content of file 3" }
        };

        foreach (var (fileName, content) in files)
        {
            await File.WriteAllTextAsync(Path.Combine(_tempDirectory, fileName), content);
        }

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Content of file 1", result.Content);
        Assert.Contains("Content of file 2", result.Content);
        Assert.Contains("Content of file 3", result.Content);
        Assert.Contains("--- BEGIN SOURCE: file1.md ---", result.Content);
        Assert.Contains("--- BEGIN SOURCE: file2.md ---", result.Content);
        Assert.Contains("--- BEGIN SOURCE: file3.md ---", result.Content);
    }

    [Fact]
    public async Task LoadAsync_WithMultipleFiles_SortsFilesDeterministically()
    {
        // Arrange
        var fileNames = new[] { "zebra.md", "apple.md", "monkey.md" };
        foreach (var fileName in fileNames)
        {
            await File.WriteAllTextAsync(
                Path.Combine(_tempDirectory, fileName),
                $"Content of {fileName}");
        }

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        // Check that files appear in ordinal sorted order
        var appleIndex = result.Content.IndexOf("--- BEGIN SOURCE: apple.md ---", StringComparison.Ordinal);
        var monkeyIndex = result.Content.IndexOf("--- BEGIN SOURCE: monkey.md ---", StringComparison.Ordinal);
        var zebraIndex = result.Content.IndexOf("--- BEGIN SOURCE: zebra.md ---", StringComparison.Ordinal);

        Assert.True(appleIndex < monkeyIndex, "apple.md should appear before monkey.md");
        Assert.True(monkeyIndex < zebraIndex, "monkey.md should appear before zebra.md");
    }

    [Fact]
    public async Task LoadAsync_WithMultipleFiles_PreservesFileBoundaries()
    {
        // Arrange
        var files = new Dictionary<string, string>
        {
            { "first.md", "First content" },
            { "second.md", "Second content" }
        };

        foreach (var (fileName, content) in files)
        {
            await File.WriteAllTextAsync(Path.Combine(_tempDirectory, fileName), content);
        }

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        var firstBegin = result.Content.IndexOf("--- BEGIN SOURCE: first.md ---", StringComparison.Ordinal);
        var firstEnd = result.Content.IndexOf("--- END SOURCE: first.md ---", StringComparison.Ordinal);
        var secondBegin = result.Content.IndexOf("--- BEGIN SOURCE: second.md ---", StringComparison.Ordinal);
        var secondEnd = result.Content.IndexOf("--- END SOURCE: second.md ---", StringComparison.Ordinal);

        Assert.True(firstBegin >= 0, "first.md BEGIN marker should exist");
        Assert.True(firstEnd > firstBegin, "first.md END marker should come after BEGIN");
        Assert.True(secondBegin > firstEnd, "second.md BEGIN should come after first.md END");
        Assert.True(secondEnd > secondBegin, "second.md END should come after BEGIN");
    }

    [Fact]
    public async Task LoadAsync_PreservesMarkdownContent()
    {
        // Arrange
        const string markdownContent = @"# Heading 1
## Heading 2

This is a paragraph with **bold** and *italic*.

- Bullet 1
- Bullet 2

1. Numbered 1
2. Numbered 2

```code
Some code
```";
        var filePath = Path.Combine(_tempDirectory, "markdown.md");
        await File.WriteAllTextAsync(filePath, markdownContent);

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.Contains("# Heading 1", result.Content);
        Assert.Contains("## Heading 2", result.Content);
        Assert.Contains("**bold**", result.Content);
        Assert.Contains("*italic*", result.Content);
        Assert.Contains("- Bullet 1", result.Content);
        Assert.Contains("1. Numbered 1", result.Content);
        Assert.Contains("```code", result.Content);
    }

    [Fact]
    public async Task LoadAsync_WithNonExistentDirectory_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        var nonExistentDirectory = Path.Combine(_tempDirectory, "does_not_exist");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DirectoryNotFoundException>(
            () => _loader.LoadAsync(nonExistentDirectory));

        Assert.Contains("Directory not found", exception.Message);
    }

    [Fact]
    public async Task LoadAsync_WithEmptyDirectory_ThrowsInvalidOperationException()
    {
        // Arrange
        var emptyDirectory = Path.Combine(_tempDirectory, "empty");
        Directory.CreateDirectory(emptyDirectory);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loader.LoadAsync(emptyDirectory));

        Assert.Contains("No Markdown files", exception.Message);
    }

    [Fact]
    public async Task LoadAsync_WithOnlyNonMarkdownFiles_ThrowsInvalidOperationException()
    {
        // Arrange
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "file.txt"), "text content");
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "file.json"), "json content");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loader.LoadAsync(_tempDirectory));

        Assert.Contains("No Markdown files", exception.Message);
    }

    [Fact]
    public async Task LoadAsync_WithCancellationToken_RespectsCancellation()
    {
        // Arrange
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "test.md"), "content");
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _loader.LoadAsync(_tempDirectory, cts.Token));
    }

    [Fact]
    public async Task LoadAsync_WithLargeFile_LoadsSuccessfully()
    {
        // Arrange
        var largeContent = string.Concat(Enumerable.Repeat("Line of content\n", 1000));
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "large.md"), largeContent);

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Line of content", result.Content);
    }

    [Fact]
    public async Task LoadAsync_IgnoresFilesInSubdirectories()
    {
        // Arrange
        var subDir = Path.Combine(_tempDirectory, "subdir");
        Directory.CreateDirectory(subDir);

        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "root.md"), "Root content");
        await File.WriteAllTextAsync(Path.Combine(subDir, "sub.md"), "Sub content");

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        // Should only load root.md, not sub.md
        Assert.Contains("Root content", result.Content);
        Assert.DoesNotContain("Sub content", result.Content);
    }

    [Fact]
    public async Task LoadAsync_WithEmptyMarkdownFile_IncludesEmptyBoundaries()
    {
        // Arrange
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "empty.md"), "");

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.Contains("--- BEGIN SOURCE: empty.md ---", result.Content);
        Assert.Contains("--- END SOURCE: empty.md ---", result.Content);
    }

    [Fact]
    public async Task LoadAsync_WithSpecialCharactersInFileName_HandlesCorrectly()
    {
        // Arrange
        const string fileName = "test-file_v2.md";
        const string content = "Test content";
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, fileName), content);

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.Contains($"--- BEGIN SOURCE: {fileName} ---", result.Content);
        Assert.Contains(content, result.Content);
    }

    [Fact]
    public async Task LoadAsync_ReturnsStoryInputRecord()
    {
        // Arrange
        await File.WriteAllTextAsync(Path.Combine(_tempDirectory, "test.md"), "content");

        // Act
        var result = await _loader.LoadAsync(_tempDirectory);

        // Assert
        Assert.IsType<StoryInput>(result);
        Assert.NotNull(result.Content);
        Assert.IsType<string>(result.Content);
    }
}
