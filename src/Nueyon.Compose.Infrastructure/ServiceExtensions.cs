using Microsoft.Agents.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Compose;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Narrative;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
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

        // Register the Synthesizer Agent
        services.AddSingleton<IAgent<SynthesisInput, SynthesisResult>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var logger = provider.GetRequiredService<ILogger<SynthesizerAgent>>();

            var baseAiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetSynthesisSystemInstructions());

            return new SynthesizerAgent(baseAiAgent, logger);
        });

        // Register the Narrative Agent
        services.AddSingleton<IAgent<NarrativeInput, NarrativeResult>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var logger = provider.GetRequiredService<ILogger<NarrativeAgent>>();

            var baseAiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetNarrativeSystemInstructions());

            return new NarrativeAgent(baseAiAgent, logger);
        });

        // Register the Compose Agent
        services.AddSingleton<IAgent<ComposeInput, ComposeResult>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            options.Validate();

            var logger = provider.GetRequiredService<ILogger<ComposeAgent>>();

            var baseAiAgent = OpenAIAgentFactory.CreateOpenAIAgent(
                options.ApiKey,
                options.Model,
                GetComposeSystemInstructions());

            return new ComposeAgent(baseAiAgent, logger);
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

    private static string GetSynthesisSystemInstructions() =>
        """
        You are the Synthesizer Agent in Nueyon.Compose.

        Your job is to read research produced by the Research Agent and produce a concise
        editorial synthesis (a brief) that captures the meaning of the research. Do NOT
        produce a final article, social-media post, or any consumer-facing content.

        The synthesis must:
        - Determine the central thesis or message supported by the research.
        - Identify the most important insights.
        - Preserve important facts and supporting evidence.
        - Surface meaningful nuances, uncertainty, or contradictions.
        - Not invent facts or add information not supported by the research.

        Prefer clear headings such as:
        - Core thesis
        - Key insights
        - Supporting evidence
        - Nuances

        Return only valid JSON.
        Do not use Markdown.
        Do not wrap the JSON in ``` fences.
        Do not include explanations outside the JSON.
        """;

    private static string GetNarrativeSystemInstructions() =>
        """
        You are the Narrative Agent in Nueyon.Compose.

        Transform an editorial synthesis into a coherent and compelling narrative structure that can later be turned into content.

        Determine:
        - the central narrative angle
        - an effective opening or hook
        - the logical progression of the story
        - the order of important insights
        - where supporting evidence belongs
        - meaningful tension, contrast, or progression when appropriate
        - the conclusion or takeaway

        Do not merely paraphrase the synthesis.
        Do not perform research.
        Do not invent facts or unsupported claims.
        Do not write final content or platform-specific content.

        Preserve the facts, evidence, nuances, uncertainty, and meaning contained in the synthesis.

        Return only valid JSON.
        Do not use Markdown.
        Do not wrap JSON in code fences.
        Do not include explanations outside the JSON.
        """;

    private static string GetComposeSystemInstructions() =>
        """
        You are the Compose Agent in Nueyon.Compose.

        Transform the supplied narrative into finished content for the requested format.

        For Article format, produce a complete article with appropriate title, introduction, body, transitions, and conclusion where appropriate.

        Do not merely copy or paraphrase the narrative.
        Do not research or invent facts.
        Preserve the narrative's facts, evidence, nuances, uncertainty, and intended meaning.

        Return only valid JSON.
        Do not use Markdown.
        Do not wrap JSON in code fences.
        Do not include explanations outside the JSON.
        """;
}

#pragma warning restore MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.