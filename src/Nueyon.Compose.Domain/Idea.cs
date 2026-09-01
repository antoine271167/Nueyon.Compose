namespace Nueyon.Compose.Domain;

public sealed record Idea(
    string Title,
    string Description,
    string Audience,
    string Rationale);