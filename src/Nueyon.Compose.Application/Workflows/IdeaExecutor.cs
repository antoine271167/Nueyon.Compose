using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
/// A factory method for creating the Idea executor used in the MAF Workflow.
/// 
/// Creates a FunctionExecutor that adapts the workflow message model (ChatInput) 
/// to the existing agent abstraction.
/// </summary>
public static class IdeaExecutorFactory
{
    /// <summary>
    /// Creates a FunctionExecutor that invokes the existing Idea Agent.
    /// The return value from the handler is automatically yielded as output.
    /// </summary>
    /// <param name="agent">The agent to invoke for generating ideas.</param>
    /// <returns>A FunctionExecutor that transforms ChatInput to IReadOnlyList<Idea>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when agent is null.</exception>
    public static FunctionExecutor<ChatInput, IReadOnlyList<Idea>> CreateIdeaExecutor(
        IAgent<ChatInput, IReadOnlyList<Idea>> agent)
    {
        ArgumentNullException.ThrowIfNull(agent);

        // Create and return the executor using the handler function
        // The executor will automatically:
        // - Declare IReadOnlyList<Idea> as the output type
        // - Yield the handler's return value as output
        return new FunctionExecutor<ChatInput, IReadOnlyList<Idea>>(
            id: "idea",
            handlerAsync: HandleAsync);

        // Create a function that will be called by the workflow for each input message
        // The return value will be automatically yielded as a WorkflowOutputEvent
        async ValueTask<IReadOnlyList<Idea>> HandleAsync(
            ChatInput input,
            IWorkflowContext context,
            CancellationToken cancellationToken)
        {
            // Create a flow execution context for the agent.
            // Use a new ExecutionId for each workflow invocation.
            var flowContext = new FlowExecutionContext(Guid.NewGuid());

            // Invoke the existing agent with the input.
            // Pass through the cancellation token unchanged.
            var ideas = await agent.ExecuteAsync(flowContext, input, cancellationToken);

            // Return the result - it will be automatically yielded by the executor
            return ideas;
        }
    }
}

