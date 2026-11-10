using System.Text.Json.Serialization;

namespace Nueyon.Compose.Domain;

/// <summary>
///     Immutable context for a single agent execution.
///     Contains diagnostic information that identifies and traces a complete agent execution.
///     This record is intentionally lightweight and contains no logging dependencies.
/// </summary>
public sealed record AgentExecutionContext(
    Guid ExecutionId)
{
    /// <summary>
    ///     Optional per-execution values for agents, such as callbacks; never serialized.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyDictionary<string, object?>? AdditionalProperties { get; init; }
}
