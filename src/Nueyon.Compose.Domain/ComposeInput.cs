namespace Nueyon.Compose.Domain;

public sealed record ComposeInput(
    NarrativeForCompose Narrative,
    ContentFormat Format);