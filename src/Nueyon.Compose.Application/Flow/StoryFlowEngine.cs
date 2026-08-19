using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Flow;

/// <summary>
///     The flow engine that orchestrates the IDEA flow.
///     Delegates to the Idea Agent for generating content ideas from user input.
///     Provides observability through structured logging of flow lifecycle events.
/// </summary>
public sealed class StoryFlowEngine : IFlowEngine
{
    /// <summary>
    ///     Initializes a new instance of the StoryFlowEngine with the specified agent and logger.
    /// </summary>
    /// <param name="ideaAgent">The agent for executing the IDEA flow (includes validation and retry via LoopAgent).</param>
    /// <param name="logger">The logger for structured diagnostic logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when ideaAgent or logger is null.</exception>
    public StoryFlowEngine(
        IAgent<ChatInput, IReadOnlyList<Idea>> ideaAgent,
        ILogger<StoryFlowEngine> logger)
    {
        _ideaAgent = ideaAgent ?? throw new ArgumentNullException(nameof(ideaAgent));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private readonly IAgent<ChatInput, IReadOnlyList<Idea>> _ideaAgent;
    private readonly ILogger<StoryFlowEngine> _logger;

    /// <summary>
    ///     Executes the IDEA flow on the provided workspace.
    ///     Logs flow lifecycle events and measures execution duration.
    /// </summary>
    /// <param name="executionContext">The execution context containing the ExecutionId for this flow.</param>
    /// <param name="workspace">The workspace to process. Its Input will be passed to the flow.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The workspace with Ideas populated by the flow execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when executionContext or workspace is null.</exception>
    public async Task<StoryWorkspace> ExecuteAsync(
        FlowExecutionContext executionContext,
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(workspace);

        var flowName = "Story";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Flow {FlowName} started with ExecutionId {ExecutionId}",
                flowName,
                executionContext.ExecutionId);

            var ideas = await _ideaAgent.ExecuteAsync(
                executionContext,
                workspace.Input,
                cancellationToken);

            stopwatch.Stop();

            workspace.Ideas = ideas;

            _logger.LogInformation(
                "Flow {FlowName} completed in {Duration}ms with ExecutionId {ExecutionId}",
                flowName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            return workspace;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Flow {FlowName} failed after {Duration}ms with ExecutionId {ExecutionId}",
                flowName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            throw;
        }
    }
}
