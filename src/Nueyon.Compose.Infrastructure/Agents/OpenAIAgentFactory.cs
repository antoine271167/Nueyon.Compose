using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
/// Factory for creating OpenAI-backed AIAgent instances.
/// </summary>
public static class OpenAIAgentFactory
{
    /// <summary>
    /// Creates an AIAgent configured to use OpenAI with the specified model and instructions.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key.</param>
    /// <param name="model">The model to use (e.g., "gpt-5.4-mini").</param>
    /// <param name="systemInstructions">The system instructions for the agent.</param>
    /// <returns>A configured AIAgent instance.</returns>
    public static AIAgent CreateOpenAIAgent(string apiKey, string model, string systemInstructions)
    {
        ArgumentNullException.ThrowIfNull(apiKey);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(systemInstructions);

        // Create OpenAI client
        var client = new OpenAIClient(apiKey);

        // Create chat client for the specified model
        var openAIChatClient = client.GetChatClient(model);

        // Cast to IChatClient for Agent Framework
        IChatClient chatClient = (IChatClient)openAIChatClient;

        // Create AIAgent from the chat client with system instructions
        var agent = new ChatClientAgent(chatClient, systemInstructions);

        return agent;
    }
}
