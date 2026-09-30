namespace Nueyon.Compose.Domain;

/// <summary>
///     Describes how content should be structured/presented. The CompositionSpec controls
///     presentation only and must never become a source of factual meaning.
/// </summary>
public sealed record CompositionSpec(ContentFormat Format);