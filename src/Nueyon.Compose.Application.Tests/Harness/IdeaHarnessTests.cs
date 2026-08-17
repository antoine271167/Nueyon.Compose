using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Harness;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Harness;

public sealed class IdeaHarnessTests
{
    /// <summary>
    /// Test 1: Valid result on first attempt.
    /// Verifies that when the agent returns valid ideas and the validator accepts them,
    /// the harness returns immediately without retrying.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithValidResultOnFirstAttempt_ReturnsResult()
    {
        // Arrange
        var validIdea = new Idea
        {
            Title = "Test Title",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var validIdeas = new[] { validIdea };

        var agent = new FakeAgent(validIdeas);
        var validator = new AlwaysTrueValidator();
        var harness = new IdeaHarness(agent, validator);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await harness.ExecuteAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(validIdea.Title, result[0].Title);
        Assert.Equal(1, agent.ExecuteCallCount);
        Assert.Equal(1, validator.IsValidCallCount);
    }

    /// <summary>
    /// Test 2: Invalid result causes retry.
    /// Verifies that when the first result is invalid and the second is valid,
    /// the harness returns the second result after exactly two calls.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithInvalidFirstResult_RetriesAndReturnsValidSecond()
    {
        // Arrange
        var validIdea = new Idea
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Audience = "Valid Audience",
            Rationale = "Valid Rationale"
        };
        var validIdeas = new[] { validIdea };

        var agent = new SequenceAgent(new[] { Array.Empty<Idea>(), validIdeas });
        var validator = new SequenceValidator(new[] { false, true });
        var harness = new IdeaHarness(agent, validator);

        var input = new ChatInput { Content = "Test input" };

        // Act
        var result = await harness.ExecuteAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(validIdea.Title, result[0].Title);
        Assert.Equal(2, agent.ExecuteCallCount);
        Assert.Equal(2, validator.IsValidCallCount);
    }

    /// <summary>
    /// Test 3: Three invalid attempts fail with InvalidOperationException.
    /// Verifies that when all three attempts produce invalid results,
    /// the harness throws InvalidOperationException and never makes a fourth attempt.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithThreeInvalidAttempts_ThrowsInvalidOperationException()
    {
        // Arrange
        var agent = new FakeAgent(Array.Empty<Idea>());
        var validator = new AlwaysFalseValidator();
        var harness = new IdeaHarness(agent, validator);

        var input = new ChatInput { Content = "Test input" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => harness.ExecuteAsync(input));

        Assert.Equal("Idea agent failed to produce valid ideas after 3 attempts.", exception.Message);
        Assert.Equal(3, agent.ExecuteCallCount);
        Assert.Equal(3, validator.IsValidCallCount);
    }

    /// <summary>
    /// Test 4: Cancellation token is forwarded to the agent.
    /// Verifies that the exact cancellation token passed to the harness
    /// is forwarded to the agent without modification.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_ForwardsCancellationTokenToAgent()
    {
        // Arrange
        var validIdea = new Idea
        {
            Title = "Test Title",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var validIdeas = new[] { validIdea };

        var agent = new TokenCapturingAgent(validIdeas);
        var validator = new AlwaysTrueValidator();
        var harness = new IdeaHarness(agent, validator);

        var input = new ChatInput { Content = "Test input" };
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        // Act
        await harness.ExecuteAsync(input, token);

        // Assert
        Assert.Equal(token, agent.CapturedToken);
    }

    /// <summary>
    /// Test 5: Null agent in constructor throws ArgumentNullException.
    /// </summary>
    [Fact]
    public void Constructor_WithNullAgent_ThrowsArgumentNullException()
    {
        // Arrange
        var validator = new AlwaysTrueValidator();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new IdeaHarness(null!, validator));

        Assert.Equal("agent", exception.ParamName);
    }

    /// <summary>
    /// Test 6: Null validator in constructor throws ArgumentNullException.
    /// </summary>
    [Fact]
    public void Constructor_WithNullValidator_ThrowsArgumentNullException()
    {
        // Arrange
        var agent = new FakeAgent(Array.Empty<Idea>());

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new IdeaHarness(agent, null!));

