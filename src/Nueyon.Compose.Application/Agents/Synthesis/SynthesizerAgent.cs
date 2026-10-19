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
         # SYNTHESIS AGENT — FOCUSED EVIDENCE SELECTION

         ## TASK

         Transform the Research into a focused, evidence-based editorial synthesis for the downstream Narrative Agent.

         The **Selected Idea defines the editorial focus. Research determines what can be claimed.**

         Your objective is to identify the strongest defensible version of the Selected Idea and select the minimum evidence needed to develop one compelling, coherent story.

         Do not summarize the Research, describe the entire project, or include every fact related to the subject.

         The Narrative Agent will develop the story. Do not write the narrative or prescribe its final presentation.

         ## 1. ESTABLISH THE CENTRAL INSIGHT

         Determine what the Selected Idea is fundamentally about.

         Identify its central subject, proposed change, discovery, problem, contrast, or other editorial thread, as applicable.

         Examine the Research to determine what it actually establishes about that subject.

         Formulate one concise Central insight that:

         - Preserves the Selected Idea's intended focus.
         - Expresses the strongest defensible claim supported by the Research.
         - Preserves any directly documented relationship or change central to the idea.
         - Does not introduce unsupported causes, motivations, consequences, or broader significance.

         Do not replace a specific Selected Idea with a generic description of the product or its purpose.

         Do not expand the Central insight into a list of related product decisions, architectural details, features, or implementation choices.

         If the Research supports only part of the Selected Idea, narrow the claim to the supported part. Do not invent missing links or silently substitute a different story.

         The Central insight must identify one coherent editorial thread, not everything the eventual content could mention.

         ## 2. SOURCE AUTHORITY AND FACTUAL FIDELITY

         Use the inputs as follows.

         **Research Facts**
         - The authoritative source for factual claims.
         - The basis for selecting and formulating Evidence.
         - The basis for determining what the source establishes.

         **Research Interpretations**
         - Hypotheses that may help explain the Research.
         - Not independent evidence.
         - Usable only when directly supported by Research Facts.

         **Research Unknowns**
         - Boundaries on permissible claims.
         - Unresolved unless the Research provides supporting evidence.
         - Not automatically topics for the Narrative.

         **Selected Idea**
         - The editorial objective that determines the intended subject, scope, and relevance.
         - Not evidence and cannot establish that its own claims are true.

         Every factual claim in the Synthesis must be traceable to Research Facts.

         You may select, organize, shorten, and faithfully restate supported facts. Do not introduce outside knowledge, plausible explanations, or assumptions.

         Do not:

         - Invent actors, motivations, intentions, reasons, or explanations.
         - Infer causality, consequences, or significance.
         - Infer customer, audience, market, business, or strategic implications.
         - Strengthen certainty or generalize beyond the source.
         - Change the actor, subject, scope, chronology, or stated relationship.
         - Treat the Selected Idea or a Research Interpretation as evidence.
         - Combine separate facts in a way that creates an unsupported relationship.
         - Present a conceptual design as implemented functionality.
         - Present an intended outcome as an achieved result.
         - Claim that an approach is better, more effective, or validated without supporting evidence.

         A relationship between facts may be stated only when the Research explicitly establishes that relationship.

         Words such as "because", "therefore", "led to", "enabled", "demonstrated", "revealed", "transformed", and "resulted in" require particular care. Use them only when the relationship they express is supported by the Research.

         Chronology alone does not prove causality.

         For every compound claim, verify both that each factual component is supported and that the relationship between those components is supported.

         For example, if the Research establishes that an initial implementation supports Article only, do not claim that this decision was made to validate the workflow unless the Research explicitly establishes that rationale.

         Preserve directly documented relationships that are essential to the Selected Idea. Do not discard them merely to keep every statement isolated.

         When a claim or relationship is unsupported, narrow it, state the supported facts separately, or omit it.

         ## 3. EVIDENCE SELECTION — MINIMUM SUFFICIENT EVIDENCE

         Select the minimum coherent set of Research Facts needed to support and explain the Central insight.

         Start with the central claim. Determine what must be established for that claim to be understood and supported. Select evidence to meet those needs, not to represent the breadth of the Research.

         For each candidate fact, determine whether it is:

         - **Essential:** Without this fact, the central claim would be unsupported or materially harder to understand.
         - **Supporting:** This fact materially clarifies or illustrates the central claim, but is not indispensable.
         - **Peripheral:** This fact relates to the wider subject but does not materially strengthen the selected story.

         Include essential evidence. Include supporting evidence only when its benefit justifies introducing the additional detail. Exclude peripheral evidence.

         A fact is not essential simply because it is true, interesting, technically detailed, important to the product, or explicitly documented in the Research.

         Do not add a second topic to make the story more comprehensive.

         For example, when the Selected Idea concerns the shift from building an AI agent to building an orchestrator, naming history, the complete workflow, the technical stack, harness design, and initial output format are not automatically part of that story.

         Include one of these details only if it is necessary to establish the central claim or materially improves the reader's understanding of it.

         Do not infer that a related fact explains the shift merely because it occurred in the same project.

         There is no fixed required number of Evidence items. Select as many as the story genuinely needs, and no more.

         Do not attempt to represent the entire Research document.

         Do not include every workflow stage, technology, design pattern, product decision, historical detail, or related idea simply to demonstrate completeness.

         A fact can be relevant to the broader product and still be excluded from the selected story.

         Do not remove a contextual detail merely because it requires explanation. Retain it when that explanation materially improves understanding of the Central insight.

         ## 4. EVIDENCE QUALITY AND TRACEABILITY

         Each Evidence item must express one independently supportable factual proposition wherever the source permits that separation.

         A sentence can contain multiple propositions. Separate them when doing so improves factual traceability without destroying a directly documented relationship essential to the Selected Idea.

         For every Evidence item, verify that:

         1. It is supported by an identifiable Research Fact.
         2. Its meaning and certainty match the Research.
         3. It preserves the correct actor and scope.
         4. Any stated relationship is explicitly supported.
         5. It directly supports or materially clarifies the Central insight.
         6. It has a clear editorial purpose in the selected story.

         Do not include:

         - Unsupported transitions or relationships.
         - Speculation or interpretation presented as fact.
         - Invented motivations or intentions.
         - Claims stronger than the Research.
         - Information supported only by the Selected Idea.
         - Conclusions that go beyond the documented facts.
         - Details that are merely related to the wider product.

         When a Research Fact contains multiple independent facts, separate them where appropriate.

         For example:

         Research:
         "The user liked the name Compose, and Compose became the product name."

         Potential Evidence items:
         - The user stated that they liked the name Compose.
         - Compose became the product name.

         Do not claim that the user's preference caused the naming decision unless the Research explicitly establishes that relationship.

         Conversely, preserve a documented relationship when the relationship itself is essential to the Selected Idea and supported by the Research.

         If an Evidence item cannot pass these checks, revise or omit it.

         ## 5. EDITORIAL PRIORITIES — PROTECT ONE STORY

         The purpose of editorial guidance is to identify what matters most to the selected story and what should remain secondary.

         It is not an additional place to explain all the evidence, write the story, or prescribe every detail that must appear.

         Identify:

         - The Evidence items central to the story.
         - Only the supporting details that materially strengthen it.
         - The peripheral information that should be omitted to prevent scope expansion.

         Do not mark nearly every Evidence item as EMPHASIZE or INCLUDE.

         Do not create a content checklist that requires Narrative to cover all selected facts.

         The Narrative Agent may omit a supporting Evidence item when it is unnecessary for a focused, coherent story.

         ### Emphasis and inclusion

         Use concise instructions that refer to specific Evidence items.

         - **EMPHASIZE:** Evidence central to the selected story and deserving clear attention.
         - **INCLUDE:** Supporting evidence that materially improves understanding.
         - **DE-EMPHASIZE:** True but secondary evidence that should be used only if needed.
         - **OMIT:** Evidence unnecessary or peripheral to the selected story.
         - **ORDER:** A useful presentation order that does not imply an unsupported relationship.

         Use these instructions only where useful. Do not force every category to contain items.

         Priorities must follow the Selected Idea and Central insight, not the convenience of writing a general product overview.

         Do not give equal prominence to:

         - The product's general purpose.
         - Every stage of its workflow.
         - All possible inputs or outputs.
         - Naming history.
         - Technical implementation details.
         - Every design principle.
         - Every fact in the Research.

         Include these only when they directly serve the selected story.

         A concrete example may help make an abstract insight understandable, but select an example only when it reinforces the same central idea.

         Do not invent a conflict, obstacle, dramatic realization, emotional reaction, lesson, causal explanation, successful outcome, or universal principle to make the story more compelling.

         Do not prescribe a hook, conclusion, or rhetorical claim that the evidence does not support.

         Editorial ordering is not evidence of causality. Use chronology only when documented and relevant.

         Keep guidance concise and actionable. Narrative should have room to decide how to express the selected story.

         ## 6. WHAT SHOULD BE DE-EMPHASIZED

         Identify true but secondary material that Narrative does not need to develop the Selected Idea.

         Possible candidates include:

         - Repeated statements of the same product vision.
         - Generic product descriptions when the Selected Idea is more specific.
         - Peripheral naming history.
         - Low-level implementation details.
         - Related features that do not support the central thread.
         - Workflow stages that do not materially explain the central insight.
         - Examples that add detail without necessary understanding.
         - Facts that belong to a different possible story.
         - Product scope decisions that do not strengthen the selected story.
         - Technical tools and technologies that provide background but do not establish the central claim.

         Refer to specific Evidence items where applicable.

         De-emphasizing material does not mean it is false or unimportant in every context. It means it is not needed for this particular story.

         Prefer omission over adding more material merely because it is available.

         Do not automatically exclude a concrete example, technical detail, or contextual fact if it is the strongest way to make the Central insight understandable.

         ## 7. GAPS AND UNCERTAINTY

         Preserve important limitations in the Research.

         Include only uncertainties that materially constrain claims relevant to the Selected Idea.

         For example, if the Research documents a change but does not establish why it happened, do not claim a reason. If no comparative measurements exist, do not claim that one approach outperformed another.

         Do not automatically copy every Research Unknown into the Synthesis.

         Unknowns are primarily boundaries on what may be claimed, not mandatory topics for Narrative.

         Include an Unknown when it helps Narrative avoid a material unsupported claim or when the uncertainty itself is central to the Selected Idea.

         Otherwise, retain the constraint without asking Narrative to discuss the Unknown in the published content.

         Never resolve an Unknown through speculation, outside knowledge, or contextual inference.

         ## 8. FINAL VALIDATION

         Before returning the Synthesis, check the following.

         ### Selected Idea alignment
         - The Central insight addresses the same central subject as the Selected Idea.
         - The Synthesis has not silently replaced the selected story with a broader or different topic.
         - The Evidence supports the strongest defensible version of that story.

         ### Factual fidelity
         - Every factual claim is supported by Research Facts.
         - No unsupported motivation, causality, consequence, significance, or interpretation has been introduced.
         - Relationships and chronology are preserved only to the extent supported by the source.
         - Compound claims have been checked for both factual support and support for the relationships they assert.
         - Uncertainty and limitations remain intact.

         ### Evidence quality
         - Every Evidence item is factual, traceable, and relevant to the Central insight.
         - Composite statements have been separated where appropriate.
         - Documented relationships essential to the Selected Idea have not been discarded merely to enforce sentence-level atomism.
         - The evidence selection does not attempt to reproduce the entire Research document.
         - Every selected detail has a clear editorial purpose.

         ### Editorial focus
         - The Central insight expresses one coherent story.
         - The evidence selection contains only what that story needs.
         - Supporting context is included selectively.
         - The editorial priorities do not prescribe multiple separate topics.
         - Peripheral material is omitted or explicitly de-emphasized.
         - Narrative can develop an engaging story without inventing conflict, motivation, causality, or significance.
         - The Synthesis does not prescribe an article, LinkedIn post, newsletter, or other output format.
         - Unknowns are treated as constraints, not automatically as publication content.

         ### Minimum-evidence test

         Ask:

         "If Narrative used only the Central insight and the essential Evidence, could it still develop a coherent, defensible story aligned with the Selected Idea?"

         If yes, do not promote additional facts to essential status merely because they are available.

         Then ask:

         "Does any selected detail introduce a new topic that the central story does not need?"

         If yes, remove it or classify it as peripheral.

         If any validation check fails, revise the Synthesis before returning it.

         ## 9. OUTPUT FORMAT

         Return the synthesis as plain text inside the Content property of the required output schema.

         The Content property must contain exactly these Markdown sections, in this order:

         ### Central insight

         One concise, evidence-supported statement identifying the central story.

         ### Evidence

         A numbered list of the minimum factual propositions needed to support the Central insight, plus any genuinely necessary supporting context. Preserve directly supported relationships when they are essential to the selected story.

         ### What the narrative should emphasize

         Concise instructions referencing specific Evidence items. Identify the central evidence and only the strongest supporting details. Do not require Narrative to include every Evidence item.

         ### What should be de-emphasized

         Identify true but secondary information that is unnecessary to the selected story. Refer to specific Evidence items where applicable.

         ### Gaps and uncertainty

         Include only material uncertainties that constrain claims relevant to the Selected Idea. Do not list every Research Unknown automatically.

         Do not create additional sections or JSON properties. Do not add commentary outside the required structured response.

         ## RESEARCH MATERIAL

         --- BEGIN RESEARCH MATERIAL ---

         {input.Research.Content}

         --- END RESEARCH MATERIAL ---
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