using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Workflows;

public sealed class IdeaWorkflowTests
{
    /// <summary>
    ///     Test 1: Workflow builds successfully and contains exactly one executor ("idea").
    ///     Uses the public ReflectExecutors() API to verify the workflow topology.
    /// </summary>
    [Fact]
    public void Build_WithValidExecutor_CompletesSuccessfully()
    {
        // Arrange
        var agent = new CapturingFakeAgent();
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new IdeaWorkflow(executor);

        // Act
        var builtWorkflow = workflow.Build();

        // Assert
        Assert.NotNull(builtWorkflow);
        var executors = builtWorkflow.ReflectExecutors();
        Assert.Single(executors);
        Assert.True(executors.ContainsKey("idea"));
    }

    /// <summary>
    ///     Test 2: Workflow executes successfully without error.
    ///     Given a ChatInput and a fake agent returning one known Idea,
    ///     verifies that the workflow executes to completion.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithValidInput_ExecutesSuccessfully()
    {
        // Arrange
        var expectedIdea = new Idea
        {
            Title = "Test Idea",
            Description = "A test idea for verification",
            Audience = "Test Audience",
            Rationale = "To verify workflow execution"
        };

        var agent = new CapturingFakeAgent(expectedIdea);
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new IdeaWorkflow(executor);

        var input = new ChatInput { Content = "Test input" };

        // Act - should not throw
        var result = await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(result);
        var idea = Assert.Single(result);
        Assert.Equal(expectedIdea.Title, idea.Title);
        Assert.Equal(expectedIdea.Description, idea.Description);
        Assert.Equal(expectedIdea.Audience, idea.Audience);
        Assert.Equal(expectedIdea.Rationale, idea.Rationale);
    }

    /// <summary>
    ///     Test 3: Workflow correctly passes the input to the executor and agent.
    ///     Uses a capturing fake agent to verify that the ChatInput is correctly
    ///     transmitted through the executor to the agent.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithValidInput_PassesInputToAgent()
    {
        // Arrange
        var agent = new CapturingFakeAgent(new Idea
        {
            Title = "Captured",
            Description = "d",
            Audience = "a",
            Rationale = "r"
        });
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new IdeaWorkflow(executor);

        const string expectedContent = "This is the user's input";
        var input = new ChatInput { Content = expectedContent };

        // Act
        await workflow.RunAsync(input);

        // Assert
        Assert.NotNull(agent.CapturedInput);
        Assert.Equal(expectedContent, agent.CapturedInput.Content);
    }

    /// <summary>
    ///     Test 4: The cancellation token is structurally propagated through the workflow to the agent.
    ///     MAF wraps the caller's token in an internal linked token rather than passing it unchanged.
    ///     This test verifies that the agent receives a valid (non-default) cancellation token,
    ///     proving the plumbing exists from RunAsync through the executor to the agent.
    /// </summary>
    [Fact]
    public async Task RunAsync_WithCancellationToken_PropagatesTokenToAgent()
    {
        // Arrange
        var agent = new CapturingFakeAgent(new Idea
        {
            Title = "Captured",
            Description = "d",
            Audience = "a",
            Rationale = "r"
        });
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new IdeaWorkflow(executor);

        var input = new ChatInput { Content = "Test input" };
        using var cts = new CancellationTokenSource();

        // Act
        await workflow.RunAsync(input, cts.Token);

        // Assert: MAF provides its own linked token to the handler; the agent must receive it
        Assert.NotNull(agent.CapturedCancellationToken);
        // The captured token is MAF's internal linked token (not the exact same instance),
        // but it must be a valid, non-default, non-cancelled token
        Assert.NotEqual(CancellationToken.None, agent.CapturedCancellationToken.Value);
        Assert.False(agent.CapturedCancellationToken.Value.IsCancellationRequested);
    }

    /// <summary>
    ///     Test 5: An agent failure surfaces as an InvalidOperationException at the workflow boundary.
    ///     MAF swallows executor-level exceptions internally; the observable failure is that
    ///     the workflow produces no output, which ExtractResult turns into an InvalidOperationException.
    /// </summary>
    [Fact]
    public async Task RunAsync_WhenAgentFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var agent = new FailingFakeAgent(new InvalidOperationException("Test agent failure"));
        var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
        var workflow = new IdeaWorkflow(executor);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => workflow.RunAsync(input));
    }

    /// <summary>
    ///     A fake agent that captures the input and cancellation token for verification.
    /// </summary>
    private sealed class CapturingFakeAgent(Idea? ideaToReturn = null) : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea>? _ideaToReturn = ideaToReturn is not null
            ? new List<Idea> { ideaToReturn }.AsReadOnly()
            : [];

        public ChatInput? CapturedInput { get; private set; }

        public CancellationToken? CapturedCancellationToken { get; private set; }

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            FlowExecutionContext executionContext,
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);
            ArgumentNullException.ThrowIfNull(input);

            CapturedInput = input;
            CapturedCancellationToken = cancellationToken;

            return Task.FromResult(_ideaToReturn!);
        }
    }

    /// <summary>
    ///     A fake agent that always throws the specified exception.
    /// </summary>
    private sealed class FailingFakeAgent(Exception exceptionToThrow) : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly Exception _exceptionToThrow =
            exceptionToThrow ?? throw new ArgumentNullException(nameof(exceptionToThrow));

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            FlowExecutionContext executionContext,
            ChatInput input,
            CancellationToken cancellationToken = default) =>
            Task.FromException<IReadOnlyList<Idea>>(_exceptionToThrow);
    }
}