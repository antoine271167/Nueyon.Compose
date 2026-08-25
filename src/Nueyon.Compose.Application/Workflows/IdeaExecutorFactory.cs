using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public static class IdeaExecutorFactory
{
    public static FunctionExecutor<ChatInput, Idea[]> CreateIdeaExecutor(
        IAgent<ChatInput, IReadOnlyList<Idea>> agent)
    {
        ArgumentNullException.ThrowIfNull(agent);

        return new FunctionExecutor<ChatInput, Idea[]>(
            "idea",
            async (input, context, cancellationToken) =>
            {
                await context.QueueStateUpdateAsync(
                    StoryWorkflowState.ChatInputKey,
                    input,
                    StoryWorkflowState.ScopeName,
                    cancellationToken);

                var executionContext = new AgentExecutionContext(Guid.NewGuid());

                var ideas = await agent.ExecuteAsync(
                    executionContext,
                    input,
                    cancellationToken);

                return ideas.ToArray();
            });
    }
}