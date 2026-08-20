using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console.Tests;

public sealed class ConsoleApplicationTests
{
    /// <summary>
    ///     Test: Valid input executes the workflow and displays results.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithValidInput_ExecutesWorkflowAndDisplaysResults()
    {
        // Arrange
        var input = new[] { "artificial intelligence", "/exit" };
        var console = new FakeConsole(input);
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(agent.WasCalled);
        Assert.Equal("artificial intelligence", agent.LastInputContent);
        Assert.Contains("Ideas", console.GetOutput());
        Assert.Contains("Test Idea", console.GetOutput());
    }

    /// <summary>
    ///     Test: Multiple inputs are processed independently.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithMultipleInputs_ExecutesWorkflowForEach()
    {
        // Arrange
        var input = new[] { "topic one", "topic two", "/exit" };
        var console = new FakeConsole(input);
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(2, agent.ExecutionCount);
        Assert.Equal(2, agent.AllInputs.Count);
        Assert.Equal("topic one", agent.AllInputs[0]);
        Assert.Equal("topic two", agent.AllInputs[1]);
    }

    /// <summary>
    ///     Test: Empty input does not invoke the workflow.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithEmptyInput_DoesNotExecuteWorkflow()
    {
        // Arrange
        var input = new[] { "", "   ", "/exit" };
        var console = new FakeConsole(input);
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
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
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
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
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
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
        var agent = new TrackingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
    }

    /// <summary>
    ///     Test: Workflow failures do not crash the application.
    /// </summary>
    [Fact]
    public async Task RunAsync_WhenWorkflowFails_ContinuesExecution()
    {
        // Arrange
        var input = new[] { "topic", "/exit" };
        var console = new FakeConsole(input);
        var agent = new FailingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

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
        var agent = new CustomFakeAgent([
            new Idea
            {
                Title = "First idea",
                Description = "First description",
                Audience = "Test",
                Rationale = "Test"
            },
            new Idea
            {
                Title = "Second idea",
                Description = "Second description",
                Audience = "Test",
                Rationale = "Test"
            }
        ]);
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

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
        var agent = new CustomFakeAgent(Array.Empty<Idea>());
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        var output = console.GetOutput();
        Assert.Equal(0, exitCode);
        Assert.Contains("No ideas were generated", output);
    }

    /// <summary>
    ///     Test: Cancellation token is passed through to the workflow.
    /// </summary>
    [Fact]
    public async Task RunAsync_PassesCancellationTokenToWorkflow()
    {
        // Arrange
        var input = new[] { "test topic", "/exit" };
        var console = new FakeConsole(input);
        var agent = new CancellationObservingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new StoryWorkflow(executor);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);
        var cts = new CancellationTokenSource();

        // Act
        var exitCode = await app.RunAsync(cts.Token);

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(agent.ReceivedCancellationToken);
    }

    /// <summary>
    ///     Test: ConsoleApplication works against IStoryWorkflow abstraction without MAF or real implementations.
    ///     Proves the boundary between ConsoleApplication and workflow is correctly abstracted.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithFakeStoryWorkflow_ExecutesWorkflowAndDisplaysResults()
    {
        // Arrange
        var expectedIdea = new Idea
        {
            Title = "Fake Workflow Idea",
            Description = "Generated by fake workflow",
            Audience = "Test Audience",
            Rationale = "To verify abstraction boundary"
        };
        var userInput = "abstraction test topic";
        var input = new[] { userInput, "/exit" };
        var console = new FakeConsole(input);
        var workflow = new FakeStoryWorkflow(expectedIdea);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Equal(userInput, workflow.CapturedInput?.Content);
        Assert.Contains(expectedIdea.Title, console.GetOutput());
        Assert.Contains(expectedIdea.Description, console.GetOutput());
    }
}

/// <summary>
///     Fake console implementation that accepts pre-determined input
///     and captures output for testing assertions.
/// </summary>
internal sealed class FakeConsole(IEnumerable<string> inputLines) : IConsole
{
    private readonly Queue<string?> _inputQueue = new(inputLines);
    private readonly List<string> _output = [];

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
///     Minimal fake implementation of IStoryWorkflow for testing the abstraction boundary.
///     Does not use MAF, StoryWorkflow, IdeaExecutorFactory, or IdeaAgent.
/// </summary>
internal sealed class FakeStoryWorkflow(Idea ideaToReturn) : IStoryWorkflow
{
    public ChatInput? CapturedInput { get; private set; }

    public Task<IReadOnlyList<Idea>> RunAsync(
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        CapturedInput = input;
        var ideas = new List<Idea> { ideaToReturn }.AsReadOnly();
        return Task.FromResult<IReadOnlyList<Idea>>(ideas);
    }
}

/// <summary>
///     Tracking fake agent that counts executions and records inputs.
/// </summary>
internal sealed class TrackingFakeAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
{
    public int ExecutionCount { get; private set; }
    public string? LastInputContent { get; private set; }
    public List<string> AllInputs { get; } = new();
    public bool WasCalled => ExecutionCount > 0;

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        FlowExecutionContext executionContext,
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        ExecutionCount++;
        LastInputContent = input.Content;
        AllInputs.Add(input.Content);

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

        return Task.FromResult<IReadOnlyList<Idea>>(ideas.AsReadOnly());
    }
}

/// <summary>
///     Fake agent that always throws an exception.
/// </summary>
internal sealed class FailingFakeAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
{
    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        FlowExecutionContext executionContext,
        ChatInput input,
        CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException("Simulated agent failure for testing.");
}

/// <summary>
///     Custom fake agent that returns specified ideas.
/// </summary>
internal sealed class CustomFakeAgent(IEnumerable<Idea> ideas) : IAgent<ChatInput, IReadOnlyList<Idea>>
{
    private readonly IReadOnlyList<Idea> _ideas = ideas.ToList().AsReadOnly();

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        FlowExecutionContext executionContext,
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);
        return Task.FromResult(_ideas);
    }
}

/// <summary>
///     Fake agent that observes and records the cancellation token.
/// </summary>
internal sealed class CancellationObservingFakeAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
{
    public bool ReceivedCancellationToken { get; private set; }

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        FlowExecutionContext executionContext,
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        ReceivedCancellationToken = !cancellationToken.Equals(CancellationToken.None);

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

        return Task.FromResult<IReadOnlyList<Idea>>(ideas.AsReadOnly());
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