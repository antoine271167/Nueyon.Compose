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
                "Agent {AgentName} invocation failed after {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            throw;
        }
    }

    private static string CreateUserMessage(NarrativeInput input) =>
        $"""
         Transform the editorial synthesis into a coherent and compelling narrative
         structure that can later be turned into content.

         The goal is to improve the expression, structure, and flow of the story
         WITHOUT changing the meaning, scope, or certainty of the source material.

         The synthesis is the PRIMARY SOURCE for the narrative.

         Your job is to decide HOW the supported material should be communicated,
         not WHAT additional meaning should be added to it.

         CORE PRINCIPLE:

         NARRATIVE MAY TRANSFORM THE EXPRESSION,
         BUT MUST NOT TRANSFORM THE MEANING.

         This includes preserving:

         - what the synthesis claims
         - what the synthesis does NOT claim
         - who the synthesis refers to
         - the scope of those actors
         - the relationships established by the synthesis
         - the certainty of each interpretation
         - explicit uncertainty and UNKNOWNs

         ---

         NARRATIVE TASK

         Determine:

         - the central narrative angle
         - an effective opening or hook
         - the logical progression of the story
         - the order of important insights
         - where supporting evidence belongs
         - meaningful tension or contrast when supported
         - the conclusion or takeaway when supported

         You may improve:

         - structure
         - sequencing
         - emphasis
         - transitions
         - wording
         - pacing
         - narrative flow
         - rhetorical framing
         - opening hook

         These improvements must operate on the EXPRESSION of the source material,
         not on its meaning.

         ---

         SOURCE FIDELITY

         Treat the synthesis as both:

         1. the material from which the narrative is constructed
         2. the boundary of what the narrative may claim

         Every substantive claim in the narrative must be supported by the synthesis.

         Do not:

         - invent facts
         - invent events
         - invent people
         - invent groups
         - invent motivations
         - invent intentions
         - invent decisions
         - invent outcomes
         - invent feedback
         - invent reactions
         - invent user or customer needs
         - invent stakeholder involvement
         - invent causal relationships
         - add general knowledge
         - add market or societal implications
         - add strategic conclusions
         - add generic lessons
         - introduce broader claims
         - resolve missing information
         - turn assumptions into facts
         - turn UNKNOWNs into facts or interpretations

         ---

         EPISTEMIC FIDELITY

         Preserve the CERTAINTY LEVEL of the synthesis.

         Do not strengthen, weaken, broaden, or generalize an interpretation.

         If the synthesis presents something as:

         - possible
         - uncertain
         - tentative
         - suggested
         - indicated
         - apparent
         - unclear
         - unknown
         - not established

         the narrative must preserve that same level of certainty.

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

         Do not use stronger wording merely because it sounds more compelling.

         For example, if the synthesis says:

             "The change may reflect a broader understanding of the product."

         Do NOT write:

             "The change demonstrates a broader understanding of user needs."

         The second statement changes both the certainty and the scope of the
         original interpretation.

         IMPORTANT:

         A statement being traceable to the synthesis does NOT make a stronger
         version of that statement valid.

         Preserve the original epistemic strength.

         ---

         ACTOR FIDELITY

         Preserve the exact scope of actors described by the synthesis.

         Do not broaden:

         - "I" into "we"
         - "the user" into "users"
         - "a customer" into "customers"
         - "a stakeholder" into "stakeholders"
         - a specific person into a group
         - a group into a broader audience or community
         - an individual experience into a general user experience

         unless the synthesis explicitly supports that broader scope.

         Do not introduce an actor merely because that actor makes the narrative
         easier, more natural, or more compelling.

         ---

         MOTIVATION AND INTENTION

         Do not infer motivations or intentions from actions.

         Do not infer why something happened unless the synthesis establishes the
         reason.

         Do not transform:

         - an action into a motivation
         - a decision into an assumed reason
         - an observation into an intention
         - a sequence into an explanation

         If the reason is unknown, preserve that uncertainty.

         ---

         CAUSALITY

         Do not create causal relationships that are not established by the synthesis.

         If the synthesis establishes:

             A happened.
             Later B happened.

         but does not establish that A caused B, preserve the sequence without
         implying causality.

         Be especially careful with narrative transitions.

         Words such as:

         - therefore
         - because
         - consequently
         - as a result
         - which led to
         - this meant
         - this demonstrated
         - this allowed
         - this resulted in

         must only be used when the underlying causal relationship is supported.

         Narrative flow must never create causality that the source does not contain.

         ---

         UNCERTAINTY AND UNKNOWNs

         Preserve meaningful uncertainty.

         UNKNOWN is a hard boundary.

         If the synthesis identifies something as unknown, do not resolve it for
         narrative coherence.

         Do not:

         - fill gaps
         - provide plausible explanations
         - guess what probably happened
         - imply an unknown relationship
         - invent missing context
         - make the story appear more complete than the source supports

         Narrative coherence must come from structure and expression,
         not from resolving missing information.

         ---

         NARRATIVE CREATIVITY

         Make the narrative engaging through communication rather than invention.

         Creativity is allowed in:

         - structure
         - pacing
         - wording
         - emphasis
         - rhetorical framing
         - hooks
         - transitions
         - supported contrast
         - supported progression

         Creativity is NOT permission to introduce:

         - new facts
         - new actors
         - new motivations
         - new reactions
         - new consequences
         - broader claims
         - unsupported implications
         - generic wisdom
         - outside knowledge

         A compelling narrative is NOT one that contains more meaning.

         It is one that communicates the existing meaning more effectively.

         ---

         DO NOT AMPLIFY

         Do not make a claim sound more important, certain, broad, or consequential
         than it is in the synthesis.

         In particular, do not turn a narrow observation into:

         - a general trend
         - a statement about users
         - a statement about customers
         - a statement about content creators
         - a market observation
         - a strategic conclusion
         - a societal implication
         - a general lesson

         unless the synthesis explicitly establishes that broader meaning.

         Do not use phrases such as:

         - "this shows that..."
         - "this demonstrates..."
         - "clearly..."
         - "what this means is..."
         - "the lesson is..."
         - "users increasingly..."
         - "content creators need..."
         - "as the industry evolves..."
         - "in today's world..."

         when they introduce meaning that is not explicitly supported.

         ---

         DO NOT PARAPHRASE MECHANICALLY

         Do not simply repeat the synthesis sentence by sentence.

         Make genuine editorial choices about:

         - order
         - emphasis
         - pacing
         - transitions
         - narrative structure

         However, those choices must not introduce new semantic information.

         The narrative should clarify the existing meaning,
         not create a stronger or broader version of it.

         ---

         FINAL FIDELITY CHECK

         Before returning the result, verify:

         1. The central narrative angle is supported by the synthesis.
         2. Every substantive claim is supported by the synthesis.
         3. No new actor has been introduced.
         4. No actor has been broadened.
         5. No motivation or intention has been invented.
         6. No user, customer, stakeholder, or audience reaction has been invented.
         7. No unsupported causal relationship has been introduced.
         8. No UNKNOWN has been converted into a fact or interpretation.
         9. No general knowledge has been added.
         10. No broader strategic, business, market, or societal implication has been added.
         11. No generic lesson has been added.
         12. No interpretation has been strengthened.
         13. No uncertain statement has been made more certain.
         14. No narrow statement has been made broader.
         15. The certainty level of important claims matches the synthesis.
         16. The narrative is more compelling because of better expression and structure,
             NOT because it contains stronger or broader meaning.

         When in doubt:

         - prefer the narrower interpretation
         - preserve the uncertainty
         - preserve the original actor scope
         - preserve the original certainty
         - do not add meaning

         ---

         OUTPUT

         Do not write the final article.

         Do not write final platform-specific content.

         Do not perform research.

         Return only the structured response defined by the output schema.

         Synthesis input:
         {input.Synthesis.Content}
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