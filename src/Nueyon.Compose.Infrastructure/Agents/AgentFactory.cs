using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
///     Centralizes the repeated creation of the MAF <see cref="AIAgent" /> from an <see cref="IChatClient" />.
/// </summary>
internal static class AgentFactory
{
    /// <summary>
    ///     Creates an <see cref="AIAgent" /> configured with the specified chat client, instructions and name.
    /// </summary>
    /// <param name="chatClient">The chat client backing the agent.</param>
    /// <param name="systemInstructions">The system instructions for the agent.</param>
    /// <param name="name">The name assigned to the agent.</param>
    /// <returns>A configured <see cref="AIAgent" /> instance.</returns>
    public static AIAgent CreateAgent(IChatClient chatClient, string systemInstructions, string name)
    {
        ArgumentNullException.ThrowIfNull(chatClient);
        ArgumentNullException.ThrowIfNull(systemInstructions);
        ArgumentNullException.ThrowIfNull(name);

        return chatClient.AsAIAgent(
            systemInstructions,
            name);
    }
}