        Assert.Equal("validator", exception.ParamName);
    }

    /// <summary>
    /// Test 7: Null input to ExecuteAsync throws ArgumentNullException.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithNullInput_ThrowsArgumentNullException()
    {
        // Arrange
        var agent = new FakeAgent(Array.Empty<Idea>());
        var validator = new AlwaysTrueValidator();
        var harness = new IdeaHarness(agent, validator);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => harness.ExecuteAsync(null!));

        Assert.Equal("input", exception.ParamName);
    }

    /// <summary>
    /// Test 8: Cancellation propagates through the harness.
    /// Verifies that when the agent is cancelled, the exception propagates.
    /// </summary>
    [Fact]
    public async Task ExecuteAsync_WithCancellation_PropagatesOperationCanceledException()
    {
        // Arrange
        var agent = new CancellingAgent();
        var validator = new AlwaysTrueValidator();
        var harness = new IdeaHarness(agent, validator);

        var input = new ChatInput { Content = "Test input" };
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => harness.ExecuteAsync(input, cts.Token));
    }

    #region Test Doubles

    /// <summary>
    /// A fake agent that returns a predefined list of ideas.
    /// </summary>
    private sealed class FakeAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea> _ideas;

        public int ExecuteCallCount { get; private set; }

        public FakeAgent(IReadOnlyList<Idea> ideas)
        {
            _ideas = ideas;
        }

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            ExecuteCallCount++;
            return Task.FromResult(_ideas);
        }
    }

    /// <summary>
    /// A fake agent that returns different ideas on each call from a sequence.
    /// </summary>
    private sealed class SequenceAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea>[] _sequence;
        private int _index;

        public int ExecuteCallCount { get; private set; }

        public SequenceAgent(IReadOnlyList<Idea>[] sequence)
        {
            _sequence = sequence;
        }

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            ExecuteCallCount++;
            var result = _sequence[_index++];
            return Task.FromResult(result);
        }
    }

    /// <summary>
    /// A fake agent that captures the cancellation token passed to it.
    /// </summary>
    private sealed class TokenCapturingAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        private readonly IReadOnlyList<Idea> _ideas;

        public CancellationToken CapturedToken { get; private set; }

        public TokenCapturingAgent(IReadOnlyList<Idea> ideas)
        {
            _ideas = ideas;
        }

        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            CapturedToken = cancellationToken;
            return Task.FromResult(_ideas);
        }
    }

    /// <summary>
    /// A fake agent that throws OperationCanceledException.
    /// </summary>
    private sealed class CancellingAgent : IAgent<ChatInput, IReadOnlyList<Idea>>
    {
        public Task<IReadOnlyList<Idea>> ExecuteAsync(
            ChatInput input,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new OperationCanceledException();
        }
    }

    /// <summary>
    /// A fake validator that always returns true.
    /// </summary>
    private sealed class AlwaysTrueValidator : IIdeaValidator
    {
        public int IsValidCallCount { get; private set; }

        public bool IsValid(IReadOnlyList<Idea> ideas)
        {
            IsValidCallCount++;
            return true;
        }
    }

    /// <summary>
    /// A fake validator that always returns false.
    /// </summary>
    private sealed class AlwaysFalseValidator : IIdeaValidator
    {
        public int IsValidCallCount { get; private set; }

        public bool IsValid(IReadOnlyList<Idea> ideas)
        {
            IsValidCallCount++;
            return false;
        }
    }

    /// <summary>
    /// A fake validator that returns different values from a sequence.
    /// </summary>
    private sealed class SequenceValidator : IIdeaValidator
    {
        private readonly bool[] _sequence;
        private int _index;

        public int IsValidCallCount { get; private set; }

        public SequenceValidator(bool[] sequence)
        {
            _sequence = sequence;
        }

        public bool IsValid(IReadOnlyList<Idea> ideas)
        {
            IsValidCallCount++;
            return _sequence[_index++];
        }
    }

    #endregion
}
