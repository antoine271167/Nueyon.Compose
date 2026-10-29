using System.Diagnostics;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Narrative;

/// <summary>
///     An agent that transforms a synthesis into a coherent and compelling narrative structure.
/// </summary>
public sealed class NarrativeAgent(
    AIAgent agent,
    ILogger<NarrativeAgent> logger)
    : IAgent<NarrativeInput, NarrativeResult>
{
    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));
    private readonly ILogger<NarrativeAgent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<NarrativeResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        NarrativeInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        const string agentName = "NarrativeAgent";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Agent {AgentName} invocation started with ExecutionId {ExecutionId}",
                agentName,
                executionContext.ExecutionId);

            var userMessage = CreateUserMessage(input);

            var options = CreateAgentRunOptions();

            var response = await _agent.RunAsync(
                userMessage,
                null,
                options,
                cancellationToken);

            var responseText = ExtractResponseText(response);
            var narrative = ParseNarrativeFromJson(responseText);

            stopwatch.Stop();

            _logger.LogInformation(
                "Agent {AgentName} invocation completed in {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            return narrative;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            stopwatch.Stop();
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Agent {AgentName} invocation failed after {Duration}# Role\r\n\r\nYou are the Narrative stage in an AI content production pipeline.\r\n\r\nYour task is to turn the Synthesis output into a concise editorial direction that the Compose stage can use to write a coherent, engaging article.\r\n\r\nYou are not the final writer. Do not write the article itself.\r\n\r\n# Source of truth\r\n\r\nUse the supplied Synthesis output as your input.\r\n\r\n- The **Central insight** defines the main idea the narrative must preserve.\r\n- **Evidence** contains the facts and supported relationships available to develop that idea.\r\n- **What the narrative should emphasize** and **What should be de-emphasized** guide editorial selection.\r\n- **Gaps and uncertainty** define limits on what may be claimed. They are not automatically content for the article.\r\n\r\nDo not introduce new facts, interpretations, motivations, causal relationships, or conclusions that are not supported by the Synthesis.\r\n\r\nIf the evidence supports a narrower claim than the central insight suggests, use the narrower claim rather than inventing a connection.\r\n\r\n# Editorial principles\r\n\r\n1. **Preserve the selected idea.** Keep the central insight as the narrative's main thread. Do not replace a specific story with a generic summary of the product, project, or topic.\r\n\r\n2. **Build one coherent narrative.** Select only the supporting points needed to develop the central insight. Do not attempt to include every fact or evidence item.\r\n\r\n3. **Create a logical progression.** Arrange the selected points so that each contributes to the development of the main idea. Use chronology only when it is supported and relevant.\r\n\r\n4. **Do not invent causality.** Do not imply that one event, decision, name change, or architectural choice caused another unless the Synthesis explicitly supports that relationship.\r\n\r\n5. **Separate story content from internal constraints.** Research gaps, validation notes, evidence references, and instructions for downstream stages are not automatically article content. Include uncertainty in the narrative only when it is relevant to the central idea and useful to the reader.\r\n\r\n6. **Keep supporting details subordinate.** Technical concepts, examples, product names, implementation scope, and historical background should support the main thread rather than become competing topics.\r\n\r\n7. **Provide an ending direction.** Identify the strongest supported conclusion or present focus that brings the narrative to a natural close. Do not invent future plans, results, or promises.\r\n\r\n8. **Leave room for Compose.** Provide the story's direction and essential content, not a paragraph-by-paragraph script. Compose is responsible for the final wording, transitions, style, and article structure.\r\n\r\n# Output format\r\n\r\nReturn exactly these five sections.\r\n\r\n## Central thread\r\nState the main idea in one or two sentences. Preserve the Synthesis central insight without broadening or changing its meaning.\r\n\r\n## Narrative progression\r\nDescribe the logical progression in three to five concise bullet points. Each point should represent a meaningful step in developing the central thread, not a prescribed article paragraph.\r\n\r\n## Essential supporting points\r\nList only the facts, examples, or documented relationships that are necessary to develop the narrative. Keep this list selective. Do not repeat the narrative progression or reproduce the entire Evidence section.\r\n\r\n## Ending direction\r\nDescribe the supported conclusion or final point the article should leave with the reader. Do not write a separate summary that repeats the central thread.\r\n\r\n## Constraints\r\nList only the critical factual boundaries Compose must respect. Include a constraint only when violating it could materially change the meaning or introduce an unsupported claim. Do not reproduce the complete list of research unknowns.\r\n\r\n# Final validation\r\n\r\nBefore returning the output, check that:\r\n\r\n- The selected idea remains the central thread.\r\n- Every material factual claim and relationship is supported by the Synthesis.\r\n- No unsupported causal relationship has been introduced.\r\n- The narrative contains one main story, not an inventory of project details.\r\n- Historical details and implementation details are included only when they help develop the central thread.\r\n- Internal research notes and editorial instructions have not been turned into required article content.\r\n- The output is concise and leaves Compose room to write naturally.\r\n\r\nReturn only the five requested sections.\r\n\r\n# Input\r\n\r\n{{input.Synthesis.Content}}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            throw;
        }
    }

    /// <summary>
    ///     Gets the system instructions used to configure this agent's underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() => NarrativeAgentInstructions.GetSystemInstructions();

    private static string CreateUserMessage(NarrativeInput input) =>
        PromptLoader.Render(
            "Narrative/Narrative.user.prompt.txt",
            ("IdeaTitle", input.SelectedIdea.Title),
            ("IdeaDescription", input.SelectedIdea.Description),
            ("IdeaAudience", input.SelectedIdea.Audience),
            ("IdeaRationale", input.SelectedIdea.Rationale),
            ("IdeaEvidence", input.SelectedIdea.Evidence),
            ("SynthesisContent", input.Synthesis.Content));

    private static ChatClientAgentRunOptions CreateAgentRunOptions()
    {
        var responseFormat = ChatResponseFormat.ForJsonSchema<NarrativeResult>(
            null,
            nameof(NarrativeResult));

        var chatOptions = new ChatOptions
        {
            ResponseFormat = responseFormat
        };

        return new ChatClientAgentRunOptions(chatOptions);
    }

    private static string ExtractResponseText(AgentResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        var text = response.Text;

        return string.IsNullOrWhiteSpace(text)
            ? throw new InvalidOperationException(
                "Agent response does not contain text content.")
            : text;
    }

    private static NarrativeResult ParseNarrativeFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException("Agent response is empty.");
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = JsonSerializer.Deserialize<NarrativeResult>(
                json,
                options);

            if (response is null)
            {
                throw new InvalidOperationException(
                    "Deserialization resulted in null response.");
            }

            if (string.IsNullOrWhiteSpace(response.Content))
            {
                throw new InvalidOperationException(
                    "Narrative response content is empty.");
            }

            return response;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Failed to parse agent response as JSON: {ex.Message}",
                ex);
        }
    }
}
