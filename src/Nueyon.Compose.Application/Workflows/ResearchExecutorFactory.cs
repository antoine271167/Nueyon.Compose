using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public static class ResearchExecutorFactory
{
    public static FunctionExecutor<SelectedIdea, ResearchResult> CreateResearchExecutor(
        IAgent<ResearchInput, ResearchResult> agent)
    {
        ArgumentNullException.ThrowIfNull(agent);

        return new FunctionExecutor<SelectedIdea, ResearchResult>(
            "research",
            async (selectedIdea, context, cancellationToken) =>
            {
                var input = await context.ReadStateAsync<ChatInput>(
                                StoryWorkflowState.ChatInputKey,
                                StoryWorkflowState.ScopeName,
                                cancellationToken)
                            ?? throw new InvalidOperationException(
                                "ChatInput was not found in the StoryWorkflow state.");

                var researchInput = new ResearchInput
                {
                    Input = input,
                    SelectedIdea = selectedIdea
                };

                var executionContext = new AgentExecutionContext(Guid.NewGuid());

                return await agent.ExecuteAsync(
                    executionContext,
                    researchInput,
                    cancellationToken);
            });
    }
}