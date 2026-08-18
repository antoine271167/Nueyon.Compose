using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Host.Console;

namespace Nueyon.Compose.Host.Console.Tests;

public sealed class ConsoleApplicationTests
{
    /// <summary>
    /// Test 1: Console application can be instantiated with valid dependencies.
    /// </summary>
    [Fact]
    public void Constructor_WithValidDependencies_Succeeds()
    {
        // Arrange
        var mockLogger = new MockLogger<ConsoleApplication>();
        var mockFlowEngine = new MockFlowEngine();

        // Act
        var consoleApp = new ConsoleApplication(mockFlowEngine, mockLogger);

        // Assert
        Assert.NotNull(consoleApp);
    }

    /// <summary>
    /// Test 2: Flow engine receives the user input as ChatInput with correct content.
    /// </summary>
    [Fact]
    public async Task MockFlowEngine_ExecuteAsync_ReturnsWorkspaceWithIdeas()
    {
        // Arrange
        var mockFlowEngine = new MockFlowEngine();
        var userInput = "artificial intelligence";
        var chatInput = new ChatInput { Content = userInput };
        var workspace = new StoryWorkspace { Input = chatInput };

        // Act
        var result = await mockFlowEngine.ExecuteAsync(workspace);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userInput, result.Input.Content);
        Assert.NotEmpty(result.Ideas);
        Assert.Equal(2, result.Ideas.Count);
    }

    /// <summary>
    /// Test 3: Mock flow engine returns ideas with correct structure.
    /// </summary>
    [Fact]
    public async Task MockFlowEngine_ReturnsIdeasWithRequiredFields()
    {
        // Arrange
        var mockFlowEngine = new MockFlowEngine();
        var chatInput = new ChatInput { Content = "test topic" };
        var workspace = new StoryWorkspace { Input = chatInput };

        // Act
        var result = await mockFlowEngine.ExecuteAsync(workspace);

        // Assert
        Assert.NotNull(result.Ideas);
        foreach (var idea in result.Ideas)
        {
            Assert.NotNull(idea.Title);
            Assert.NotEmpty(idea.Title);
            Assert.NotNull(idea.Description);
            Assert.NotEmpty(idea.Description);
            Assert.NotNull(idea.Audience);
            Assert.NotEmpty(idea.Audience);
            Assert.NotNull(idea.Rationale);
            Assert.NotEmpty(idea.Rationale);
        }
    }

    /// <summary>
    /// Test 4: Failing flow engine throws the expected exception.
    /// </summary>
    [Fact]
    public async Task FailingFlowEngine_ExecuteAsync_ThrowsInvalidOperationException()
    {
        // Arrange
        var failingFlowEngine = new FailingFlowEngine();
        var chatInput = new ChatInput { Content = "test" };
        var workspace = new StoryWorkspace { Input = chatInput };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => failingFlowEngine.ExecuteAsync(workspace));
    }

    /// <summary>
    /// Test 5: Mock logger can be instantiated and used.
    /// </summary>
    [Fact]
    public void MockLogger_LogsWithoutException()
    {
        // Arrange
        var logger = new MockLogger<ConsoleApplication>();

        // Act & Assert - should not throw
        logger.Log(LogLevel.Information, default(EventId), "Test message", null, (s, e) => s.ToString() ?? string.Empty);
        Assert.True(logger.IsEnabled(LogLevel.Information));
    }

    /// <summary>
    /// Test 6: Console application can be constructed with mock dependencies.
    /// </summary>
    [Fact]
    public void ConsoleApplication_WithMockFlowEngine_CanBeCreated()
    {
        // Arrange
        var logger = new MockLogger<ConsoleApplication>();
        var flowEngine = new MockFlowEngine();

        // Act
        var app = new ConsoleApplication(flowEngine, logger);

        // Assert
        Assert.NotNull(app);
    }

    /// <summary>
    /// Test 7: Multiple flow executions return different result instances.
    /// </summary>
    [Fact]
    public async Task MockFlowEngine_MultipleExecutions_ReturnDifferentInstances()
    {
        // Arrange
        var flowEngine = new MockFlowEngine();
        var chatInput1 = new ChatInput { Content = "topic 1" };
        var chatInput2 = new ChatInput { Content = "topic 2" };
        var workspace1 = new StoryWorkspace { Input = chatInput1 };
        var workspace2 = new StoryWorkspace { Input = chatInput2 };

        // Act
        var result1 = await flowEngine.ExecuteAsync(workspace1);
        var result2 = await flowEngine.ExecuteAsync(workspace2);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotSame(result1, result2);
        Assert.NotEqual(result1.Input.Content, result2.Input.Content);
    }
}

/// <summary>
/// Mock logger that captures log messages without outputting to console.
/// </summary>
internal sealed class MockLogger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        // Do nothing - mock implementation
    }
}

/// <summary>
/// Mock flow engine that returns a workspace with example ideas.
/// </summary>
internal sealed class MockFlowEngine : IFlowEngine
{
    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        // Return the workspace with example ideas
        var ideas = new List<Idea>
        {
            new Idea
            {
                Title = "Example Idea 1",
                Description = "First example idea",
                Audience = "Test Audience",
                Rationale = "For testing purposes"
            },
            new Idea
            {
                Title = "Example Idea 2",
                Description = "Second example idea",
                Audience = "Test Audience",
                Rationale = "For testing purposes"
            }
        };

        workspace.Ideas = ideas.AsReadOnly();
        return Task.FromResult(workspace);
    }
}

/// <summary>
/// Mock flow engine that always throws an exception.
/// </summary>
internal sealed class FailingFlowEngine : IFlowEngine
{
    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Simulated flow engine failure for testing.");
    }
}
