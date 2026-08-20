using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     A factory method for creating the Idea executor used in the MAF Workflow.
///     Creates a FunctionExecutor that adapts the workflow message model (ChatInput)
///     to the existing agent abstraction.
/// </summary>
public static class IdeaExecutorFactory
{
    /// <summary>
    ///     Creates a FunctionExecutor that invokes the existing Idea Agent.
    ///     The return value from the handler is automatically yielded as output.
    ///     MAF requires a concrete array type as the output; the result from the agent
    ///     is converted to Idea[] to satisfy this constraint.
    /// </summary>
    /// <param name="agent">The agent to invoke for generating ideas.</param>
    /// <returns>A FunctionExecutor that transforms ChatInput to Idea[].</returns>
    /// <exception cref="ArgumentNullException">Thrown when agent is null.</exception>
    public static FunctionExecutor<ChatInput, Idea[]> CreateIdeaExecutor(
        IAgent<ChatInput, IReadOnlyList<Idea>> agent)
    {
        ArgumentNullException.ThrowIfNull(agent);

        return new FunctionExecutor<ChatInput, Idea[]>(
            "idea",
            HandleAsync);

        async ValueTask<Idea[]> HandleAsync(
            ChatInput input,
            IWorkflowContext context,
            CancellationToken cancellationToken)
        {
            var flowContext = new FlowExecutionContext(Guid.NewGuid());
            var ideas = await agent.ExecuteAsync(flowContext, input, cancellationToken);
            return [.. ideas];
        }
    }
}