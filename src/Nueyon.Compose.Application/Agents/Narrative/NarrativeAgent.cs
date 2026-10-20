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
         # NARRATIVE USER PROMPT

         ## ROLE

         Create a concise editorial brief that turns the Selected Idea and Synthesis into one clear, coherent story direction for the Compose stage.

         You are defining **what the story is about and how it should develop**, not writing the final article or LinkedIn post.

         The Compose stage will decide how to express the story for the requested content format.

         ## INPUT

         ### Selected Idea

         **Title**
         {input.SelectedIdea.Title}

         **Description**
         {input.SelectedIdea.Description}

         **Audience**
         {input.SelectedIdea.Audience}

         **Rationale**
         {input.SelectedIdea.Rationale}

         **Evidence**
         {input.SelectedIdea.Evidence}

         ### Synthesis

         {input.Synthesis.Content}

         ## OBJECTIVE

         Define one focused story that:

         - Preserves the central subject and intended direction of the Selected Idea.
         - Uses the Synthesis to establish what can be stated and which supporting details are relevant.
         - Gives the reader enough context to understand the subject before presenting the central insight.
         - Develops the idea in a logical progression rather than presenting disconnected facts.
         - Gives Compose a clear foundation for producing engaging, natural, publishable content.

         The result must be a story brief, not a research summary, evidence inventory, source report, or finished piece of writing.

         ## INSTRUCTIONS

         ### 1. Preserve the Selected Idea

         The Selected Idea determines the subject and central direction of the story.

         Use the Synthesis to develop that idea, not to replace it with a different topic simply because other information is available.

         Keep the story focused on one central idea. Include supporting details only when they help the reader understand that idea.

         Do not introduce additional topics that compete with the main story.

         ### 2. Establish the Reader's Context

         Identify what the reader needs to understand before the central insight makes sense.

         Start the story direction with the relevant situation, subject, experience, problem, development, or observation supported by the inputs.

         Do not begin with an abstract conclusion that assumes the reader already knows the background.

         Do not invent a personal experience, project history, problem, conflict, or starting assumption merely to create a more dramatic opening.

         ### 3. Develop a Clear Story Progression

         Organize the story into a natural progression:

         1. **Context:** Establish the subject and the relevant background.
         2. **Development:** Explain the key detail, change, discovery, contrast, or relationship that makes the subject interesting, if supported by the inputs.
         3. **Central insight:** Make clear what the available material supports the reader in understanding.
         4. **Resolution:** Identify the natural endpoint of the story, without inventing an outcome, recommendation, lesson, or conclusion.

         These are conceptual stages, not mandatory sections in the final content. Use only the stages that fit the material.

         Do not force every story into a personal journey, problem-and-solution structure, or transformation narrative.

         ### 4. Use Evidence Selectively

         Select the most relevant information from the Synthesis to support the story.

         Prioritize details that explain the subject, establish necessary context, or help the reader follow the central idea.

         Do not reproduce the Synthesis in full. Do not include facts simply because they are available.

         Distinguish between:
         - What the input explicitly states.
         - What can reasonably be rephrased without changing its meaning.
         - What remains unknown or unsupported.

         Use the first two to shape the story. Treat the third as a boundary, not as content that must be discussed.

         ### 5. Preserve Meaning and Certainty

         The Selected Idea and Synthesis are the boundaries of the story's meaning.

         Do not invent or infer:
         - Facts, events, experiences, or outcomes.
         - Motivations, intentions, or reasons that are not explicitly supported.
         - Causal relationships that the inputs do not establish.
         - Broader trends, implications, lessons, or recommendations.
         - Claims about effectiveness, superiority, success, or impact without supporting evidence.
         - Actors, collaborators, teams, or collective ownership that the inputs do not establish.

         Preserve the original scope and level of certainty. A possibility must not become a fact, and a specific observation must not become a universal claim.

         If information is missing, do not fill the gap with a plausible explanation.

         ### 6. Preserve Point of View and Ownership

         Represent who did, observed, decided, experienced, or developed something accurately.

         Do not turn an individual's work into team work. Do not invent a first-person experience or attribute intentions to the author.

         Use first-person framing only when the inputs support the author's personal involvement and that perspective is relevant to the story.

         Avoid unnecessary references to a project, team, or source as if reporting what its documentation says. When the inputs support a direct statement, identify the actual subject and express the supported development directly.

         ### 7. Define the Story, Not the Writing Style

         Do not prescribe a finished opening sentence, rhetorical hook, paragraph structure, headline, or publication-specific style.

         Do not instruct Compose to quote the source material or reproduce particular phrases verbatim.

         Identify the central insight and the sequence of ideas that make it understandable. Compose is responsible for turning that direction into effective writing for the requested format.

         The story direction should work whether Compose produces an article or a LinkedIn post.

         ### 8. Avoid Research-Report Framing

         The story is intended for a reader, not an auditor reviewing the underlying material.

         Do not make the story about:
         - What the documentation records or fails to record.
         - What the research or synthesis says.
         - Which evidence is missing from the inputs.
         - A list of caveats included merely to demonstrate caution.
         - The process by which the source material was assembled.

         Use uncertainty and evidence limitations to control what can be claimed. Mention a limitation in the story direction only when it is itself materially relevant to the central subject and explicitly supported by the inputs.

         Do not manufacture a defensive disclaimer or a conclusion about what cannot be proven.

         ## OUTPUT FORMAT

         Return the editorial brief using exactly these sections:

         ### Selected Story
         State the central story in one concise paragraph. Identify the subject, the main development or insight, and why it is a coherent story direction for the intended audience. Do not add unsupported significance.

         ### Story Progression
         Provide 3–4 concise bullet points describing the logical progression of the story, from the context the reader needs to the central insight and its natural endpoint.

         Each bullet must contribute a distinct step. Avoid repetition and do not turn the bullets into instructions to report sources or limitations.

         ### Critical Constraints
         List only the specific factual boundaries that Compose must respect to tell this story accurately. Include this section only when there are meaningful constraints beyond the general requirement to preserve the inputs' meaning.

         Do not use this section to repeat the story, summarize missing information, or list generic warnings.

         ## FINAL VALIDATION

         Before returning the brief, verify that:

         - The story remains faithful to the Selected Idea.
         - Every substantive statement is supported by the inputs.
         - The reader receives sufficient context before the central insight.
         - The story has one clear focus and a logical progression.
         - The point of view, actors, and ownership are represented accurately.
         - Unknowns and limitations constrain the story without becoming unnecessary content.
         - The brief describes the story to tell, rather than how the source material was documented.
         - Compose has enough direction to write naturally without being forced to repeat source wording.

         Return only the editorial brief. Do not include commentary about your process or the quality of the inputs.
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