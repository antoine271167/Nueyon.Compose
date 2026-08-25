namespace Nueyon.Compose.Application.Agents.Research;

/// <summary>
///     Structured response returned by the Research agent.
/// </summary>
public sealed class ResearchResponse
{
    public required string Content { get; init; }
}