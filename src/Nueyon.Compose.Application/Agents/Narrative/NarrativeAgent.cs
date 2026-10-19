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

         Create a concise editorial brief that enables Compose to produce compelling, well-written content in the requested output format.

         ## Input

         ### Selected Idea

         Title: {input.SelectedIdea.Title}
         Description: {input.SelectedIdea.Description}
         Audience: {input.SelectedIdea.Audience}
         Rationale: {input.SelectedIdea.Rationale}
         Evidence: {input.SelectedIdea.Evidence}

         ### Synthesis

         {input.Synthesis.Content}

         ## Objective

         Turn the Selected Idea and its supporting Synthesis into one clear, compelling, focused story direction that Compose can express in the requested content format.

         **The Selected Idea defines what the story is about. Synthesis provides the evidence and context from which the story can be developed. Neither the availability of facts nor the plausibility of an interpretation permits unsupported claims or unnecessary expansion of the story.**

         The goal is not merely to summarize the available information. Identify the most compelling story the evidence supports, preserve its factual boundaries, and give Compose enough editorial direction to communicate it effectively.

         ## Instructions

         ### 1. Preserve the Selected Idea

         - Identify the central point of the Selected Idea.
         - Preserve its original meaning, focus, and intended message.
         - Do not replace it with a broader topic suggested by Synthesis.
         - Do not turn an insight about one specific development into a general explanation of the entire product or project.
         - Treat the Selected Idea as the editorial objective, not as a rigid prescription of how the story must be told.
         - If the Selected Idea contains an unsupported assertion, preserve its intended subject while narrowing the claim to what the evidence establishes.

         ### 2. Select Supporting Evidence

         Use Synthesis to identify the facts, interpretations, examples, and context that can responsibly support the story.

         - Select details for both their evidential value and their ability to make the story clearer, more concrete, interesting, or memorable.
         - Prefer the strongest illustrative details over a comprehensive list of related facts.
         - Include context when it materially improves the reader's understanding of the central insight.
         - Exclude details that do not strengthen the story, even when they are factually correct or relevant to the wider project.
         - Do not include every product feature, workflow stage, architectural decision, naming decision, or implementation detail simply because it appears in Synthesis.
         - Do not introduce facts or conclusions that are not supported by the input.
         - Do not treat an item marked for emphasis in Synthesis as mandatory if including it would dilute the Selected Idea.
         - If Synthesis contains a claim that conflicts with its own stated uncertainties or exceeds the underlying evidence, do not repeat the unsupported claim. Preserve only the defensible information.

         ### 3. Verify Claims and Relationships

         Before using a factual statement in the story, determine exactly what the available evidence establishes.

         Distinguish between:

         - **Established fact:** A statement directly supported by the Research as represented in Synthesis.
         - **Documented relationship:** A connection, sequence, or relationship between facts that the source explicitly establishes.
         - **Interpretation:** A meaning or explanation that goes beyond the directly stated facts.
         - **Unknown:** A matter the available evidence does not establish.

         Use established facts and documented relationships as the factual foundation of the story. Interpretations may inform editorial judgment, but must not be presented as established facts unless independently supported.

         #### Validate compound claims

         A statement may combine several individually supported facts while asserting an unsupported relationship between them.

         For every compound claim, check:

         1. Is each factual component supported by the input?
         2. Is the relationship between those components also supported?
         3. Does the statement preserve the original actors, scope, chronology, and level of certainty?
         4. Does the wording introduce a motivation, cause, intention, consequence, or conclusion that the evidence does not establish?

         If the individual facts are supported but their relationship is not, do not assert that relationship.

         For example, suppose the evidence establishes that an initial implementation supports Article only and separately states that the first implementation should remain small.

         You may report those facts independently. You must not conclude that Article was chosen specifically to validate the orchestration architecture unless the evidence explicitly establishes that reason.

         Likewise, if the evidence establishes that a project evolved from an AI agent toward an orchestrator, you may describe that documented evolution. You must not invent the technical problems, realization, motivation, or decision process that supposedly caused it.

         **A plausible explanation is not an established explanation. Two supported facts do not automatically support the relationship between them.**

         #### Preserve uncertainty

         - Do not invent motivations, reasons for decisions, causal relationships, results, metrics, comparisons, or outcomes.
         - Do not claim that an approach is better than an alternative unless the input provides supporting evidence.
         - Do not turn a possibility into a fact, a preference into a decision, or an intention into a completed implementation.
         - Do not turn a conceptual design into an implemented capability.
         - Do not turn an untested expectation into a demonstrated outcome.
         - Do not attribute an action or decision to a team, individual, or role unless the evidence supports that attribution.
         - Do not strengthen an uncertain chronology into a precise historical sequence.
         - Do not generalize beyond the source.

         When a claim is only partially supported, narrow it to the supported portion or omit it.

         Do not add uncertainty disclaimers to the published story merely because the source contains unknowns. Record only the limitations that materially constrain what Compose may claim.

         ### 4. Define a Compelling Story Progression

         Describe how the central idea can develop into a coherent, engaging story.

         Identify the most effective progression supported by the evidence. Where appropriate, consider:

         1. **Starting point:** The situation, assumption, question, or problem that makes the story worth exploring.
         2. **Key development:** The change, discovery, contrast, example, or insight that advances the story.
         3. **Significance:** What the reader can understand or take away from the development.

         These are editorial possibilities, not mandatory story beats. Use the progression that best serves the Selected Idea.

         - Prefer a meaningful development over a chronological inventory of events.
         - Identify a useful contrast, tension, concrete example, or surprising implication when the evidence supports it.
         - Ensure the story has a clear through-line and a meaningful payoff.
         - Select the strongest available detail that makes the central insight understandable or memorable.
         - Do not manufacture conflict, surprise, failure, realization, emotional reaction, or resolution to make the story more dramatic.
         - Do not invent an obstacle, turning point, lesson learned, or successful outcome.
         - Do not imply that an event caused another event merely because the sequence would make a more satisfying story.
         - Do not force the story into a beginning-development-conclusion structure when the evidence supports a different progression.

         The objective is compelling storytelling grounded in evidence, not a dramatic reconstruction of events the source does not establish.

         ### 5. Keep the Story Focused Without Making It Shallow

         - Build the brief around one central idea.
         - Prefer a compelling, coherent story over a comprehensive project summary.
         - Include secondary details when they materially strengthen the reader's understanding of the central idea.
         - Do not exclude useful context merely to make the story shorter.
         - Do not add secondary themes merely because they are interesting or relevant to the wider project.
         - Include product or implementation details only when they help explain, substantiate, or illustrate the central story.
         - Do not instruct Compose to explain the complete architecture, every workflow stage, the entire product vision, or the full development history unless the Selected Idea specifically concerns those subjects.
         - Do not require every detail identified as relevant in Synthesis to appear in the story.
         - Prefer a small number of meaningful, well-supported details over a catalogue of facts.

         A detail should earn its place by helping the reader understand, appreciate, or remember the central insight.

         ### 6. Define the Story's Meaning, Not Its Presentation

         Narrative is format-independent. Compose determines how the story is expressed in the requested content format.

         - Define the story's meaning, progression, evidence, and essential context independently of the eventual output format.
         - Do not assume that Compose will produce an article, LinkedIn post, newsletter, or any other specific format.
         - Do not prescribe platform-specific hooks, paragraph lengths, formatting conventions, calls to action, or content structures.
         - Do not optimize the story for the length or conventions of a particular publishing platform.
         - Give Compose the editorial substance it needs to adapt the story to the requested format without changing its meaning.

         Narrative should identify what makes the story worth telling. Compose should decide how best to present it.

         ### 7. Define the Brief, Not the Final Content

         - Do not write publication-ready paragraphs, an introduction, a conclusion, or a complete draft.
         - Do not prescribe the content of every paragraph.
         - Identify the central message, essential story progression, and most valuable supporting details.
         - Give Compose sufficient editorial direction to produce engaging content independently.
         - Keep the brief concise, specific, and useful.
         - Concision must not come at the expense of essential context or a meaningful story progression.

         ## Output Format

         ### Selected Story

         State the central story in 1–2 sentences.

         Preserve the original meaning of the Selected Idea and identify the main insight the content should communicate.

         The statement must describe the strongest defensible version of the story, not combine the central insight with every relevant product or implementation detail.

         ### Story Progression

         Provide 3–4 concise bullet points describing the essential progression of the story.

         Each point must advance the same central idea. Include the strongest supporting details where they materially improve the story.

         For every point:

         - Use only facts and relationships supported by the input.
         - Preserve uncertainty about motivations, causes, attribution, chronology, and outcomes.
         - Avoid turning contextual information into an additional central theme.
         - Identify a useful example or contrast when it helps make the story more compelling.
         - Do not prescribe platform-specific presentation.

         The points should provide an editorial direction, not a mandatory checklist of everything Compose must include.

         ### Critical Constraints

         List the essential factual boundaries and unsupported claims that Compose must avoid.

         Prioritize specific risks identified in the input, such as:

         - An unknown motivation that could be mistaken for an established reason.
         - A causal relationship that the source does not establish.
         - An attribution or historical sequence that remains uncertain.
         - An untested expectation that could be presented as a proven result.
         - A conceptual capability that could be mistaken for an implemented feature.

         State the boundary precisely enough for Compose to apply it.

         Do not list every Research Unknown. Include only constraints that materially affect the story.

         If no additional constraints are necessary, state that explicitly.

         ## Final Validation

         Before producing the output, verify that:

         ### Editorial alignment

         - The Selected Story preserves the original Selected Idea.
         - The Selected Idea, not the breadth of Synthesis, determines the story's focus.
         - The story direction is compelling, coherent, and supported by the available evidence.
         - Supporting details have been selected for their explanatory and narrative value, not merely their relevance.
         - Unnecessary project, product, and architecture details have been excluded without removing essential context.
         - The progression offers a meaningful development and payoff where the evidence supports them.

         ### Factual reasoning

         - Every factual claim is supported by the input.
         - Each compound claim has been checked for support for both its individual facts and the relationships it asserts.
         - No unsupported meaning, motivation, causality, attribution, chronology, or conclusion has been introduced.
         - Interpretations have not been silently promoted to established facts.
         - Uncertainty is preserved wherever it materially affects the claims Compose may make.
         - No unsupported explanation has been added merely to make the story more compelling.

         ### Downstream usability

         - Compose receives enough evidence and context to write a compelling piece of content.
         - Critical constraints identify the specific claims that must not be made.
         - The brief remains independent of the eventual output format.
         - Compose retains freedom to choose the presentation, structure, and style appropriate to the requested format.
         - The brief is an editorial direction, not a rigid outline or a disguised draft.

         If any check fails, revise the brief before returning it.

         **Priority rule:** Preserve the Selected Idea while developing the most compelling story the evidence supports. Select details for their explanatory and narrative value. Validate both individual facts and the relationships between them. Never invent a motivation, cause, event, or outcome to improve the story, and never sacrifice meaningful storytelling merely to make the brief shorter.

         Research and Synthesis provide the boundaries of what can be claimed. Within those boundaries, choose the story direction that gives Compose the best opportunity to produce clear, engaging, memorable content.
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