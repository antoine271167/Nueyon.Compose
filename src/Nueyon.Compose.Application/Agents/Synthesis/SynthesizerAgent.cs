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

    private static string CreateUserMessage(SynthesisInput input) =>
        $$"""
          Your task is to turn the research material into a strong editorial synthesis
          for a downstream Narrative agent.

          The goal is NOT to summarize the research.

          The goal is to identify what the research actually supports as the central
          insight and give the Narrative agent clear editorial direction.

          The synthesis may:

          - select
          - prioritize
          - organize
          - connect
          - interpret

          information that is already present in the research.

          However, the synthesis MUST NOT introduce semantic information that is not
          supported by the research.

          CORE PRINCIPLE:

          THE SYNTHESIS MAY INTERPRET THE RESEARCH,
          BUT MUST NOT ENRICH, STRENGTHEN, BROADEN, RESOLVE, OR COMPLETE IT.

          The research is both:

          1. the evidence from which the synthesis is constructed
          2. the boundary of what the synthesis may claim

          The synthesis is NOT a second research phase.

          Do not use general knowledge, plausibility, common sense, or editorial
          expectations to fill gaps in the research.

          ---

          READ THE RESEARCH FIRST

          Read the complete research material before deciding what the story is about.

          First determine what the research actually establishes.

          Pay particular attention to:

          - Facts
          - Interpretations
          - Unknowns
          - Development Sequence
          - Editorial Relevance
          - documented decisions
          - documented changes
          - documented consequences
          - documented cause-and-effect relationships
          - concrete experiences and details
          - uncertainty and gaps

          Do not decide what the story is about from an assumed narrative angle.

          The research determines what can actually be said.

          The Development Sequence is a derived evidence view.

          It may help organize supported events, states, changes, and decisions, but it
          is NOT additional evidence.

          Editorial Relevance is also a derived view.

          It identifies evidence relevant to the selected editorial idea, but it does
          NOT establish new facts, relationships, motivations, consequences, or
          significance.

          Neither derived section may be used as proof of something that is not
          established by Facts, Interpretations, or Unknowns.

          ---

          EVIDENCE HIERARCHY

          The research contains three authoritative evidence categories:

          FACT:

          Explicitly supported by the source.

          INTERPRETATION:

          Meaning derived from the source material but not necessarily stated
          literally as a fact.

          UNKNOWN:

          The research does not establish the information.

          These distinctions are part of the meaning of the research.

          Preserve them throughout the synthesis.

          In addition:

          DEVELOPMENT SEQUENCE:

          A derived ordering of source-supported events, states, changes, and
          decisions.

          EDITORIAL RELEVANCE:

          A derived prioritization of evidence for the editorial idea.

          These derived sections are NOT additional evidence.

          Do not use either derived section to introduce information that is absent
          from Facts, Interpretations, and Unknowns.

          ---

          RESEARCH IS THE SEMANTIC BOUNDARY

          The synthesis may reorganize and interpret the research, but every claim
          in the synthesis must remain semantically supported by the research.

          This means:

          - no new facts
          - no new actors
          - no new motivations
          - no new intentions
          - no new causes
          - no new consequences
          - no new reactions
          - no new user groups
          - no new market claims
          - no new strategic claims
          - no new significance
          - no new outcomes

          A claim may be phrased differently from the research.

          A claim may be expressed more concisely.

          A claim may connect information that is already explicitly related in the
          research.

          But a different wording must not create a different meaning.

          When in doubt, use the narrower statement.

          ---

          INTERPRETATION IS ALLOWED, BUT SEMANTIC EXPANSION IS NOT

          The synthesis is allowed to interpret the research.

          However, interpretation means explaining or compressing meaning that is
          already supported by the research.

          Interpretation does NOT mean:

          - explaining what probably motivated someone
          - explaining why a decision was made when the reason is unknown
          - explaining what a decision means strategically
          - predicting consequences
          - explaining broader business significance
          - inferring user needs
          - inferring market demand
          - inferring audience reactions
          - turning an individual experience into a general trend
          - turning chronology into causality
          - turning a possibility into a conclusion
          - making a documented decision appear more deliberate than documented
          - making the source appear more successful, strategic, sophisticated, or
            significant

          Example:

          Research:

          - The user considered StoryFlow.
          - The user later considered Compose.
          - The user stated, "I like Compose."

          Supported synthesis:

          "The product name evolved from StoryFlow toward Compose."

          Unsupported synthesis:

          "The naming change reflected a strategic decision to broaden the product's
          market positioning."

          The second statement may be plausible, but the research does not establish
          it.

          Another example:

          Research:

          - The user became less certain about using "Story."
          - Alternatives were considered.
          - Compose was selected.

          Supported:

          "The user moved away from the name StoryFlow and ultimately preferred
          Compose."

          Unsupported:

          "The user recognized that StoryFlow was too narrow for modern content
          creators."

          The latter introduces a reason and an actor motivation that the research
          does not establish.

          ---

          EPISTEMIC FIDELITY

          Preserve the CERTAINTY and STRENGTH of the research.

          If the research describes something as:

          - possible
          - uncertain
          - tentative
          - suggested
          - indicated
          - apparent
          - unclear
          - unknown
          - not established

          do not rewrite it as a stronger claim.

          Do NOT silently convert:

          - "may" into "does"
          - "might" into "will"
          - "could" into "can"
          - "suggests" into "shows"
          - "indicates" into "demonstrates"
          - "appears" into "is"
          - "possibly" into a fact
          - "unclear" into an explanation
          - "unknown" into a conclusion

          An interpretation must not become stronger merely because it creates a
          clearer or more compelling story.

          Preserve the weakest claim that is fully supported by the research.

          ---

          DO NOT AMPLIFY

          Do not make the research sound:

          - stronger
          - broader
          - more certain
          - more consequential
          - more general
          - more strategic
          - more successful

          than it actually is.

          In particular, do not turn:

          - a specific observation into a general trend
          - an individual experience into a user trend
          - an interpretation into a fact
          - a possibility into a conclusion
          - a product decision into a market insight
          - an observation into a strategic lesson
          - a naming decision into evidence of user demand
          - a development sequence into a deliberate strategy
          - a personal preference into evidence of broader preference
          - a product capability into evidence of user benefit

          Do not introduce broader concepts such as:

          - users
          - customers
          - content creators
          - audiences
          - markets
          - industries
          - society

          unless that scope is explicitly established by the research.

          A statement being plausible does not make it supported.

          A statement being interesting does not make it supported.

          A statement being consistent with the research does not make it established
          by the research.

          When a stronger statement and a narrower statement are both possible,
          choose the narrower statement unless the research clearly supports the
          stronger one.

          ---

          SELECTED IDEA IS NOT EVIDENCE

          If a Selected Idea or editorial hypothesis is available as part of the
          research context, treat it only as an editorial hypothesis.

          It may help determine which evidence is relevant.

          It may NOT be used as evidence that its own claims are true.

          Do not copy claims from the Selected Idea into the synthesis unless those
          claims are independently supported by the research.

          In particular, do not use a Selected Idea to establish:

          - motivation
          - user needs
          - market relevance
          - strategic significance
          - product impact
          - causality
          - consequences
          - broader meaning

          The correct dependency is:

          SOURCE
            ↓
          RESEARCH EVIDENCE
            ↓
          SYNTHESIS

          The Selected Idea may influence relevance, but it must not become a source
          of evidence.

          Never reason:

          SELECTED IDEA → conclusion → supporting evidence

          Instead reason:

          RESEARCH EVIDENCE → supported conclusion

          If the Selected Idea is only partially supported by the research, the
          synthesis must remain limited to what the research supports.

          ---

          FIND THE CENTRAL INSIGHT

          Identify the single strongest SPECIFIC insight supported by the research.

          The central insight should answer:

          "What does the research actually support us saying?"

          It should NOT answer:

          "What would make this a more interesting story?"

          The central insight must:

          - be specific to this research
          - be supported by concrete evidence
          - remain within the scope of the research
          - preserve the actors described by the research
          - preserve documented relationships
          - preserve uncertainty
          - preserve the certainty level of interpretations
          - avoid generic claims about AI, technology, productivity, innovation,
            users, customers, markets, or society unless explicitly supported

          Do not make the insight stronger by adding:

          - new actors
          - broader groups
          - motivations
          - intentions
          - strategic reasons
          - reactions
          - outcomes
          - causal relationships
          - broader implications

          A central insight may be relatively simple.

          A simple insight that is strongly supported is preferable to a sophisticated
          insight that requires unsupported assumptions.

          ---

          CENTRAL INSIGHT TEST

          Before accepting the central insight, mentally test it against the research.

          Ask:

          1. Which specific Facts support this insight?
          2. Which explicit Interpretations support it?
          3. Is the wording stronger than the evidence?
          4. Does it depend on an UNKNOWN?
          5. Does it introduce an actor that the research does not establish?
          6. Does it introduce a motivation or intention?
          7. Does it introduce causality that is not documented?
          8. Does it generalize from one person or event to a broader group?
          9. Does it imply user, customer, stakeholder, market, or societal reactions
             that are not established?
          10. Does it turn a sequence of events into a deliberate strategy?
          11. Does it make a possibility sound like a fact?
          12. Does it introduce significance that exists only in the Selected Idea?
          13. Would the insight still be valid if every UNKNOWN in the research
              remained UNKNOWN?
          14. Could the same insight be stated more narrowly?

          If any answer indicates that the insight depends on unsupported information,
          weaken or replace the insight.

          Prefer a narrower supported insight over a stronger unsupported one.

          ---

          CONNECT EVIDENCE WITHOUT CREATING RELATIONSHIPS

          Do not simply list facts.

          Explain how the most important evidence supports the central insight.

          You may connect evidence from different parts of the research when the
          research itself supports that connection.

          However:

          CONNECTING EVIDENCE DOES NOT MEAN CREATING A RELATIONSHIP BETWEEN EVENTS.

          You may only state a relationship when the research establishes it.

          Do NOT infer:

          - causality
          - motivation
          - intention
          - influence
          - feedback
          - reaction
          - strategic purpose
          - consequence

          merely because two pieces of evidence fit together logically.

          Chronology is not causality.

          If the research establishes:

          A happened.

          Later B happened.

          but does not establish that A caused B, preserve the sequence without
          creating causality.

          Correct:

          "A happened, and later B happened."

          Incorrect:

          "A happened, which led to B."

          Also incorrect:

          "The experience of A ultimately resulted in B."

          ---

          PRESERVE ACTOR SCOPE

          Use actors exactly as the research supports them.

          Do not broaden:

          - "I" into "we"
          - "the user" into "users"
          - "a customer" into "customers"
          - "a stakeholder" into "stakeholders"
          - a specific person into a group
          - a group into a broader audience or community
          - an individual experience into a general user experience

          unless the research explicitly supports that broader scope.

          Do not introduce an actor merely because that actor makes the editorial
          explanation easier or more compelling.

          ---

          PRESERVE MOTIVATION AND INTENTION

          Do not infer why someone acted unless the research supports that reason.

          Do not turn:

          - action → assumed motivation
          - decision → assumed rationale
          - sequence → assumed cause
          - outcome → assumed intention
          - preference → assumed strategic objective

          If the reason is unknown, keep it unknown.

          Do not turn a documented decision into evidence of a strategy unless the
          research explicitly establishes that strategy.

          ---

          PRESERVE UNKNOWNs

          UNKNOWN is a hard boundary.

          If the research says that something is unknown, do not:

          - turn it into a fact
          - turn it into an interpretation
          - imply that it probably happened
          - create a plausible explanation
          - introduce an actor that would explain it
          - use wording that causes the reader to infer it
          - use a related fact to indirectly resolve it

          If the uncertainty materially affects the story, preserve it in the
          synthesis.

          Example:

          Research:

          FACT:
          A happened.

          FACT:
          Later B happened.

          UNKNOWN:
          The research does not establish whether A caused B.

          Correct:

          "A happened, and later B happened. The research does not establish whether
          A caused B."

          Incorrect:

          "A happened, which led to B."

          Also incorrect:

          "The experience of A ultimately resulted in B."

          ---

          MAKE EDITORIAL CHOICES

          Not every research point deserves equal weight.

          Determine:

          - which evidence is essential to the central insight
          - which evidence provides useful supporting context
          - which material is secondary
          - which material is generic, repetitive, or distracting

          The Narrative agent should receive a clear signal about what matters most.

          Do not force every research point into the central insight.

          Do not select evidence merely because it makes the story more compelling.

          Do not manufacture a narrative arc when the research does not establish one.

          Editorial selection is allowed.

          Editorial invention is not.

          ---

          WHY IT MATTERS

          Explain why the central insight is worth communicating.

          This section is NOT permission to introduce a broader implication.

          "Why it matters" must remain within the semantic boundaries of the research.

          It may explain:

          - what the documented change illustrates
          - what the documented decision demonstrates about the documented process
          - why the documented development is relevant to the selected editorial idea
          - what the evidence makes clear

          It must NOT introduce:

          - market implications
          - business implications
          - user implications
          - customer implications
          - strategic implications
          - societal implications
          - predicted consequences
          - general lessons

          unless those implications are explicitly supported by the research.

          Do not write:

          "This matters because it shows how products should respond to changing
          market demands."

          unless the research explicitly establishes changing market demands.

          Prefer:

          "This matters because the documented naming change illustrates how the
          product concept developed from one documented framing to another."

          The exact wording must still be supported by the research.

          If the research does not establish broader significance, keep the
          significance local to the documented experience, decision, change, or
          observation.

          It is acceptable for "Why it matters" to be modest.

          ---

          NARRATIVE GUIDANCE

          Tell the downstream Narrative agent what deserves emphasis.

          Identify only narrative elements supported by the research, such as:

          - people
          - decisions
          - discoveries
          - changes
          - documented tensions
          - documented consequences
          - concrete details
          - important uncertainty

          Do not invent:

          - scenes
          - dialogue
          - emotional reactions
          - user reactions
          - stakeholder reactions
          - motivations
          - experiences
          - outcomes

          Do not instruct the Narrative agent to make an interpretation stronger,
          broader, or more certain than the research supports.

          Narrative guidance is guidance about EMPHASIS, not permission to create
          new meaning.

          ---

          WHAT SHOULD BE DE-EMPHASIZED

          Identify material that is true but secondary to the central insight.

          Good reasons to de-emphasize material include:

          - it is repetitive
          - it is generic
          - it is peripheral
          - it provides implementation detail that does not support the central
            insight
          - it is less relevant to the selected editorial idea

          Do not de-emphasize evidence simply because it makes the story less
          compelling.

          Do not remove or reinterpret an UNKNOWN because it complicates the
          narrative.

          ---

          GAPS AND UNCERTAINTY

          Identify important information that the research does not establish.

          Preserve explicit UNKNOWN findings when they materially affect the story.

          Do not fill gaps with plausible assumptions.

          Do not convert the absence of evidence into evidence of absence.

          ---

          OUTPUT

          Return the synthesis as plain text inside the Content property.

          The Content property MUST be a single string.

          Inside that string, use exactly these Markdown headings:

          ### Central insight

          State the single strongest insight supported by the research.

          Do not add unsupported causality, motivation, actors, consequences,
          strategic intent, or broader implications.

          ### Why it matters

          Explain why that supported insight is worth communicating.

          Keep the significance within the boundaries of the research.

          ### How the research supports it

          Identify and connect the most important evidence.

          Preserve documented causality, actor scope, and uncertainty.

          Do not create relationships between events merely because they fit together.

          ### What the narrative should emphasize

          Identify the research elements that deserve the most attention.

          ### What should be de-emphasized

          Identify material that is true but secondary, generic, repetitive, or
          distracting from the central insight.

          ### Gaps and uncertainty

          Identify important information that the research does not establish.

          Preserve explicit UNKNOWN findings when they materially affect the story.

          These headings and their content belong inside the single Content string.

          Do NOT create additional JSON properties for these sections.

          The expected response shape is:

          {
            "content": "### Central insight\n...\n\n### Why it matters\n..."
          }

          ---

          FINAL SEMANTIC CHECK

          Before returning the synthesis, verify:

          - The central insight is directly supported by the research.
          - Every important claim can be traced to Facts or supported Interpretations.
          - No claim comes only from the Selected Idea.
          - Every interpretation preserves the research's certainty level.
          - No interpretation has been strengthened merely for editorial effect.
          - No new actor has been introduced.
          - No actor has been broadened beyond the research.
          - No motivation has been inferred without evidence.
          - No intention has been inferred without evidence.
          - No unsupported causal relationship has been created.
          - No chronology has been presented as causality.
          - No UNKNOWN has been converted into a fact or interpretation.
          - No unsupported user, customer, stakeholder, market, or societal reaction
            has been introduced.
          - No broader implication has been introduced merely because it sounds
            reasonable or interesting.
          - No generic knowledge has been added.
          - No documented decision has been turned into an assumed strategy.
          - No narrow observation has been generalized.
          - No product capability has been turned into an assumed user benefit.
          - No personal preference has been turned into a broader preference.
          - No stronger conclusion has been chosen when only a weaker conclusion is
            supported.
          - The synthesis does not make the research appear to contain more
            information than it actually does.
          - The synthesis does not resolve an UNKNOWN indirectly through wording.
          - The synthesis does not use Editorial Relevance or Development Sequence
            as additional evidence.
          - "Why it matters" does not introduce a new insight.
          - Narrative guidance does not contain unsupported interpretation.
          - Every sentence could be defended by pointing to specific research
            evidence.

          FINAL SIMPLIFICATION TEST:

          For every important sentence, ask:

          "If I had to point to the exact research evidence supporting this sentence,
          could I do so?"

          If not, remove or weaken the sentence.

          Also ask:

          "Am I saying something because the research supports it, or because it
          would make the story better?"

          If the answer is the latter, remove it.

          When in doubt:

          - prefer the narrower statement
          - preserve the original certainty
          - preserve the original actor scope
          - preserve the original causal boundary
          - preserve the UNKNOWN
          - prefer omission over unsupported interpretation

          The purpose of the synthesis is to give the Narrative agent better
          editorial judgment WITHOUT changing the semantic meaning, certainty,
          causality, motivation, or scope of the research.

          Research material:

          --- BEGIN RESEARCH MATERIAL ---

          {{input.Research.Content}}

          --- END RESEARCH MATERIAL ---

          Return only the structured response defined by the output schema.
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