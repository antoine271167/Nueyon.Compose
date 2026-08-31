using Microsoft.Agents.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Infrastructure.Agents;
using Nueyon.Compose.Infrastructure.Options;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Nueyon.Compose.Infrastructure;

/// <summary>
///     Extension methods for registering Infrastructure services into the dependency injection container.
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    ///     Adds OpenAI-backed infrastructure services to the dependency injection container.
    ///     Configures OpenAI options, validates configuration, and registers the Idea Agent with LoopAgent-backed
    ///     validation/retry logic.
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

        // Register the Idea Validation Loop Evaluator (stateless, safe for concurrent use)
        services.AddSingleton<IdeaValidationLoopEvaluator>(provider =>
        {
            var validator = provider.GetRequiredService<IIdeaValidator>();
            return new IdeaValidationLoopEvaluator(validator);
        });

        // Register the Idea Agent with LoopAgent-backed validation and retry logic
        // The agent is created as a LoopAgent wrapping the OpenAI AIAgent
        services.AddSingleton<IAgent<ChatInput, IReadOnlyList<Idea>>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var logger = provider.GetRequiredService<ILogger<IdeaAgent>>();

            // Create the base OpenAI AIAgent
            var baseAiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetSystemInstructions());

            // Create the loop evaluator for validation and retry decision-making
            var evaluator = provider.GetRequiredService<IdeaValidationLoopEvaluator>();

            var loopOptions = new LoopAgentOptions
            {
                MaxIterations = 3
            };

            // Create the LoopAgent that wraps the base OpenAI agent
            // The evaluator will decide when ideas are valid and the loop should stop
            var loopAgent = new LoopAgent(baseAiAgent, evaluator, loopOptions);

            // Return the IdeaAgent that uses the loop-backed agent
            return new IdeaAgent(loopAgent, logger);
        });

        // Register the Research Agent
        services.AddSingleton<IAgent<ResearchInput, ResearchResult>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var logger = provider.GetRequiredService<ILogger<ResearchAgent>>();

            var baseAiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetResearchSystemInstructions());

            return new ResearchAgent(baseAiAgent, logger);
        });

        return services;
    }

    /// <summary>
    ///     Gets the system instructions for the Research Agent.
    /// </summary>
    /// <returns>The system instructions string.</returns>
    private static string GetResearchSystemInstructions() =>
        """
        You are the Research Agent in Nueyon.Compose.

        Your job is to research and develop useful background material for
        a selected content idea.

        Use the original user input to understand the user's intent and context.
        Use the selected idea as the specific subject to investigate.

        Produce relevant, concrete research material that can later be used by
        another agent to create a high-quality story or article.

        Focus on:
        - important facts and context
        - useful concepts and terminology
        - relevant arguments or perspectives
        - interesting supporting details
        - potential angles worth exploring

        Return only valid JSON.
        Do not use Markdown.
        Do not wrap the JSON in ``` fences.
        Do not include explanations outside the JSON.
        """;

    /// <summary>
    ///     Gets the system instructions for the Idea Agent.
    /// </summary>
    /// <returns>The system instructions string.</returns>
    private static string GetSystemInstructions() =>
        """
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

#pragma warning restore MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.