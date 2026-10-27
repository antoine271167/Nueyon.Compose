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

         Create a concise editorial brief that defines one clear, coherent story for the Compose stage.

         Identify what the story is about, what the reader needs to understand, and how the central idea should develop.

         You are a narrative structurer, not a finished-content writer. Compose will turn your story direction into publishable content in the requested format.

         Your task is to identify and structure the story supported by the inputs—not to summarize the source material or invent a more dramatic story.

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

         - Preserves the subject and central direction of the Selected Idea.
         - Uses the Synthesis to establish what can truthfully be said.
         - Gives the reader enough context to understand the central idea.
         - Develops the idea through a logical progression of relevant details.
         - Identifies the most important supported development or observation, when one exists.
         - Ends at a natural, supported endpoint.
         - Gives Compose a clear foundation for engaging, natural, publishable content.

         The result must be a story brief, not a research summary, evidence inventory, technical specification, source report, or finished piece of writing.

         ## INSTRUCTIONS

         ### 1. Preserve the Selected Idea

         The Selected Idea determines the subject and central direction of the story.

         Use the Synthesis to develop that idea, not to replace it with a different topic simply because other information is available.

         Keep the story focused on one central idea. Include supporting details only when they help explain or develop that idea.

         Do not turn the story into a general overview of the product, project, technology, or subject.

         ### 2. Identify the Actual Story

         Determine the most important development, contrast, observation, or relationship explicitly supported by the inputs.

         Ask:

         - What is the central subject?
         - What does the material actually establish?
         - What changed or became clearer, if anything?
         - What background does the reader need to understand the central idea?
         - Which details make the central idea concrete?
         - What is the natural endpoint supported by the material?

         Not every story contains a decision, discovery, problem, turning point, or personal transformation.

         A documented change does not automatically establish who decided to make it, why it happened, or what benefits resulted.

         Do not manufacture a decision, realization, conflict, motivation, or lesson to make the story more interesting.

         When the material supports a straightforward explanation rather than a dramatic story, use the straightforward explanation.

         ### 3. Establish the Reader's Context

         Identify the background the reader needs before the central idea will make sense.

         Start with the relevant subject, situation, or development supported by the inputs.

         Do not begin with an abstract conclusion that assumes the reader already knows the background.

         Do not invent a personal experience, project history, problem, conflict, or starting assumption.

         Use the audience specified in the Selected Idea to judge which background details are necessary and which can be omitted.

         ### 4. Preserve Individual Ownership

         Represent who did, observed, decided, experienced, or developed something accurately.

         **Never invent a team, collaborators, organizational involvement, or collective decision-making.**

         The existence of a product, project, architecture, workflow, or specification does not establish that a team created it or made decisions about it.

         Apply these rules:

         - When the inputs establish the author's personal actions, preserve that individual perspective.
         - When describing the product's design, describe the design directly rather than inventing who decided to create it.
         - When the source establishes that something changed but not who caused or decided the change, describe only the supported change.
         - When ownership is unknown, leave it unspecified.
         - Do not turn individual work into collective work.
         - Do not replace an unsupported "team" with an equally unsupported "project," "organization," or "developers" as the decision-maker.

         For example, if supported by the inputs:

         - Personal action: "I set out to build an AI agent."
         - Product design: "The design centers on an orchestrator that coordinates specialized agents through distinct stages."
         - Unsupported attribution: "The team decided to replace a single agent with an orchestration layer."

         Do not force first-person language into every sentence. Preserve individual ownership where relevant, and describe documented product characteristics directly elsewhere.

         Do not invent collaboration, shared decisions, feelings, motivations, or personal experiences.

         ### 5. Build a Focused Story Progression

         Organize the story around three conceptual steps:

         1. **Context:** Establish the subject and the background the reader needs.
         2. **Development:** Present the key supported development or observation and the details that make it concrete.
         3. **Endpoint:** Bring the reader to the central insight or natural conclusion supported by the inputs.

         These steps describe the story's logical progression. They are not mandatory sections of the finished content.

         Each step must contribute something new. Avoid repeating the same insight in different words.

         Do not force a problem-and-solution structure, personal transformation, or dramatic narrative arc.

         Do not invent a causal connection to bridge two facts that the source merely presents together.

         If the source does not support a meaningful progression, use the clearest and most conservative structure it supports.

         ### 6. Keep the Central Story Above the Details

         The central idea must remain more important than the details used to explain it.

         Select only the evidence and context that help the reader understand the story.

         Technical details, architecture, processes, components, and implementation choices are supporting material unless the Selected Idea makes one of them the central subject.

         When a technical concept is relevant:

         - Explain its role in the central story.
         - Include only the detail necessary to understand that role.
         - Avoid turning one concept into a catalogue of components, stages, or features.
         - Do not infer technical behavior from a conceptual description.
         - Do not claim that an intended design has been implemented unless the inputs establish implementation.

         **For this Selected Idea, the central story is the development from setting out to build an AI agent toward designing an orchestrator that coordinates specialized agents.**

         Use the staged workflow to explain what orchestration means. Mention the harness concept only if it materially clarifies the central development.

         Do not give the workflow, harness, stage boundaries, and LLM-wrapper distinction equal prominence. They are supporting details, not separate central ideas.

         Prefer a simple explanation of the architectural direction over an inventory of architectural concepts.

         Do not reproduce the Synthesis in full. Do not include facts simply because they are available.

         ### 7. Preserve Meaning, Certainty, and Causality

         The Synthesis defines the boundary of the story's factual meaning.

         Do not invent or infer:

         - Facts, events, experiences, or outcomes.
         - Actors, collaborators, teams, or collective ownership.
         - Motivations, intentions, or reasons that are not established.
         - Causal relationships that the inputs do not establish.
         - Benefits, effectiveness, superiority, success, or impact without supporting evidence.
         - Broader trends, implications, lessons, or recommendations.
         - Technical implementation details that are not established.

         Preserve the original scope and certainty. A possibility must not become a fact, and a specific observation must not become a universal claim.

         Chronological sequence does not establish causality. A change does not establish why it happened. A design does not establish a realized benefit.

         Do not turn:

         - "X happened" into "X happened because Y."
         - "X changed" into "X changed because of Y."
         - "X was designed" into "X was successfully implemented."
         - "X exists" into "X improves performance."
         - "A direction emerged" into "a team formally decided to adopt it."

         Make these stronger statements only when the Synthesis explicitly supports them.

         Distinguish between a design concept, an intended capability, an implemented feature, and a demonstrated outcome. The inputs must establish which level is justified.

         If information is missing, leave the gap unresolved. Do not fill it with a plausible explanation.

         ### 8. Keep Source Analysis Out of the Story

         The story is intended for a reader, not an auditor reviewing the underlying material.

         Use the Synthesis to determine what can be said, but do not make the story about the Synthesis or the process of analyzing evidence.

         Do not frame the story around:

         - What documentation records or fails to record.
         - What research or synthesis states.
         - Which evidence is missing.
         - What cannot be proven.
         - How the source material was assembled.
         - Whether the inputs contain sufficient evidence for claims that are not central to the story.

         Express supported developments directly.

         For example, if the inputs establish that the author's work moved from building an AI agent toward an orchestration-focused design, describe that development directly. Do not characterize it as a documented shift or an account of what the project stated.

         Unknowns and limitations are boundaries on what may be claimed, not automatic subjects for the story.

         Do not make missing implementation details, performance measurements, user validation, or decision-making history part of the story unless that absence is itself central to the Selected Idea.

         Do not require the story to end with a list of what remains unknown or unproven. If the story can be told accurately without mentioning a limitation, omit it from the story direction.

         ### 9. Define the Story, Not the Writing Style

         Do not prescribe a finished opening sentence, rhetorical hook, headline, paragraph structure, or publication-specific style.

         Do not instruct Compose to reproduce memorable source phrases or use direct quotations.

         Identify the central idea and the logical progression that makes it understandable. Compose is responsible for expressing that direction in effective writing for the requested format.

         The story direction must work for both Article and LinkedIn formats.

         Express the meaning naturally rather than repeating source phrases simply because they appear in the Synthesis. Preserve the substance, not the wording.

         ### 10. Define a Natural Endpoint

         Identify where the central idea reaches a natural conclusion.

         The endpoint should complete the story's progression without introducing a new topic or unsupported claim.

         It may be a central insight, a supported description of the resulting direction, or another conclusion that follows from the material.

         Do not require the story to end with:

         - A list of technical decisions or components.
         - A summary of every supporting detail.
         - A lesson or recommendation not established by the inputs.
         - A discussion of missing evidence or implementation details.
         - A claim that the change made something easier, better, or more effective without evidence.
         - A disclaimer about what cannot be concluded.

         Do not invent an outcome simply to provide a satisfying ending.

         For this story, the endpoint may simply clarify the product's orchestration-focused direction. Do not add claims about what this direction subsequently changed, enabled, improved, or prioritized unless the inputs establish those claims.

         A concise, accurate endpoint is better than a more dramatic conclusion that the inputs cannot support.

         ## OUTPUT FORMAT

         Return the editorial brief using the following structure.

         ### Selected Story

         Write one concise paragraph defining the story.

         Identify:
         - The subject and central idea.
         - The key supported development or observation, when one exists.
         - The main point the reader should understand.

         Describe the underlying subject directly. Preserve individual ownership when supported. Do not invent a team, decision-maker, motivation, causal explanation, benefit, or outcome.

         Do not include a list of technical components or a discussion of evidence gaps.

         ### Story Progression

         Provide exactly three concise bullet points:

         - **Context:** What the reader needs to understand first.
         - **Development:** The key supported development and the relevant details that make it concrete.
         - **Endpoint:** The central insight or natural conclusion supported by the inputs.

         Each bullet must add a distinct step to the story.

         Keep technical details subordinate to the central idea. Do not turn the progression into a list of every fact, feature, workflow stage, or architectural component.

         Do not instruct Compose to report documentation, reproduce source wording, or end with limitations.

         ### Critical Constraints

         Include this section only when specific factual boundaries need to be communicated to Compose.

         List concise instructions about what must not be claimed or inferred. These constraints guide the writing; they are not story content.

         Useful constraints may include:
         - Do not attribute a decision to a team when no team is established.
         - Do not invent why a change occurred.
         - Do not claim benefits or comparative performance without evidence.
         - Do not claim implementation details that the inputs do not establish.

         Do not repeat the story or provide a general list of unknowns. Do not instruct Compose to mention missing evidence, undocumented implementation details, unverified outcomes, or unknown motivations in the finished content.

         If no specific constraint needs to be highlighted, omit this section.

         ## FINAL VALIDATION

         Before returning the brief, verify that:

         - The story preserves the subject and central direction of the Selected Idea.
         - Every substantive statement is supported by the Synthesis.
         - The reader receives enough context before the central idea.
         - The story has one clear focus and a logical progression.
         - Supporting details explain the central idea instead of competing with it.
         - Technical concepts are included only when they help develop the story.
         - The workflow and harness remain subordinate to the central story unless they are themselves central to the Selected Idea.
         - Individual ownership and point of view are preserved when supported.
         - No team, collaborator, decision-maker, motivation, cause, benefit, or outcome has been invented.
         - Design intentions are not presented as implemented or demonstrated capabilities.
         - Source wording has not been repeated unnecessarily.
         - Unknowns constrain claims without becoming unnecessary story content.
         - The story describes the underlying subject directly, not what documentation or research says about it.
         - The endpoint follows from the material rather than acting as a disclaimer or catalogue of decisions.
         - Compose has enough direction to write naturally without reproducing source wording.

         If a coherent narrative requires an unsupported assumption, choose the narrower supported structure.

         If a detail is not necessary to communicate the central thread, omit it unless doing so would distort the meaning.

         Return only the editorial brief in the required output structure. Do not include commentary about your process or the quality of the inputs.
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