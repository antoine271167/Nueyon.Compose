namespace Nueyon.Compose.Domain;

public sealed record ComposeInput(
    NarrativeResult Narrative,
    ContentFormat Format);