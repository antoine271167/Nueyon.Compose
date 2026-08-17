using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Harness;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Infrastructure.Agents;
using Nueyon.Compose.Infrastructure.Options;

namespace Nueyon.Compose.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure services into the dependency injection container.
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Adds OpenAI-backed infrastructure services to the dependency injection container.
    /// Configures OpenAI options, validates configuration, and registers the Idea Agent and Idea Harness.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Register the IdeaValidator
        services.AddSingleton<IIdeaValidator, IdeaValidator>();

        // Register the Idea Agent (backed by OpenAI)
        services.AddSingleton<IAgent<ChatInput, IReadOnlyList<Idea>>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var aiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetSystemInstructions());

            return new IdeaAgent(aiAgent);
        });

        // Register the Idea Harness (with validation and retry logic)
        services.AddSingleton<IAgentHarness<ChatInput, IReadOnlyList<Idea>>>(provider =>
        {
            var agent = provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>();
            var validator = provider.GetRequiredService<IIdeaValidator>();
            return new IdeaHarness(agent, validator);
        });

        return services;
    }

    /// <summary>
    /// Gets the system instructions for the Idea Agent.
    /// </summary>
    /// <returns>The system instructions string.</returns>
    private static string GetSystemInstructions() => """
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
}
