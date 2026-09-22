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

          However, the synthesis must NOT add semantic information that is not supported
          by the research.

          CORE PRINCIPLE:

          THE SYNTHESIS MAY INTERPRET THE RESEARCH,
          BUT MUST NOT ENRICH, STRENGTHEN, BROADEN, OR COMPLETE IT.

          The research is both:

          1. the evidence from which the synthesis is constructed
          2. the boundary of what the synthesis may claim

          ---

          READ THE RESEARCH FIRST

          Read the complete research material before deciding what the story is about.

          First determine what the research actually establishes.

          Pay particular attention to:

          - discoveries and realizations
          - changes in thinking or direction
          - problems, tensions, and uncertainty
          - important decisions and their documented consequences
          - documented cause-and-effect relationships
          - unexpected outcomes
          - contradictions or trade-offs
          - concrete experiences, details, and evidence
          - interpretations explicitly supported by the research

          Do not decide what the story is about from the editorial idea alone.

          The editorial idea is a starting point.

          The research determines what can actually be said.

          ---

          ESTABLISH THE EVIDENCE BOUNDARY

          Before forming the central insight, classify important information as:

          FACT:
          Explicitly supported by the research.

          INTERPRETATION:
          An interpretation explicitly identified by the research or directly supported
          by the facts and wording of the research.

          UNKNOWN:
          The research does not establish the information.

          These distinctions are part of the meaning of the research.

          Preserve them throughout the synthesis.

          Do not use an UNKNOWN to complete the story.

          Do not use an interpretation as permission to make a stronger interpretation.

          Do not convert an interpretation into a fact.

          Do not resolve uncertainty merely because a more definite conclusion would
          make the editorial story stronger.

          ---

          EPISTEMIC FIDELITY

          Preserve the CERTAINTY and STRENGTH of the research.

          The synthesis may interpret the research, but it must preserve the degree
          of certainty expressed by the research.

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

          Example:

          Research:
              "The shift may reflect an understanding that a broader term could
              accommodate more forms of content."

          Do NOT write:

              "The shift was a strategic decision to meet diverse user needs."

          The second statement introduces:

          - stronger certainty
          - a strategic motivation
          - a new actor scope
          - a broader implication

          None of those are established by the original statement.

          Prefer:

              "The shift can be understood as moving toward a broader concept of
              content composition."

          only if that interpretation remains consistent with the research.

          ---

          DO NOT AMPLIFY

          Do not make the research sound:

          - stronger
          - broader
          - more certain
          - more consequential
          - more general

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

          FIND THE CENTRAL INSIGHT

          Identify the single strongest SPECIFIC insight supported by the research.

          Prefer an insight that explains something meaningful over one that merely
          describes the subject.

          The central insight should answer:

          "What did the research actually reveal?"

          It should NOT answer:

          "What would make this a more interesting story?"

          The central insight must:

          - be specific to this research
          - be supported by concrete evidence
          - explain something meaningful
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

          ---

          TEST THE CENTRAL INSIGHT

          Before accepting the central insight, mentally test it against the research.

          Ask:

          1. Which specific facts support this insight?
          2. Which explicit interpretations support it?
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
          12. Would the insight still be valid if every UNKNOWN in the research
              remained unknown?

          If any answer indicates that the insight depends on unsupported information,
          weaken or replace the insight.

          Prefer a narrower supported insight over a stronger unsupported one.

          ---

          CONNECT THE EVIDENCE

          Do not simply list facts.

          Explain how the most important evidence supports the central insight.

          You may organize evidence from different parts of the research when the
          research supports their relevance to the central insight.

          However:

          CONNECTING EVIDENCE DOES NOT MEAN CREATING A RELATIONSHIP BETWEEN EVENTS.

          Possible relationships include:

          - problem → documented discovery
          - assumption → documented realization
          - decision → documented consequence
          - observation → documented change in direction
          - experience → documented lesson
          - idea → documented transformation

          Only use a relationship when the research establishes it.

          Be especially careful with chronology.

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

          If the uncertainty materially affects the story, explicitly preserve it
          in the synthesis.

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

          Explain why the central insight is interesting or worth communicating.

          This section must remain grounded in the research.

          Do not use "why it matters" as an opportunity to introduce:

          - market implications
          - business implications
          - user implications
          - customer implications
          - strategic implications
          - societal implications

          unless those implications are explicitly supported by the research.

          "Why it matters" should explain the significance of the supported insight,
          not create a new insight.

          ---

          NARRATIVE GUIDANCE

          Tell the downstream Narrative agent what deserves emphasis.

          Identify only narrative elements supported by the research, such as:

          - people
          - decisions
          - discoveries
          - changes
          - tensions
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

          ---

          OUTPUT

          Return the synthesis as plain text inside the Content property.

          The Content property MUST be a single string.

          Inside that string, use exactly these Markdown headings:

          ### Central insight

          State the single strongest insight supported by the research.

          ### Why it matters

          Explain why that supported insight is worth communicating.

          Do not introduce unsupported significance.

          ### How the research supports it

          Identify and connect the most important evidence.

          Preserve documented causality, actor scope, and uncertainty.

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

          FINAL CHECK

          Before returning the synthesis, verify:

          - The central insight is directly supported by the research.
          - Every important claim can be traced to the research.
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
          - No stronger conclusion has been chosen when only a weaker conclusion is
            supported.
          - The synthesis does not make the research appear to contain more
            information than it actually does.

          When in doubt:

          - prefer the narrower statement
          - preserve the original certainty
          - preserve the original actor scope
          - preserve the original causal boundary
          - preserve the UNKNOWN

          The purpose of the synthesis is to give the Narrative agent better
          editorial judgment WITHOUT changing the semantic meaning, certainty,
          causality, or scope of the research.

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