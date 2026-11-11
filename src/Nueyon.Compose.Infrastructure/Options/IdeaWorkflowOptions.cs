using Nueyon.Compose.Application.Agents.Idea;

namespace Nueyon.Compose.Infrastructure.Options;

/// <summary>
///     Strongly typed configuration of the Idea workflow, bound from the <c>IdeaWorkflow</c> section.
///     A missing section or key selects Legacy; a present but empty or unknown mode is a configuration error.
/// </summary>
public sealed class IdeaWorkflowOptions
{
    public const string SectionName = "IdeaWorkflow";

    /// <summary>Legacy or Simplified.</summary>
    public string? Mode { get; set; } = nameof(IdeaWorkflowMode.Legacy);

    /// <summary>Returns the configured mode.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the mode is empty or not Legacy or Simplified.</exception>
    public IdeaWorkflowMode ResolveMode() =>
        IdeaWorkflowModes.TryParse(Mode, out var mode)
            ? mode
            : throw new InvalidOperationException(
                $"{SectionName}:Mode must be '{nameof(IdeaWorkflowMode.Legacy)}' or '{nameof(IdeaWorkflowMode.Simplified)}'.");

    /// <exception cref="InvalidOperationException">Thrown when the mode is empty or unknown.</exception>
    public void Validate() => ResolveMode();
}
