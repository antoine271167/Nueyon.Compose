using System.Collections.ObjectModel;

namespace Nueyon.Compose.Application.Agents.Idea;

public enum IdeaValidationFailureKind
{
    /// <summary>The response was unparsable or the first idea had an empty required field.</summary>
    Structural,

    /// <summary>The semantic judge rejected the first idea.</summary>
    Semantic
}

/// <summary>The part of a semantic blocker that is safe to show: never the source evidence.</summary>
public sealed record IdeaValidationBlocker(string Category, string Field, string Problem);

/// <summary>
///     The Idea generations were exhausted without an acceptable first idea. Technical judge failures and
///     cancellation are not reported with this type.
/// </summary>
public sealed class IdeaValidationException : InvalidOperationException
{
    public IdeaValidationException(string message, int attemptCount, IdeaValidationFailureKind kind,
        string diagnosis, IEnumerable<IdeaValidationBlocker>? blockers = null, Exception? innerException = null)
        : base(message, innerException)
    {
        AttemptCount = attemptCount;
        Kind = kind;
        Diagnosis = diagnosis;
        Blockers = new ReadOnlyCollection<IdeaValidationBlocker>([.. blockers ?? []]);
    }

    public int AttemptCount { get; }
    public IdeaValidationFailureKind Kind { get; }

    /// <summary>The last rejection feedback, exactly as given to the repair prompt and the optional trace.</summary>
    public string Diagnosis { get; }

    /// <summary>The last semantic blockers; empty for a structural failure.</summary>
    public IReadOnlyList<IdeaValidationBlocker> Blockers { get; }
}
