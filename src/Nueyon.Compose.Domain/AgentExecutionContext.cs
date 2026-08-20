namespace Nueyon.Compose.Domain;

/// <summary>
///     Immutable context for a single agent execution.
///     Contains diagnostic information that identifies and traces a complete agent execution.
///     This record is intentionally lightweight and contains no logging dependencies.
/// </summary>
public sealed record AgentExecutionContext(
    Guid ExecutionId);
