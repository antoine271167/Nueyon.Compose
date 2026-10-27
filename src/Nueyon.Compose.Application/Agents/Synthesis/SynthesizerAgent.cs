using System.Diagnostics;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Synthesis;

/// <summary>
///     An agent that synthesizes research into a concise editorial brief.
/// </summary>
public sealed class SynthesizerAgent(
    AIAgent agent,
    ILogger<SynthesizerAgent> logger)
    : IAgent<SynthesisInput, SynthesisResult>
{
    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));
    private readonly ILogger<SynthesizerAgent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<SynthesisResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        SynthesisInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        const string agentName = "SynthesizerAgent";
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
            var synthesis = ParseSynthesisFromJson(responseText);

            stopwatch.Stop();

            _logger.LogInformation(
                "Agent {AgentName} invocation completed in {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            return synthesis;
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
                "Agent {AgentName} invocation failed after {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            throw;
        }
    }

    /// <summary>
    ///     Gets the system instructions used to configure this agent's underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() => SynthesizerAgentInstructions.GetSystemInstructions();

    private static string CreateUserMessage(SynthesisInput input) =>
        $"""
         # SYNTHESIS AGENT — EDITORIAL FOCUS AND EVIDENCE SELECTION

         ## TASK

         Transform the Research into a focused, evidence-based editorial synthesis for the downstream Narrative Agent.

         The **Selected Idea determines the intended story. Research determines what can truthfully be said.**

         Your responsibilities are to:

         1. Identify the central story that the Selected Idea and Research can support.
         2. Select the minimum evidence needed to establish and explain that story.
         3. Give Narrative concise guidance about what matters most and what should remain secondary.

         Do not summarize the entire Research, describe the whole product, write the story, or prescribe the final content format.

         **Optimize for one coherent story, not comprehensive coverage.**

         ## 1. ESTABLISH THE CENTRAL STORY

         Start with the Selected Idea. Identify its central subject and the specific claim, change, discovery, contrast, problem, or development that makes it worth exploring.

         Then examine the Research to determine what it actually establishes about that subject.

         Write one concise **Central insight** that:

         - Preserves the Selected Idea's central subject and editorial intent.
         - States the strongest defensible claim supported by the Research.
         - Preserves directly documented relationships essential to the story.
         - Does not introduce unsupported explanations, motivations, consequences, benefits, or broader implications.

         ### The Selected Idea is not evidence

         The Selected Idea may express a strong editorial claim. Do not assume that its wording is factually justified.

         If it says an approach is "best," "better," "more effective," or otherwise superior, check whether the Research establishes that comparison. If it does not, narrow the claim to the documented principle, intention, or direction without changing the subject of the story.

         For example, evidence that a company considers orchestration important supports describing orchestration as a product principle. It does not, by itself, establish that orchestration produces better AI applications.

         Do not silently replace an unsupported claim with a different story. Preserve the intended subject while narrowing what can be asserted about it.

         ### Keep one editorial thread

         The Central insight must identify one coherent story, not a collection of related themes.

         Do not expand a specific Selected Idea into a general product overview, an architectural manifesto, or a list of related product decisions.

         The Central insight is the anchor for all subsequent decisions. Evidence and editorial guidance must serve it.

         ## 2. SOURCE AUTHORITY AND FACTUAL FIDELITY

         Use each input according to its role.

         **Selected Idea**
         - Defines the intended subject, audience, and editorial purpose.
         - Does not establish the truth of its own claims.

         **Research Facts**
         - Are the authoritative basis for factual claims.
         - Determine what the source establishes and what Evidence may contain.

         **Research Interpretations**
         - May help identify a possible reading of the facts.
         - Are not independent evidence.
         - May be used only when their substance is directly supported by Research Facts.

         **Research Unknowns**
         - Define boundaries on permissible claims.
         - Are not automatically topics for the story.

         Every factual claim in the Synthesis must be traceable to Research Facts.

         You may select, organize, shorten, and faithfully restate supported facts. Do not introduce outside knowledge, assumptions, or plausible explanations.

         Never:
         - Invent actors, motivations, intentions, reasons, or explanations.
         - Infer causality, consequences, benefits, or significance.
         - Generalize beyond the source.
         - Strengthen the source's certainty.
         - Change the actor, scope, chronology, or documented relationship.
         - Combine separate facts in a way that creates an unsupported relationship.
         - Present a conceptual design as implemented functionality.
         - Present an intended capability as a demonstrated outcome.
         - Claim that an approach is superior, effective, or validated without supporting evidence.

         Chronology alone does not establish causality. A relationship between facts may be stated only to the extent that the Research supports it.

         For compound claims, verify both the individual facts and the relationship between them. Pay particular attention to words such as "because," "therefore," "led to," "enabled," "resulted in," "improved," and "demonstrated."

         When support is insufficient, narrow the claim, state the facts separately, or omit it.

         Preserve directly documented relationships when they are essential to the Selected Idea. Do not remove meaningful relationships merely to make every Evidence item an isolated fact.

         ## 3. SELECT THE MINIMUM SUFFICIENT EVIDENCE

         Select only the Research Facts needed to establish and explain the Central insight.

         Assess each candidate fact using three categories:

         - **Essential:** Without this fact, the central claim would be unsupported or materially harder to understand.
         - **Supporting:** This fact meaningfully clarifies or illustrates the central claim, but the story can work without it.
         - **Peripheral:** This fact relates to the wider subject but does not materially strengthen this story.

         Include essential evidence. Include supporting evidence only when it adds meaningful understanding. Exclude peripheral evidence from the selected story.

         A fact is not essential merely because it is true, interesting, technically detailed, or important to the wider product.

         There is no required number of Evidence items. Select as many as the story needs, and no more.

         ### Choose evidence for its editorial function

         For every proposed Evidence item, ask:

         1. What does this fact establish?
         2. Why does the reader need to know it to understand the Central insight?
         3. Does it clarify the same story, or introduce another topic?
         4. Would removing it materially weaken the story?

         If a fact does not establish the central claim or materially improve understanding, omit it.

         A fact may be important to the overall product and still be unnecessary for this story.

         For example, when the Selected Idea concerns a product's orchestration principle, naming history, every workflow stage, the technical stack, future products, and the full roadmap are not automatically relevant. Include one of these details only if it is needed to explain the central idea.

         Do not infer that one event explains another merely because both occurred in the same project.

         Prefer omission over completeness. However, retain a concrete example when it is genuinely necessary to make an abstract idea understandable.

         ## 4. ENSURE EVIDENCE QUALITY AND TRACEABILITY

         The Evidence section must contain factual propositions, not narrative instructions, opinions, or conclusions that exceed the Research.

         For each Evidence item, verify that:

         1. It is supported by an identifiable Research Fact.
         2. Its meaning and certainty match the source.
         3. It preserves the correct actor and scope.
         4. Any relationship it asserts is supported.
         5. It directly supports or materially clarifies the Central insight.
         6. It has a clear editorial purpose in this story.

         Separate independent propositions when doing so improves traceability. Preserve a documented relationship when that relationship is essential to the Selected Idea.

         For example, if the Research states that the user liked the name Compose and that Compose became the product name, these can be recorded as separate facts. Do not claim that the preference caused the naming decision unless the Research establishes that relationship.

         Do not include speculation presented as fact, unsupported transitions, claims supported only by the Selected Idea, conclusions stronger than the evidence, or facts relevant only to a different possible story.

         Revise or omit any Evidence item that fails these checks.

         ## 5. GIVE NARRATIVE EDITORIAL GUIDANCE, NOT A CONTENT CHECKLIST

         The Central insight defines the story. Evidence establishes what can be said. Editorial guidance protects the focus.

         These responsibilities must remain distinct.

         ### Central insight

         State the one main idea the story should communicate. Keep it concise and specific.

         Do not turn it into a summary of the entire project, a list of product capabilities, a technical architecture description, a recommendation for the reader, or a claim about unsupported benefits.

         ### Evidence

         Present the minimum factual propositions needed to support the Central insight. Identify at most one concrete example when it materially improves understanding.

         Do not provide several examples merely to demonstrate completeness.

         ### What the narrative should emphasize

         Identify:

         - The one or two Evidence items that establish the central point.
         - At most one optional supporting detail or example that materially improves understanding.
         - Any essential distinction or documented relationship that must be preserved.

         Refer to specific Evidence items where practical.

         Do not require Narrative to include every Evidence item. Do not turn this section into a paragraph-by-paragraph outline or a checklist of technical concepts.

         Narrative may omit optional evidence when it is unnecessary for a focused, coherent story.

         ### What should be de-emphasized

         Identify true but secondary information that is unnecessary for the selected story. Refer to specific Evidence items where possible.

         Potential examples include naming history, additional workflow stages, related features, technical implementation details, broader product plans, and roadmap items.

         Do not force a broader company or roadmap narrative into a story focused on one product insight.

         Do not force this section to contain items when there is nothing significant to de-emphasize.

         ### Explain the subject without turning it into advice

         The story may be relevant to the Selected Idea's audience, but relevance does not automatically justify giving that audience advice.

         Distinguish between:

         - **Describing:** What Nuëyon believes, is building, intends to offer, or plans to explore.
         - **Interpreting:** What the documented facts mean for the selected story.
         - **Recommending:** What the reader should do or believe.

         Use description as the default. Interpretation is permitted only when directly supported by the Research. Do not introduce recommendations unless the Selected Idea explicitly calls for advice and the Research supports the recommendation.

         Do not turn Nuëyon's product principles into universal rules for building AI products. Do not infer benefits such as repeatability, reliability, productivity, or better results from the existence of a staged workflow.

         The purpose of the story is to explain the selected subject, not to persuade the audience that Nuëyon's approach is superior.

         In the editorial guidance, explicitly state when the story should explain Nuëyon's own product direction rather than prescribe an approach for the reader.

         ### Editorial restraint

         Do not instruct Narrative to:
         - Cover every stage of a workflow.
         - Explain the complete product architecture.
         - Compare approaches that the Research does not compare.
         - Derive universal lessons or recommendations for the audience.
         - Invent a problem, conflict, motivation, dramatic realization, or successful outcome.
         - End with a prescribed benefit, lesson, or implication.

         The purpose of editorial guidance is to give Narrative a clear direction, not to write the story in advance.

         ## 6. HANDLE UNCERTAINTY WITHOUT MAKING IT THE STORY

         The Synthesis must establish what can be said, not invent explanations to make the story sound more complete.

         Distinguish clearly between:

         - A product principle and a proven general rule.
         - A design intention and an implemented feature.
         - An intended capability and a demonstrated outcome.
         - A proposed benefit and an observed benefit.
         - A product's stated positioning and demonstrated market differentiation.

         Do not turn an individual product-development experience into a universal principle unless the Research supports that generalization.

         Include a Research Unknown only when it materially constrains a claim relevant to the Selected Idea or when the uncertainty itself is central to the story.

         Otherwise, use the Unknown to constrain the synthesis without asking Narrative to discuss it in the published content.

         Unknowns are boundaries on permissible claims, not mandatory publication content.

         ## 7. FINAL VALIDATION

         Before returning the Synthesis, verify the following.

         **Editorial focus**
         - The Central insight addresses the Selected Idea directly.
         - One coherent story is identifiable.
         - The Selected Idea has not been strengthened beyond the Research.
         - Every Evidence item serves the central story.
         - Optional supporting details have not become mandatory coverage.

         **Factual fidelity**
         - Every factual claim is supported by Research Facts.
         - Actors, scope, chronology, certainty, and relationships are preserved.
         - Unsupported motivations, causes, benefits, and consequences have been excluded.
         - Conceptual designs, intended capabilities, implemented features, and demonstrated outcomes are not conflated.

         **Evidence quality**
         - Every Evidence item is traceable and relevant.
         - Unsupported relationships between facts have been avoided.
         - Duplicate and unnecessary details have been removed.
         - The evidence supports the story without reproducing the entire Research.

         **Downstream freedom**
         - Narrative knows what the story is about and which facts matter most.
         - Narrative can develop a coherent story without including every Evidence item.
         - Editorial guidance does not prescribe an unsupported lesson, recommendation, or ending.
         - The synthesis leaves the actual storytelling and final presentation to Narrative and Compose.

         Apply these two final tests:

         **Minimum-evidence test:** If Narrative used only the Central insight and essential Evidence, could it develop a coherent, defensible story aligned with the Selected Idea?

         **Scope test:** Does any selected detail introduce a second topic that the central story does not need?

         If the minimum-evidence test passes, do not add more evidence merely because it is available. If the scope test reveals an unnecessary topic, omit it or identify it as secondary.

         Revise the Synthesis if either test fails.

         ## 8. OUTPUT FORMAT

         Return the Synthesis as plain text inside the `Content` property of the required output schema.

         The `Content` property must contain exactly these Markdown sections, in this order:

         ### Central insight

         One concise, evidence-supported statement identifying the central story.

         ### Evidence

         A numbered list of the minimum factual propositions needed to establish the Central insight. Include supporting facts only when they materially improve understanding. Preserve documented relationships essential to the Selected Idea.

         ### What the narrative should emphasize

         Concise editorial guidance identifying the central evidence and, if useful, at most one supporting detail or example. Do not require Narrative to include every Evidence item.

         ### What should be de-emphasized

         Identify true but secondary information that is unnecessary for the selected story. Refer to specific Evidence items where applicable. Do not force content into this section when there is nothing significant to de-emphasize.

         ### Gaps and uncertainty

         Include only material uncertainties that constrain claims relevant to the Selected Idea. Do not list every Research Unknown automatically.

         Do not create additional sections or JSON properties. Do not add commentary outside the required structured response.

         ## SELECTED IDEA

         **Title:** {input.SelectedIdea.Title}

         **Description:** {input.SelectedIdea.Description}

         **Audience:** {input.SelectedIdea.Audience}

         **Rationale:** {input.SelectedIdea.Rationale}

         **Evidence:** {input.SelectedIdea.Evidence}

         ## RESEARCH MATERIAL

         --- BEGIN RESEARCH MATERIAL ---

         {input.Research.Content}

         --- END RESEARCH MATERIAL---
         """;

    private static ChatClientAgentRunOptions CreateAgentRunOptions()
    {
        var responseFormat = ChatResponseFormat.ForJsonSchema<SynthesisResult>(
            null,
            nameof(SynthesisResult));

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

    private static SynthesisResult ParseSynthesisFromJson(string json)
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

            var response = JsonSerializer.Deserialize<SynthesisResult>(
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
                    "Synthesis response content is empty.");
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