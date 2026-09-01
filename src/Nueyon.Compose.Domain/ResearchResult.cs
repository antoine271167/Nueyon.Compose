namespace Nueyon.Compose.Domain;

public sealed class ResearchResult
{
    public required string Content { get; init; }
}

public sealed record SynthesisInput(
    ResearchResult Research);

public sealed record SynthesisResult(
    string Content);