using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console.Tests;

public sealed class ConsoleApplicationTests
{
    /// <summary>
    ///     Test: Valid input executes the flow and displays results.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithValidInput_ExecutesFlowAndDisplaysResults()
    {
        // Arrange
        var input = new[] { "artificial intelligence", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(1, flowEngine.ExecutionCount);
        Assert.Equal("artificial intelligence", flowEngine.LastInputContent);
        Assert.Contains("Ideas", console.GetOutput());
        Assert.Contains("Example Idea 1", console.GetOutput());
    }

    /// <summary>
    ///     Test: Multiple inputs are processed independently.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithMultipleInputs_ExecutesFlowForEach()
    {
        // Arrange
        var input = new[] { "topic one", "topic two", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(2, flowEngine.ExecutionCount);
        Assert.Equal(2, flowEngine.AllInputs.Count);
        Assert.Equal("topic one", flowEngine.AllInputs[0]);
        Assert.Equal("topic two", flowEngine.AllInputs[1]);
    }

    /// <summary>
    ///     Test: Empty input does not invoke the flow engine.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithEmptyInput_DoesNotExecuteFlow()
    {
        // Arrange
        var input = new[] { "", "   ", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, flowEngine.ExecutionCount);
        Assert.Contains("Please enter an idea or topic", console.GetOutput());
    }

    /// <summary>
    ///     Test: /exit command terminates the application.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithExitCommand_TerminatesCleanly()
    {
        // Arrange
        var input = new[] { "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, flowEngine.ExecutionCount);
        Assert.Contains("Goodbye", console.GetOutput());
    }

    /// <summary>
    ///     Test: /exit is case-insensitive.
    /// </summary>
    [Theory]
    [InlineData("/EXIT")]
    [InlineData("/Exit")]
    [InlineData("/eXiT")]
    public async Task RunAsync_WithVariousCasesOfExit_Terminates(string exitCommand)
    {
        // Arrange
        var input = new[] { exitCommand };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, flowEngine.ExecutionCount);
        Assert.Contains("Goodbye", console.GetOutput());
    }

    /// <summary>
    ///     Test: Whitespace around /exit is trimmed and recognized.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithWhitespaceAroundExit_Terminates()
    {
        // Arrange
        var input = new[] { "   /exit   " };
        var console = new FakeConsole(input);
        var flowEngine = new TrackingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, flowEngine.ExecutionCount);
    }

    /// <summary>
    ///     Test: Flow engine failures do not crash the application.
    /// </summary>
    [Fact]
    public async Task RunAsync_WhenFlowEngineFails_ContinuesExecution()
    {
        // Arrange
        var input = new[] { "topic", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new FailingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Contains("Unable to process the request", console.GetOutput());
        Assert.Contains("Goodbye", console.GetOutput());
    }

    /// <summary>
    ///     Test: Result display shows ideas correctly.
    /// </summary>
    [Fact]
    public async Task RunAsync_DisplaysResultsCorrectly()
    {
        // Arrange
        var input = new[] { "test topic", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new CustomFlowEngine(
            new[]
            {
                new Idea
                {
                    Title = "First idea", Description = "First description", Audience = "Test", Rationale = "Test"
                },
                new Idea
                {
                    Title = "Second idea", Description = "Second description", Audience = "Test", Rationale = "Test"
                }
            });
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        var output = console.GetOutput();
        Assert.Equal(0, exitCode);
        Assert.Contains("1. First idea", output);
        Assert.Contains("First description", output);
        Assert.Contains("2. Second idea", output);
        Assert.Contains("Second description", output);
    }

    /// <summary>
    ///     Test: Empty results are handled correctly.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithEmptyResults_DisplaysNoIdeasMessage()
    {
        // Arrange
        var input = new[] { "test topic", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new CustomFlowEngine(Array.Empty<Idea>());
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        var output = console.GetOutput();
        Assert.Equal(0, exitCode);
        Assert.Contains("No ideas were generated", output);
    }

    /// <summary>
    ///     Test: Cancellation token is passed through to the flow engine.
    /// </summary>
    [Fact]
    public async Task RunAsync_PassesCancellationTokenToFlowEngine()
    {
        // Arrange
        var input = new[] { "test topic", "/exit" };
        var console = new FakeConsole(input);
        var flowEngine = new CancellationObservingFlowEngine();
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(flowEngine, logger, console);
        var cts = new CancellationTokenSource();

        // Act
        var exitCode = await app.RunAsync(cts.Token);

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(flowEngine.ReceivedCancellationToken);
    }
}

/// <summary>
///     Fake console implementation that accepts pre-determined input
///     and captures output for testing assertions.
/// </summary>
internal sealed class FakeConsole : IConsole
{
    public FakeConsole(IEnumerable<string> inputLines) => _inputQueue = new Queue<string?>(inputLines);

    private readonly Queue<string?> _inputQueue;
    private readonly List<string> _output = new();

    public string? ReadLine()
    {
        if (_inputQueue.Count == 0)
        {
            return null;
        }

        var line = _inputQueue.Dequeue();
        _output.Add($"[INPUT] {line}");
        return line;
    }

    public void Write(string value)
    {
        _output.Add(value);
    }

    public void WriteLine(string value)
    {
        _output.Add(value);
    }

    public string GetOutput() => string.Join("\n", _output);
}

/// <summary>
///     Mock flow engine that tracks execution count and input content.
/// </summary>
internal sealed class TrackingFlowEngine : IFlowEngine
{
    public int ExecutionCount { get; private set; }
    public string? LastInputContent { get; private set; }
    public List<string> AllInputs { get; } = new();

    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        ExecutionCount++;
        LastInputContent = workspace.Input.Content;
        AllInputs.Add(workspace.Input.Content);

        var ideas = new List<Idea>
        {
            new()
            {
                Title = "Example Idea 1",
                Description = "First example idea",
                Audience = "Test Audience",
                Rationale = "For testing purposes"
            },
            new()
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
///     Mock flow engine that always throws an exception.
/// </summary>
internal sealed class FailingFlowEngine : IFlowEngine
{
    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException("Simulated flow engine failure for testing.");
}

/// <summary>
///     Custom flow engine that returns specified ideas.
/// </summary>
internal sealed class CustomFlowEngine : IFlowEngine
{
    public CustomFlowEngine(IEnumerable<Idea> ideas) => _ideas = ideas.ToList().AsReadOnly();

    private readonly IReadOnlyList<Idea> _ideas;

    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        workspace.Ideas = _ideas;
        return Task.FromResult(workspace);
    }
}

/// <summary>
///     Flow engine that observes and records the cancellation token.
/// </summary>
internal sealed class CancellationObservingFlowEngine : IFlowEngine
{
    public bool ReceivedCancellationToken { get; private set; }

    public Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        ReceivedCancellationToken = !cancellationToken.Equals(default);

        var ideas = new List<Idea>
        {
            new()
            {
                Title = "Test Idea",
                Description = "Test description",
                Audience = "Test Audience",
                Rationale = "For testing purposes"
            }
        };

        workspace.Ideas = ideas.AsReadOnly();
        return Task.FromResult(workspace);
    }
}

/// <summary>
///     Mock logger that captures log messages without outputting to console.
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