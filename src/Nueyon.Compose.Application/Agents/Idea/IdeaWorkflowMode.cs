namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>
///     Selects the Idea prompt set and validation budget. The mode is fixed when an agent is created.
/// </summary>
public enum IdeaWorkflowMode
{
    /// <summary>The original prompts with three generations.</summary>
    Legacy = 0,

    /// <summary>The concise prompts with two generations in total, sharing one repair.</summary>
    Simplified = 1
}

public static class IdeaWorkflowModes
{
    public const int LegacyMaxAttempts = 3;
    public const int SimplifiedMaxAttempts = 2;

    /// <summary>Total generations, including the initial one, that a mode may use.</summary>
    public static int MaxAttempts(this IdeaWorkflowMode mode) => mode switch
    {
        IdeaWorkflowMode.Legacy => LegacyMaxAttempts,
        IdeaWorkflowMode.Simplified => SimplifiedMaxAttempts,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown Idea workflow mode.")
    };

    /// <summary>Parses the exact names Legacy or Simplified (case-insensitive); numbers and blanks are rejected.</summary>
    public static bool TryParse(string? value, out IdeaWorkflowMode mode)
    {
        foreach (var candidate in Enum.GetValues<IdeaWorkflowMode>())
        {
            if (string.Equals(value, candidate.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                mode = candidate;
                return true;
            }
        }

        mode = default;
        return false;
    }
}
