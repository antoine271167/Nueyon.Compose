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

    /// <summary>
    ///     Key of the optional per-invocation <c>Action&lt;IdeaValidationAttempt&gt;</c> observer, used both in
    ///     <see cref="Domain.AgentExecutionContext.AdditionalProperties" /> and in the MAF run options.
    /// </summary>
    public const string ObserverKey = "Nueyon.Compose.Idea.Observer";

    internal const string SourceContentKey = "Nueyon.Compose.Idea.SourceContent";

    private readonly IdeaSemanticJudge? _judge;

    private readonly IIdeaValidator _validator = validator ?? throw new ArgumentNullException(nameof(validator));

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

        var observer = GetObserver(context);
        var responseText = context.LastResponse.Text;
        observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "incomplete", null));
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
            observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "accepted", null));
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
            observer?.Invoke(new IdeaValidationAttempt(context.Iteration, responseText, "accepted", null));
            return LoopEvaluation.Stop();
        }

        var feedback = "Regenerate the ranked ideas using the original source; the first idea is the one developed. " +
                       "Address these blocking issues in the first idea: " +
                       JsonSerializer.Serialize(issues);

        return Reject(context, responseText, feedback);
    }

    // The observer travels with the invocation, so concurrent runs of this shared evaluator stay separate.
    private static Action<IdeaValidationAttempt>? GetObserver(LoopContext context) =>
        context.RunOptions?.AdditionalProperties is { } properties &&
        properties.TryGetValue(ObserverKey, out var value) && value is Action<IdeaValidationAttempt> observer
            ? observer
            : null;

    private static LoopEvaluation Reject(LoopContext context, string responseText, string feedback,
        Exception? innerException = null)
    {
        GetObserver(context)?.Invoke(
            new IdeaValidationAttempt(context.Iteration, responseText, "rejected", feedback));

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