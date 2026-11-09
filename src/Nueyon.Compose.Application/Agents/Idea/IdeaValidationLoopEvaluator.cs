using System.Text.Json;
using Microsoft.Agents.AI;
using Nueyon.Compose.Application.Validation;

#pragma warning disable MAAI001 // LoopAgent APIs are experimental.

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>Validates each generation before allowing the Idea loop to return it.</summary>
public sealed class IdeaValidationLoopEvaluator(IIdeaValidator validator) : LoopEvaluator
{
    // Retain structural-only validation for callers that use the evaluator outside production.

    public IdeaValidationLoopEvaluator(IIdeaValidator validator, IdeaSemanticJudge judge) : this(validator) =>
        _judge = judge ?? throw new ArgumentNullException(nameof(judge));

    private const int MaxAttempts = 3;
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
        LoopContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();

        var responseText = context.LastResponse.Text;
        IReadOnlyList<Domain.Idea> ideas;
        try
        {
            ideas = IdeaAgent.ParseIdeasFromJson(responseText);
        }
        catch (InvalidOperationException ex)
        {
            return Reject(context,
                "Return a valid JSON object with an ideas array matching the response schema.", ex);
        }

        if (!_validator.IsValid(ideas))
        {
            return Reject(context,
                "Return at least one idea, each with a non-empty title, description, audience, and rationale.");
        }

        if (_judge is null)
        {
            return LoopEvaluation.Stop();
        }

        if (context.RunOptions?.AdditionalProperties is not { } properties ||
            !properties.TryGetValue(SourceContentKey, out var value) || value is not string sourceContent)
        {
            throw new InvalidOperationException("The original source is unavailable for Idea validation.");
        }

        var issues = await _judge.FindBlockingIssuesAsync(sourceContent, ideas, cancellationToken);
        if (issues.Count == 0)
        {
            return LoopEvaluation.Stop();
        }

        var feedback = "Regenerate the ranked ideas using the original source. Address these blocking issues: " +
                       JsonSerializer.Serialize(issues);
        return Reject(context, feedback);
    }

    private static LoopEvaluation Reject(LoopContext context, string feedback, Exception? innerException = null)
    {
        // MAF counts completed generations starting at one, including the initial generation.
        if (context.Iteration >= MaxAttempts)
        {
            throw new InvalidOperationException(
                $"Idea agent failed to produce valid ideas after {MaxAttempts} attempts. {feedback}", innerException);
        }

        return LoopEvaluation.Continue(feedback);
    }
}

#pragma warning restore MAAI001