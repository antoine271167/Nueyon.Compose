namespace Nueyon.Compose.Domain;

public sealed class Idea
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string Audience { get; init; }

    public required string Rationale { get; init; }
}
