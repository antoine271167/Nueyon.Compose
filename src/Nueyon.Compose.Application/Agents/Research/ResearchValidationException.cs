namespace Nueyon.Compose.Application.Agents.Research;

public enum ResearchValidationFailureKind
{
    Structural,
    Semantic
}

public sealed record ResearchValidationBlocker(string Category, string ResearchLocation, string Problem);

/// <summary>
///     Bounded Research attempts exhausted. Technical judge failures and cancellation propagate separately.
/// </summary>
public sealed class ResearchValidationException(
    int attemptCount,
    ResearchValidationFailureKind kind,
    IEnumerable<ResearchValidationBlocker>? blockers = null)
    : InvalidOperationException($"Research validation failed after {attemptCount} attempts ({kind}).")
{
    public int AttemptCount { get; } = attemptCount;
    public ResearchValidationFailureKind Kind { get; } = kind;
    public IReadOnlyList<ResearchValidationBlocker> Blockers { get; } = Array.AsReadOnly((blockers ?? []).ToArray());
}