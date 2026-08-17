using Microsoft.Agents.AI;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
/// Factory for creating IdeaAgent instances backed by OpenAI.
/// </summary>
public static class IdeaAgentFactory
{
    private const string ModelName = "gpt-5.4-mini";

    private const string SystemInstruction = """
        You are the Idea Agent in Nueyon.Compose.

        Your job is to transform a user's idea or thought into one or more concrete content ideas.

        Generate useful, specific ideas rather than generic topics.

        Each idea must have:
        - a short title
        - a clear description
        - a specific target audience
        - a clear rationale explaining why the idea is worth pursuing

        Return only valid JSON.
        Do not use Markdown.
        Do not wrap the JSON in ``` fences.
        Do not include explanations outside the JSON.
        """;

    /// <summary>
    /// Creates an IdeaAgent backed by OpenAI.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key. If null or empty, reads from OPENAI_API_KEY environment variable.</param>
    /// <returns>A configured IdeaAgent instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when API key is not provided and not found in environment.</exception>
    public static IAgent<ChatInput, IReadOnlyList<Idea>> CreateOpenAIIdeaAgent(string? apiKey = null)
    {
        // Get API key from parameter or environment
        apiKey ??= Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API key is required. Provide it as a parameter or set the OPENAI_API_KEY environment variable.");
        }

        // Create the OpenAI-backed AIAgent
        var aiAgent = OpenAIAgentFactory.CreateOpenAIAgent(apiKey, ModelName, SystemInstruction);

        // Create and return the IdeaAgent
        return new IdeaAgent(aiAgent);
    }
}
