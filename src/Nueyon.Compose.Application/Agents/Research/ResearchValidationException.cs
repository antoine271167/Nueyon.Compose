namespace Nueyon.Compose.Application.Agents.Research;

public enum ResearchValidationFailureKind { Structural, Semantic }

public sealed record ResearchValidationBlocker(string Category, string ResearchLocation, string Problem);

/// <summary>Bounded Research attempts exhausted. Technical judge failures and cancellation propagate separately.</summary>
public sealed class ResearchValidationException : InvalidOperationException
{
    public ResearchValidationException(int attemptCount, ResearchValidationFailureKind kind,
        IEnumerable<ResearchValidationBlocker>? blockers = null)
        : base($"Research validation failed after {attemptCount} attempts ({kind}).")
    {
        AttemptCount = attemptCount;
        Kind = kind;
        Blockers = Array.AsReadOnly((blockers ?? []).ToArray());
    }

    public int AttemptCount { get; }
    public ResearchValidationFailureKind Kind { get; }
    public IReadOnlyList<ResearchValidationBlocker> Blockers { get; }
}
