using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
///     Factory for creating OpenAI-backed AIAgent instances using the official Microsoft Agent Framework OpenAI
///     integration.
/// </summary>
public static class OpenAiAgentFactory
{
    /// <summary>
    ///     Creates an AIAgent configured to use OpenAI with the specified model and instructions.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key.</param>
    /// <param name="model">The model to use (e.g., "gpt-5.4-mini").</param>
    /// <param name="systemInstructions">The system instructions for the agent.</param>
    /// <returns>A configured AIAgent instance.</returns>
    public static AIAgent CreateOpenAiAgent(string apiKey, string model, string systemInstructions)
    {
        ArgumentNullException.ThrowIfNull(apiKey);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(systemInstructions);

        // Create OpenAI client
        var openAiClient = new OpenAIClient(apiKey);

        // Get the Chat client
        var chatClient = openAiClient.GetChatClient(model).AsIChatClient();

        // Create AIAgent using the Chat client
        // Structured output is configured by the caller (Application layer) at execution time
        return AgentFactory.CreateAgent(chatClient, systemInstructions, "OpenAIAgent");
    }
}