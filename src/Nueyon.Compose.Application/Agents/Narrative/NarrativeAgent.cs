using System.Diagnostics;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
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
        $"""
          # Narrative — User Prompt
          
          Create a concise editorial brief for the Compose stage.
          
          ## Input
          
          ### Selected Idea
          
          {input.SelectedIdea.Content}
          
          ### Synthesis
          
          {input.Synthesis.Content}
          
          ## Objective
          
          Turn the Selected Idea into one clear, focused story direction that Compose can develop into an article.
          
          **The Selected Idea defines what the story is about. Synthesis provides supporting evidence and context; it must not redefine or broaden the story.**
          
          ## Instructions
          
          ### 1. Preserve the Selected Idea
          
          - Identify the central point of the Selected Idea.
          - Preserve its original meaning, focus, and intended message.
          - Do not replace it with a broader topic suggested by Synthesis.
          - Do not turn an insight about one specific development into a general explanation of the entire product or project.
          
          ### 2. Select Supporting Evidence
          
          - Use Synthesis to identify facts, interpretations, and context that directly support the Selected Idea.
          - Include only information that helps explain or substantiate the central story.
          - Omit relevant but unnecessary information.
          - Do not include every product feature, workflow stage, architectural decision, naming decision, or implementation detail simply because it appears in Synthesis.
          - Do not introduce facts or conclusions that are not supported by the input.
          
          ### 3. Define the Story Progression
          
          Describe a simple progression that Compose can turn into a coherent article.
          
          The progression should normally cover:
          
          1. **Starting point:** The initial situation, assumption, or problem established by the input.
          2. **Key development:** The important change, discovery, or realization supported by the input.
          3. **Significance:** What this development means, within the limits of the available evidence.
          
          Use only the steps that fit the Selected Idea. Do not invent events or force the story into this structure when the input does not support them.
          
          ### 4. Keep the Story Focused
          
          - Build the brief around one central idea.
          - Prefer a clear story over a comprehensive project summary.
          - Include product or implementation details only when they help the reader understand the central idea.
          - Do not instruct Compose to explain the complete architecture, every workflow stage, the entire product vision, or the full development history unless the Selected Idea specifically concerns those subjects.
          - Do not add secondary themes merely because they are interesting or relevant to the wider project.
          
          ### 5. Respect Factual Boundaries
          
          - Do not invent motivations, reasons for decisions, causal relationships, results, metrics, comparisons, or outcomes.
          - Do not claim that an approach is better than an alternative unless the input provides supporting evidence.
          - Preserve the distinction between established facts, interpretations, and unknowns.
          - Record only the critical limitations that affect what the article can claim.
          - Treat unknowns as boundaries, not as additional topics that the article must discuss.
          
          ### 6. Define the Brief, Not the Article
          
          - Do not write article paragraphs, an introduction, a conclusion, or polished prose intended for publication.
          - Do not prescribe the content of every paragraph.
          - Give Compose a clear editorial direction and enough supporting context to write the article independently.
          - Keep the output concise and specific.
          
          ## Output Format
          
          ### Selected Story
          
          State the central story in 1–2 sentences.
          
          The story must preserve the original meaning of the Selected Idea and identify the main point the article should communicate.
          
          ### Story Progression
          
          Provide 3–4 concise bullet points describing the essential progression of the story.
          
          Each point must advance the same central idea. Include only developments supported by the input.
          
          ### Critical Constraints
          
          List the essential factual boundaries and unsupported claims that Compose must avoid.
          
          Include only constraints that materially affect the article. If no additional constraints are necessary, state that explicitly.
          
          ## Final Validation
          
          Before producing the output, verify that:
          
          - The Selected Story preserves the original Selected Idea.
          - The Selected Idea, not the breadth of Synthesis, determines the story's focus.
          - Every progression point contributes directly to the central story.
          - Unnecessary project, product, and architecture details have been excluded.
          - No unsupported meaning, motivation, causality, or conclusion has been introduced.
          - Compose receives an editorial direction, not a disguised article outline with every detail prescribed.
          
          **Priority rule:** When the Selected Idea and Synthesis point toward different levels of scope, preserve the Selected Idea. Use Synthesis selectively as supporting evidence. Never broaden the story merely to cover more available information.
          """;

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