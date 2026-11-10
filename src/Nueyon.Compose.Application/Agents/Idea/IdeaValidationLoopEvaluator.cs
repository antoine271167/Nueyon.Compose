using System.Text.Json;
using Microsoft.Agents.AI;
using Nueyon.Compose.Application.Validation;

#pragma warning disable MAAI001 // LoopAgent APIs are experimental.

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>
///     Validates each generation before allowing the Idea loop to return it.
/// </summary>
public sealed class IdeaValidationLoopEvaluator(IIdeaValidator validator) : LoopEvaluator
{
    // Retain structural-only validation for callers that use the evaluator outside production.

    public IdeaValidationLoopEvaluator(IIdeaValidator validator, IdeaSemanticJudge judge) : this(validator) =>
        _judge = judge ?? throw new ArgumentNullException(nameof(judge));

    public const int MaxAttempts = 3;

    internal const string SourceContentKey = "Nueyon.Compose.Idea.SourceContent";

    private readonly IdeaSemanticJudge? _judge;

    private readonly IIdeaValidator _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    private Action<IdeaValidationAttempt>? _observer;

    /// <summary>
    ///     Registers an optional observer for completed and incomplete validation attempts.
    /// </summary>
    public void Observe(Action<IdeaValidationAttempt> observer) =>
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));

    public LoopAgent Wrap(AIAgent generator) => new(generator, this, new LoopAgentOptions
    {
        // MAF returns at its cap before calling the evaluator. Leave room to judge attempt three.
        MaxIterations = MaxAttempts + 1,
        NonStreamingReturnsLastResponseOnly = true
    });

    public override async ValueTask<LoopEvaluation> EvaluateAsync(
        LoopContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();

        var responseText = context.LastResponse.Text;
        _observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "incomplete", null));
        IReadOnlyList<Domain.Idea> ideas;

        try
        {
            ideas = IdeaAgent.ParseIdeasFromJson(responseText);
        }
        catch (InvalidOperationException ex)
        {
            return Reject(context, responseText,
                "Return a valid JSON object with an ideas array matching the response schema.", ex);
        }

        if (ideas.Count == 0 || !_validator.IsValid([ideas[0]]))
        {
            return Reject(context, responseText,
                "Return at least one idea; the first idea needs a non-empty title, description, audience, and rationale.");
        }

        if (_judge is null)
        {
            _observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "accepted", null));
            return LoopEvaluation.Stop();
        }

        if (context.RunOptions?.AdditionalProperties is not { } properties ||
            !properties.TryGetValue(SourceContentKey, out var value) || value is not string sourceContent)
        {
            throw new InvalidOperationException("The original source is unavailable for Idea validation.");
        }

        var issues = await _judge.FindBlockingIssuesAsync(sourceContent, ideas[0], cancellationToken);
        if (issues.Count == 0)
        {
            _observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "accepted", null));
            return LoopEvaluation.Stop();
        }

        var feedback = "Regenerate the ranked ideas using the original source; the first idea is the one developed. " +
                       "Address these blocking issues in the first idea: " +
                       JsonSerializer.Serialize(issues);

        return Reject(context, responseText, feedback);
    }

    private LoopEvaluation Reject(LoopContext context, string responseText, string feedback,
        Exception? innerException = null)
    {
        _observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "rejected", feedback));

        // MAF counts completed generations starting at one, including the initial generation.
        if (context.Iteration >= MaxAttempts)
        {
            throw new InvalidOperationException(
                $"Idea agent failed to produce valid ideas after {MaxAttempts} attempts. {feedback}", innerException);
        }

        return LoopEvaluation.Continue(feedback);
    }
}

/// <summary>
///     A generator response and its validation outcome; iteration starts at one.
/// </summary>
public sealed record IdeaValidationAttempt(int Attempt, string Response, string Status, string? Feedback)
{
    public int Retry => Attempt - 1;
}

#pragma warning restore MAAI001