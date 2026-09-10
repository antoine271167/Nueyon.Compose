using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Services;
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
        const string testContent = "artificial intelligence";
        var console = new FakeConsole(Array.Empty<string>());
        var loader = new FakeSourceContextLoader(new StoryInput(testContent));
        var agent = new TrackingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(agent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(agent.WasCalled);
        Assert.Equal(testContent, agent.LastInputContent);
        Assert.Contains("Selected Idea", console.GetOutput());
        Assert.Contains("Test Idea", console.GetOutput());
    }

    /// <summary>
    ///     Test: Application handles missing directory gracefully.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithMissingDirectory_ReturnsErrorExitCode()
    {
        // Arrange
        var console = new FakeConsole(Array.Empty<string>());
        var loader = new FakeSourceContextLoader(null, new DirectoryNotFoundException("Directory not found"));
        var agent = new TrackingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(agent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(1, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
        Assert.Contains("Error", console.GetOutput());
    }

    /// <summary>
    ///     Test: Application handles no Markdown files gracefully.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithNoMarkdownFiles_ReturnsErrorExitCode()
    {
        // Arrange
        var console = new FakeConsole(Array.Empty<string>());
        var loader = new FakeSourceContextLoader(
            null,
            new InvalidOperationException("No Markdown files found"));
        var agent = new TrackingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(agent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(1, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
        Assert.Contains("Error", console.GetOutput());
    }

    /// <summary>
    ///     Test: Application cancellation returns proper exit code.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithCancellation_ReturnsCancellationExitCode()
    {
        // Arrange
        var console = new FakeConsole(Array.Empty<string>());
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var loader = new FakeCancellableSourceContextLoader(cts.Token);
        var agent = new TrackingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(agent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync(cts.Token);

        // Assert
        Assert.Equal(130, exitCode);
        Assert.Equal(0, agent.ExecutionCount);
    }

    /// <summary>
    ///     Test: Workflow results are displayed correctly.
    /// </summary>
    [Fact]
    public async Task RunAsync_DisplaysAllWorkflowResults()
    {
        // Arrange
        const string testContent = "test input";
        var console = new FakeConsole(Array.Empty<string>());
        var loader = new FakeSourceContextLoader(new StoryInput(testContent));
        var agent = new TrackingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(agent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(0, exitCode);
        var output = console.GetOutput();
        Assert.Contains("Selected Idea", output);
        Assert.Contains("Research", output);
        Assert.Contains("Synthesis", output);
        Assert.Contains("Narrative", output);
        Assert.Contains("Compose", output);
    }

    [Fact]
    public async Task RunAsync_WithWorkflowFailure_ReturnsErrorExitCode()
    {
        // Arrange
        var console = new FakeConsole(Array.Empty<string>());
        var loader = new FakeSourceContextLoader(new StoryInput("test"));
        var failingAgent = new FailingFakeAgent();
        var researchAgent = CreateResearchAgent();
        var synthesizer = new FakeSynthesizerAgent(new SynthesisResult("synthesis content"));
        var narrativeAgent = new FakeNarrativeAgent(new NarrativeResult("narrative content"));
        var composeAgent = new FakeComposeAgent(new ComposeResult("complete article content"));
        var workflow = new StoryWorkflow(failingAgent, researchAgent, synthesizer, narrativeAgent, composeAgent);
        var logger = new MockLogger<ConsoleApplication>();
        var app = new ConsoleApplication(loader, workflow, logger, console);

        // Act
        var exitCode = await app.RunAsync();

        // Assert
        Assert.Equal(1, exitCode);
    }

    /// <summary>
    ///     Helper method to create a fake research agent.
    /// </summary>
    private static FakeResearchAgent CreateResearchAgent()
    {
        return new FakeResearchAgent(new ResearchResult("Test research result"));
    }
}

/// <summary>
///     Fake console for testing that buffers output.
/// </summary>
internal sealed class FakeConsole(string[] inputLines) : IConsole
{
    private int _inputIndex;
    private readonly List<string> _output = new();

    public void Write(string? value)
    {
        _output.Add(value ?? string.Empty);
    }

    public void WriteLine(string? value = null)
    {
        _output.Add(value ?? string.Empty);
        _output.Add("\n");
    }

    public string? ReadLine()
    {
        return _inputIndex < inputLines.Length ? inputLines[_inputIndex++] : null;
    }

    public string GetOutput() => string.Concat(_output);
}

/// <summary>
///     Fake source context loader that returns predetermined content.
/// </summary>
internal sealed class FakeSourceContextLoader(StoryInput? content, Exception? exception = null) : ISourceContextLoader
{
    public Task<StoryInput> LoadAsync(
        string directory,
        CancellationToken cancellationToken = default)
    {
        if (exception is not null)
        {
            return Task.FromException<StoryInput>(exception);
        }

        return Task.FromResult(content ?? throw new InvalidOperationException("No content configured"));
    }
}

/// <summary>
///     Fake source context loader that throws on cancellation.
/// </summary>
internal sealed class FakeCancellableSourceContextLoader(CancellationToken cancellationToThrow) : ISourceContextLoader
{
    public Task<StoryInput> LoadAsync(
        string directory,
        CancellationToken cancellationToken = default)
    {
        return cancellationToThrow.IsCancellationRequested
            ? Task.FromException<StoryInput>(new OperationCanceledException())
            : Task.FromResult(new StoryInput("test"));
    }
}

/// <summary>
///     Tracking fake idea agent that captures invocation details.
/// </summary>
internal sealed class TrackingFakeAgent : IAgent<StoryInput, IReadOnlyList<Idea>>
{
    public int ExecutionCount { get; private set; }
    public string? LastInputContent { get; private set; }
    public List<string> AllInputs { get; } = new();
    public bool WasCalled => ExecutionCount > 0;

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        AgentExecutionContext executionContext,
        StoryInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        ExecutionCount++;
        LastInputContent = input.Content;
        AllInputs.Add(input.Content);

        var ideas = new List<Idea>
        {
            new(
                "Test Idea",
                "Test description",
                "Test Audience",
                "For testing purposes"
            )
        };

        return Task.FromResult<IReadOnlyList<Idea>>(ideas.AsReadOnly());
    }
}

/// <summary>
///     Fake agent that always throws an exception.
/// </summary>
internal sealed class FailingFakeAgent : IAgent<StoryInput, IReadOnlyList<Idea>>
{
    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        AgentExecutionContext executionContext,
        StoryInput input,
        CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException("Simulated agent failure for testing.");
}

/// <summary>
///     Custom fake agent that returns specified ideas.
/// </summary>
internal sealed class CustomFakeAgent(IEnumerable<Idea> ideas) : IAgent<StoryInput, IReadOnlyList<Idea>>
{
    private readonly IReadOnlyList<Idea> _ideas = ideas.ToList().AsReadOnly();

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        AgentExecutionContext executionContext,
        StoryInput input,
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
internal sealed class CancellationObservingFakeAgent : IAgent<StoryInput, IReadOnlyList<Idea>>
{
    public bool ReceivedCancellationToken { get; private set; }

    public Task<IReadOnlyList<Idea>> ExecuteAsync(
        AgentExecutionContext executionContext,
        StoryInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        ReceivedCancellationToken = !cancellationToken.Equals(CancellationToken.None);

        var ideas = new List<Idea>
        {
            new(
                "Test Idea",
                "Test description",
                "Test Audience",
                "For testing purposes"
            )
        };

        return Task.FromResult<IReadOnlyList<Idea>>(ideas.AsReadOnly());
    }
}

/// <summary>
///     Fake research agent that always returns a simple research result.
/// </summary>
internal sealed class FakeResearchAgent : IAgent<ResearchInput, ResearchResult>
{
    private readonly ResearchResult _result;

    public FakeResearchAgent(ResearchResult result)
    {
        _result = result;
    }

    public Task<ResearchResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        ResearchInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        return Task.FromResult(_result);
    }
}

internal sealed class FakeSynthesizerAgent(SynthesisResult result) : IAgent<SynthesisInput, SynthesisResult>
{
    public Task<SynthesisResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        SynthesisInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        return Task.FromResult(result);
    }
}

internal sealed class FakeNarrativeAgent(NarrativeResult result) : IAgent<NarrativeInput, NarrativeResult>
{
    public Task<NarrativeResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        NarrativeInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        return Task.FromResult(result);
    }
}

internal sealed class FakeComposeAgent(ComposeResult result) : IAgent<ComposeInput, ComposeResult>
{
    public Task<ComposeResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        ComposeInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        return Task.FromResult(result);
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
