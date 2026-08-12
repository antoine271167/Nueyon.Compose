using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using OpenAI;
using OpenAI.Responses;

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

        // Get the Responses client (experimental API, but official OpenAI SDK integration)
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.
        var responsesClient = openAIClient.GetResponsesClient();
#pragma warning restore OPENAI001

        // Create AIAgent using the official Microsoft Agent Framework OpenAI integration
        var agent = responsesClient.AsAIAgent(
            model: model,
            instructions: systemInstructions,
            name: "OpenAIAgent");

        return agent;
    }
}
