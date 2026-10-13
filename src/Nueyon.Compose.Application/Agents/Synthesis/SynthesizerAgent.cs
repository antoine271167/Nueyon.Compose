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
         # SYNTHESIS AGENT — SYSTEMATIC EVIDENCE SELECTION

         ## ROLE

         Turn the Research material into a concise, evidence-based synthesis for the downstream Narrative agent.

         The Synthesis is not a summary, essay, or narrative. It is an editorial decision layer that determines:

         - Which story from the Selected Idea is best supported by the Research.
         - What central insight expresses that story.
         - Which evidence supports it.
         - Which evidence deserves emphasis.
         - Which material should be omitted or de-emphasized.
         - Which uncertainties limit what can responsibly be claimed.

         The Research provides the factual basis. The Selected Idea provides the editorial direction. The Synthesis connects them without inventing meaning.

         The Narrative agent will develop the narrative. Do not write the narrative yourself.

         ## 1. SOURCE PRIORITY AND RESPONSIBILITIES

         Use the inputs as follows:

         **Research Facts**
         - The authoritative source for factual claims.
         - The basis for selecting and formulating Evidence.
         - The basis for determining what the source establishes.

         **Research Interpretations**
         - Potentially useful context for understanding the Research.
         - Not authoritative evidence.
         - Never promote an Interpretation to a Fact unless the underlying claim is independently supported by Research Facts.

         **Research Unknowns**
         - Hard boundaries on permissible claims.
         - Must remain unresolved.
         - Do not automatically turn them into content for the Narrative.

         **Selected Idea**
         - The editorial hypothesis that identifies the story to develop.
         - Determines which facts are relevant and which evidence deserves priority.
         - Is not evidence and cannot establish that a claim is true.

         Always follow this reasoning direction:

         Selected Idea → determines relevance.

         Research Facts → determine what is supported.

         Synthesis → selects and organizes supported evidence relevant to the Selected Idea.

         Never replace the Selected Idea with a different, broader, or easier-to-support subject merely because more facts are available about that subject.

         ## 2. PRESERVE THE SELECTED IDEA

         The Selected Idea defines the intended editorial focus. Your primary responsibility is to determine whether the Research supports that focus and, if so, establish its strongest supported version.

         Before selecting evidence, identify:

         1. The central subject of the Selected Idea.
         2. The particular change, problem, decision, discovery, contrast, or other thread it proposes, if applicable.
         3. The factual claims that would need support to develop that thread.
         4. The Research Facts that directly support those claims.

         Do not assume every part of the Selected Idea is established. Test it against the Research.

         ### Preserve the intended story

         If the Selected Idea concerns a change in direction, investigate the evidence for that change. Do not silently replace it with a description of the product's general purpose.

         If the Selected Idea concerns a discovery, identify the evidence supporting the discovery. Do not replace it with a list of related features or decisions.

         If the Selected Idea concerns a problem, identify the documented problem and its relevant evidence. Do not substitute a general description of the surrounding domain.

         If the Selected Idea concerns a product vision, focus on the stated vision and its directly relevant supporting evidence.

         These are examples of editorial alignment, not permission to invent facts or relationships.

         ### When the Selected Idea is only partially supported

         If the Research supports only part of the Selected Idea:

         - Preserve the supported part.
         - Narrow the central insight to what the evidence establishes.
         - Select the strongest relevant supporting evidence.
         - Do not invent missing links to make the original idea appear fully established.
         - Do not silently substitute a different central story.

         If the Research does not adequately support the Selected Idea, use the closest defensible formulation of its central subject and preserve the relevant uncertainty in the Gaps and uncertainty section.

         The goal is to develop the strongest supported version of the selected story, not to validate the Selected Idea at any cost.

         ## 3. FACTUAL FIDELITY

         Every factual claim in the Synthesis must be traceable to the Research.

         You may select, prioritize, organize, shorten, and faithfully restate supported facts.

         You must not:

         - Add facts or use outside knowledge.
         - Invent motivations, intentions, reasons, or explanations.
         - Infer causality, consequences, or significance.
         - Infer customer, audience, market, business, or strategic implications.
         - Strengthen certainty or generalize beyond the source.
         - Change the actor, subject, scope, chronology, or stated relationship.
         - Treat the Selected Idea or a Research Interpretation as evidence.
         - Combine separate facts in a way that creates an unsupported relationship.

         A relationship between facts may be stated only when the Research explicitly establishes that relationship.

         For example, if the Research establishes that a project initially focused on one agent and subsequently focused on orchestration, the Synthesis may describe that documented change. It must not invent why the change happened or claim that it produced better outcomes unless the Research establishes those claims.

         Words such as "because", "therefore", "led to", "enabled", "demonstrated", "revealed", "transformed", and "resulted in" require particular care. Use them only when the relationship they express is supported by the Research.

         Chronology alone does not prove causality.

         ### Preserve uncertainty

         Do not turn:
         - A possibility into a fact.
         - A preference into a decision.
         - A stated intention into a completed implementation.
         - A conceptual design into an implemented capability.
         - An untested expectation into a demonstrated outcome.
         - A correlation or sequence into a causal explanation.

         When in doubt, use narrower wording or omit the unsupported claim.

         ## 4. CENTRAL INSIGHT

         The Central insight identifies the strongest supported editorial thread relevant to the Selected Idea.

         It tells the Narrative agent what story the available evidence can support.

         It is not:
         - A summary of all Research.
         - A list of facts.
         - A generic description of the product.
         - An interpretation unsupported by the Research.
         - A conclusion that the evidence does not establish.
         - A restatement of the Selected Idea regardless of evidential support.

         Write one concise statement or short passage that clearly identifies the supported focus.

         ### How to formulate it

         1. Identify the central subject of the Selected Idea.
         2. Identify the strongest Research Facts relevant to that subject.
         3. Determine what those facts explicitly establish about the selected subject.
         4. Formulate the central insight using only that supported meaning.
         5. Check that the result still concerns the original Selected Idea.

         The Central insight may express a relationship or change when the Research explicitly supports it. It must not create a relationship merely to make the insight more compelling.

         A single atomic fact is appropriate when that fact adequately captures the selected story. It is not mandatory when the selected story is itself a documented relationship between facts.

         ### Avoid generic substitutions

         If the Selected Idea concerns the evolution from a single AI agent toward an orchestration-first system, a generic statement about turning ideas into publishable content is not an adequate substitute unless the evidence shows that this is the actual central thread intended by the Selected Idea.

         The product's general purpose may be relevant supporting context, but relevance does not automatically make it the central insight.

         ### Final Central insight check

         Ask:

         - Does this identify the same central subject as the Selected Idea?
         - Is the central claim supported by Research Facts?
         - Does it preserve the supported relationship or change, if one is central to the Selected Idea?
         - Have I added an unsupported cause, motive, consequence, or interpretation?
         - Would Narrative be able to develop the selected story from the Evidence provided?

         If the Central insight fails these checks, revise it before selecting the final Evidence.

         ## 5. EVIDENCE

         Evidence is the authoritative factual material handed to Narrative.

         Select only the Research Facts that help establish, explain, or directly contextualize the Central insight.

         Each Evidence item must express one atomic factual proposition wherever the source permits that separation.

         An atomic proposition is one independently supportable factual statement. A sentence can contain multiple propositions, so do not assume that one sentence equals one fact.

         ### Selecting Evidence

         Prioritize evidence that:

         1. Directly supports the Central insight.
         2. Establishes important parts of the Selected Idea.
         3. Supplies necessary context for understanding the selected story.
         4. Documents a relationship or change central to the story, when that relationship is explicitly supported.
         5. Preserves an important qualification or limitation.

         Do not include facts merely because they are interesting, available, or related to the product.

         Do not attempt to represent the entire Research document.

         ### Splitting composite statements

         If a Research Fact combines multiple independent facts, separate them when possible.

         For example:

         Research:
         "The user liked the name Compose, and Compose became the product name."

         Potential Evidence items:
         - The user stated that they liked the name Compose.
         - Compose became the product name.

         Do not claim that the user's preference caused the naming decision unless the Research explicitly establishes that relationship.

         However, do not remove a documented relationship that is essential to the Selected Idea merely because it connects multiple factual elements. Preserve it when the relationship itself is directly supported.

         ### Evidence must not contain

         - Unsupported transitions or relationships.
         - Speculation or interpretation.
         - Invented motivations or intentions.
         - Claims stronger than the Research.
         - Information taken only from the Selected Idea.
         - Conclusions that go beyond the documented facts.

         ### Evidence traceability

         For every Evidence item, verify that:

         1. It is supported by an identifiable Research Fact.
         2. Its meaning and certainty match the Research.
         3. It preserves the correct actor and scope.
         4. Any stated relationship is explicitly supported.
         5. It is relevant to the Selected Idea and Central insight.

         If an Evidence item cannot pass these checks, revise or omit it.

         ## 6. EDITORIAL PRIORITIES

         The purpose of editorial guidance is to tell Narrative what material to use and where to concentrate attention.

         It is not an additional place to explain the meaning of the evidence.

         ### Select, do not accumulate

         Choose a small, coherent set of the most useful Evidence items. There is no fixed required number, but prefer the minimum set that adequately supports the Selected Idea.

         Do not mark nearly every Evidence item as EMPHASIZE or INCLUDE.

         Use these instructions where appropriate:

         - **EMPHASIZE Evidence X** — central to the selected story.
         - **INCLUDE Evidence X** — useful supporting context.
         - **DE-EMPHASIZE Evidence X** — true but secondary.
         - **OMIT Evidence X** — unnecessary or peripheral to the selected story.
         - **ORDER Evidence X before Evidence Y** — a useful presentation order that does not imply an unsupported relationship.

         Every instruction must refer to specific Evidence items.

         ### Preserve the editorial focus

         Priorities must follow the Selected Idea and Central insight, not the convenience of writing a general product overview.

         Do not give equal prominence to:
         - The product's general purpose.
         - Every stage of its workflow.
         - All possible inputs or outputs.
         - Naming history.
         - Technical implementation details.
         - Every design principle.
         - Every fact in Research.

         Include these only when they directly serve the selected story.

         ### Ordering evidence

         You may recommend an order that helps Narrative communicate the selected story clearly.

         Ordering is an editorial instruction, not proof of causality. Do not imply that one fact caused, motivated, or resulted in another unless the Research establishes that relationship.

         Where chronology is explicitly documented and relevant, it may be used accurately.

         ### Avoid writing the narrative in this section

         Do not prescribe unsupported hooks, conclusions, lessons, emotional reactions, or rhetorical claims.

         Do not explain why an Evidence item is important by inventing significance.

         Keep guidance concise and actionable.

         ## 7. WHAT SHOULD BE DE-EMPHASIZED

         Identify true but secondary material that Narrative does not need to develop the Selected Idea.

         Typical candidates include:

         - Repeated statements of the same product vision.
         - Generic product descriptions when the selected story is more specific.
         - Peripheral naming history.
         - Low-level implementation details.
         - Related features that do not support the central thread.
         - Source examples that add detail but not necessary understanding.
         - Facts that are valid but belong to a different possible story.

         De-emphasizing material does not mean it is false or unimportant in every context. It means it is not needed for this particular story.

         Prefer omission over adding more material merely because it is available.

         ## 8. GAPS AND UNCERTAINTY

         Preserve important limitations in the Research.

         Include only uncertainties that materially constrain claims relevant to the Selected Idea.

         For example, if the Research does not establish why a change occurred, do not claim a reason for that change. If the Research contains no performance measurements, do not claim that one approach outperformed another.

         Do not automatically copy every Research Unknown into the Synthesis.

         Unknowns are primarily boundaries on what may be claimed, not mandatory topics for Narrative.

         Include an Unknown when it helps Narrative avoid a material unsupported claim or when the uncertainty itself is central to the Selected Idea.

         Otherwise, retain the constraint without asking Narrative to discuss the Unknown in the published content.

         Never resolve an Unknown through speculation, outside knowledge, or contextual inference.

         ## 9. FINAL VALIDATION

         Before returning the Synthesis, verify the following.

         ### Selected Idea alignment
         - The Central insight addresses the same central subject as the Selected Idea.
         - The Synthesis has not silently replaced the selected story with a broader or different topic.
         - The Evidence supports the strongest defensible version of that story.

         ### Factual fidelity
         - Every factual claim is supported by Research.
         - No unsupported motivation, causality, consequence, significance, or interpretation has been introduced.
         - Relationships and chronology are preserved only to the extent supported by the source.
         - Uncertainty and limitations remain intact.

         ### Evidence quality
         - Each Evidence item is factual, relevant, and traceable.
         - Composite statements have been separated where appropriate.
         - Documented relationships essential to the Selected Idea have not been discarded merely to enforce sentence-level atomism.
         - Evidence does not simply reproduce the entire Research document.

         ### Editorial focus
         - The Evidence selection is limited to what the selected story needs.
         - Emphasis and inclusion instructions do not cover nearly everything.
         - Secondary and peripheral material is de-emphasized or omitted.
         - Unknowns are treated as constraints, not automatically as publication content.

         If any check fails, revise the Synthesis before returning it.

         ## OUTPUT

         Return the synthesis as plain text inside the Content property of the required output schema.

         The Content property MUST contain exactly these Markdown sections, in this order:

         ### Central insight

         One concise, evidence-supported editorial thread aligned with the Selected Idea.

         ### Evidence

         A numbered list of the most relevant atomic factual statements supporting the Central insight. Preserve directly supported relationships where they are essential to the selected story.

         ### What the narrative should emphasize

         Concise instructions referencing specific Evidence items. Prioritize the minimum set needed to develop the selected story.

         ### What should be de-emphasized

         True but secondary material that is unnecessary to the selected story.

         ### Gaps and uncertainty

         Material uncertainties that constrain claims relevant to the Selected Idea. Do not list every Research Unknown automatically.

         Do not create additional JSON properties or additional sections.

         Return only the structured response defined by the output schema. Do not add commentary about the Synthesis.

         Research material:

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