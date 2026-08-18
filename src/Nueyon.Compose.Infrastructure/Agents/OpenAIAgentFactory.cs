using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
/// Factory for creating OpenAI-backed AIAgent instances using the official Microsoft Agent Framework OpenAI integration.
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
        var openAIClient = new OpenAIClient(apiKey);

        // Get the Chat client
        var chatClient = openAIClient.GetChatClient(model);

        // Wrap with structured output decorator
        Func<IChatClient, IChatClient> clientFactory = (baseClient) =>
        {
            return new StructuredOutputChatClientDecorator(baseClient);
        };

        // Create AIAgent using the Chat client with structured output support
        var agent = chatClient.AsAIAgent(
            instructions: systemInstructions,
            name: "OpenAIAgent",
            clientFactory: clientFactory);

        return agent;
    }
}
