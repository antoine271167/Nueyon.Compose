namespace Nueyon.Compose.Domain;

/// <summary>
///     Immutable context for a single flow execution.
///     Contains diagnostic information that identifies and traces a complete flow execution.
///     This record is intentionally lightweight and contains no logging dependencies.
/// </summary>
public sealed record FlowExecutionContext(
    Guid ExecutionId);
