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
        $"""
         Turn the Research material into a concise evidence-based synthesis for
         the downstream Narrative agent.

         The Synthesis is NOT a summary and NOT an essay.

         Its purpose is to:
         - identify the strongest supported thread
         - select the evidence that establishes it
         - organize that evidence
         - guide the Narrative agent about which evidence deserves emphasis

         Research Facts are the evidence.
         Research Unknowns are hard boundaries.

         The Research defines the complete semantic boundary of the Synthesis.

         ---

         ## 1. ROLE OF THE SYNTHESIS

         The Synthesis is an EVIDENCE-SELECTION layer between Research and Narrative.

         You may:
         - select Facts
         - prioritize Facts
         - organize Facts
         - compress or restate Facts
         - identify the strongest supported thread
         - identify which Facts deserve emphasis
         - identify which Facts are secondary
         - preserve important Unknowns

         You may NOT:
         - add information
         - infer motivation or intention
         - infer causality or consequences
         - explain why something happened unless Research explicitly establishes it
         - create relationships between Facts that Research does not establish
         - infer user, customer, audience, market, business, or strategic meaning
         - use outside knowledge
         - turn concrete Facts into broader interpretations

         The Narrative agent will create the narrative.
         Do not write the story yourself.

         ---

         ## 2. FACTS ARE SOURCE-DERIVED ONLY

         A FACT is a statement that the source explicitly establishes.

         A Fact may be:
         - directly stated by the source
         - a faithful restatement of a directly stated statement
         - a directly documented event, decision, attribute, relationship, or result

         A Fact must preserve the sources:
         - actor and actor scope
         - meaning
         - certainty
         - chronology
         - causal strength
         - stated relationships

         A Fact must NOT contain:
         - inferred motivation
         - inferred intention
         - inferred causality
         - inferred significance
         - inferred consequence
         - inferred relationship
         - generalized meaning
         - explanation of why something happened

         ### Facts must remain atomic

         Treat each source-supported statement as a separate Fact unless the source
         explicitly connects the statements.

         DO NOT create a new Fact by combining multiple source statements.

         The fact that two statements:
         - appear near each other
         - concern the same subject
         - seem logically related
         - occur in sequence
         - appear to explain each other

         does NOT establish a relationship between them.

         Example:

         Source:
             "The name Compose was selected."
             "Compose describes bringing ideas and content together."

         FACT:
             "The name Compose was selected."

         FACT:
             "Compose describes bringing ideas and content together."

         NOT FACT:
             "The name Compose was selected because it describes bringing ideas
              and content together."

         The last statement introduces a reason that the source may not establish.

         Likewise, do not transform:

             A happened.
             B happened.

         into:

             A led to B.
             A caused B.
             A resulted in B.
             A reflected B.
             A was intended to achieve B.

         unless the source explicitly establishes that relationship.

         ### Explicit relationships are allowed

         A relationship may be included in a Fact only when the source explicitly
         establishes it.

         Example source:

             "The name Compose was selected because it better described the
              product's purpose."

         Then the relationship may be included in the Fact.

         If the source only says:

             "The name Compose was selected."
             "Compose better described the product's purpose."

         do NOT assume the second statement explains the first.

         Keep them separate unless the source explicitly connects them.

         ### Separate statements from subsequent events

         Do not combine a persons statement, preference, observation, or opinion
         with a later event or decision merely because the event appears to follow it.

         Example:

         Source:
             "I like Compose."
             "Compose was selected as the product name."

         FACT:
             "The user stated: 'I like Compose.'"

         FACT:
             "Compose was selected as the product name."

         NOT FACT:
             "The user stated: 'I like Compose,' leading to the selection of Compose."

         The second version adds a causal relationship between the statement and the
         later decision.

         Likewise, do not transform:

             "I preferred A."
             "B was selected later."

         into:

             "The preference for A caused B to be selected."

         A preference, statement, or opinion is a Fact about what the person said or
         thought. A later decision is a separate Fact unless the source explicitly
         states that the preference caused or determined the decision.

         When a relationship is not explicitly stated, keep the statements separate.

         ### Relationship words require evidence

         Words often signaling an inferred relationship include:

         - because
         - therefore
         - due to
         - led to
         - resulted in
         - caused
         - enabled
         - influenced
         - motivated
         - in order to
         - so that
         - reflected
         - demonstrated
         - represented
         - resulted from
         - as a result
         - which meant
         - which led to

         These are not forbidden, but the relationship must be explicitly supported
         by the source.

         When in doubt, prefer separate Facts.

         A Fact describes what the source establishes, not what the Research Agent
         believes the source means.

         ---

         ## 3. UNKNOWNS

         Unknowns are hard boundaries.

         Do not:
         - resolve them
         - explain them
         - speculate about them
         - fill them with plausible assumptions
         - imply an answer indirectly

         Preserve important Unknowns in the final section.

         ---

         ## 4. SELECTED IDEA

         The Selected Idea is an editorial hypothesis, not evidence.

         It may help determine which Facts are relevant.

         It must never be used to establish that a claim is true.

         Always reason:

             FACTS → SUPPORTED THREAD

         Never:

             SELECTED IDEA → CONCLUSION → SUPPORTING FACTS

         ---

         ## 5. CENTRAL INSIGHT

         The Central insight identifies the ONE ATOMIC FACTUAL STATEMENT that best
         represents the core of the Selected Idea.

         It is an ORIENTATION POINT for the Narrative agent.

         It is NOT an analytical insight, interpretation, explanation, thesis,
         conclusion, evaluation, or synthesis.

         ### Selected Idea determines relevance

         Use the Selected Idea to determine which Evidence is most relevant.

         The Selected Idea is an editorial hypothesis.

         It is NOT evidence.

         Do not copy claims from the Selected Idea into the Central insight unless
         the same claim is explicitly supported by the Evidence.

         The Central insight should provide the strongest factual anchor for the
         subject indicated by the Selected Idea.

         ### Atomic factual statement

         The Central insight must represent ONE ATOMIC FACTUAL STATEMENT.

         An atomic factual statement expresses ONE independently supported fact.

         An Evidence item may contain more than one factual statement.

         Therefore:

             ONE EVIDENCE ITEM
         does NOT automatically mean:
             ONE FACTUAL STATEMENT

         If an Evidence item contains multiple factual statements, select or restate
         only ONE of them.

         ### Example: composite Evidence

         Evidence:

             "The final product name became Compose, reflecting a shift from a focus
              on storytelling to composing useful content from ideas and knowledge."

         This Evidence item contains multiple claims:

         1. The final product name became Compose.
         2. The product shifted from a focus on storytelling.
         3. The product focused on composing useful content.
         4. The product involved ideas and knowledge.
         5. A relationship is asserted between the name and the shift.

         The Central insight may therefore be:

             "The final product name became Compose."

         It must NOT be:

             "The final product name became Compose, reflecting a shift from a focus
              on storytelling to composing useful content from ideas and knowledge."

         The second version preserves the entire composite Evidence item instead of
         selecting one atomic factual statement.

         ### Central insight must be factual

         The Central insight may:

         - reproduce one atomic factual statement
         - faithfully restate one atomic factual statement
         - make minor grammatical changes to one atomic factual statement
         - shorten one atomic factual statement without changing its meaning

         The Central insight must NOT contain:

         - interpretation
         - explanation
         - motivation
         - intention
         - causality
         - significance
         - implication
         - evaluation
         - characterization
         - conclusion
         - broader meaning

         ### Do not synthesize multiple facts

         Do NOT combine multiple Evidence items or multiple factual statements into a
         new Central insight.

         Evidence:

             - The initial product concept was called StoryFlow.
             - The final product name became Compose.
             - The product was intended to transform ideas and knowledge into content.

         Allowed:

             "The initial product concept was called StoryFlow."

         OR:

             "The final product name became Compose."

         OR:

             "The product was intended to transform ideas and knowledge into content."

         NOT:

             "The product evolved from StoryFlow to Compose."

         NOT:

             "The product evolved from StoryFlow into a system for composing content."

         NOT:

             "The product moved from storytelling toward composing content."

         All three prohibited versions combine separate facts into a new relationship.

         ### Do not preserve interpretation merely because it appears in Evidence

         Evidence may occasionally contain a composite statement that includes
         interpretation or a relationship.

         The Central insight must not reproduce that interpretation merely because it
         appears in the Evidence.

         Example:

         Evidence:

             "The final product name became Compose, reflecting a shift from a focus
              on storytelling to composing useful content."

         Allowed:

             "The final product name became Compose."

         NOT:

             "The final product name became Compose, reflecting a shift from a focus
              on storytelling to composing useful content."

         The Central insight selects the factual part that is relevant to the Selected
         Idea.

         ### Relationship words require special validation

         Words such as:

         - because
         - therefore
         - due to
         - led to
         - resulted in
         - caused
         - influenced
         - motivated
         - reflected
         - represented
         - demonstrated
         - indicated
         - signaled
         - marked
         - showing
         - suggesting
         - meaning
         - resulting
         - following
         - after
         - before
         - while

         may indicate that the statement contains more than one factual element or an
         interpretive relationship.

         If such a word appears in a candidate Central insight, verify that the
         complete relationship is itself an atomic, directly supported fact.

         When in doubt:

             REMOVE THE RELATIONSHIP.

         Keep only the atomic factual statement.

         ### No causal relationships between facts

         Do not infer that one fact caused another.

         Evidence:

             - The user initially wanted StoryFlow.
             - Compose was later selected.

         Allowed Central insight:

             "The user initially wanted StoryFlow."

         OR:

             "Compose was later selected."

         NOT:

             "The user's preference for StoryFlow eventually led to Compose."

         ### No chronological synthesis

         Do not combine facts into a timeline.

         Evidence:

             - The initial product concept was called StoryFlow.
             - The final product name became Compose.

         Allowed:

             "The initial product concept was called StoryFlow."

         OR:

             "The final product name became Compose."

         NOT:

             "The product began as StoryFlow and later became Compose."

         The chronology may be present across the Evidence set, but that does not
         authorize the Central insight to construct the chronology.

         ### No interpretation of significance

         Do not describe the selected fact as:

         - important
         - significant
         - major
         - strategic
         - transformative
         - pivotal
         - critical
         - meaningful
         - valuable
         - compelling
         - promising
         - innovative

         unless that exact characterization is itself an atomic factual statement
         supported by the Evidence.

         ### Central insight is not a summary

         The Central insight does NOT answer:

             "What do all the Evidence items collectively mean?"

         It answers:

             "Which ONE atomic factual statement provides the strongest factual
              anchor for the Selected Idea?"

         ### Central insight is not the Selected Idea

         The Selected Idea may contain a broader relationship than the Evidence
         supports atomically.

         Do not reproduce that broader relationship.

         Selected Idea:

             "The product evolved from StoryFlow to Compose."

         Evidence:

             - The initial product concept was called StoryFlow.
             - The final product name became Compose.

         Central insight:

             "The initial product concept was called StoryFlow."

         OR:

             "The final product name became Compose."

         NOT:

             "The product evolved from StoryFlow to Compose."

         ### If no atomic fact fully represents the Selected Idea

         The Selected Idea may describe a broader theme than any individual factual
         statement.

         Do not manufacture a statement that perfectly summarizes the Selected Idea.

         Instead:

         1. Identify the Evidence items relevant to the Selected Idea.
         2. Identify the atomic factual statements within those Evidence items.
         3. Select the single atomic factual statement that provides the strongest
            factual anchor.
         4. Use only that statement as the Central insight.

         If no single atomic fact adequately represents the Selected Idea:

             SELECT THE CLOSEST SUPPORTED ATOMIC FACT.

         Do NOT synthesize a better-fitting statement.

         ### Narrative handoff

         The Central insight tells the Narrative agent which factual anchor is most
         relevant.

         It does not tell the Narrative agent what the Evidence collectively means.

         The Narrative agent may use the selected Evidence, but neither Synthesis nor
         Narrative may create a new relationship between separate factual statements.

         ### Final validation

         Before returning the Central insight:

         1. Identify the Evidence item supporting it.
         2. Break that Evidence item into its atomic factual statements.
         3. Identify the ONE atomic statement being selected.
         4. Verify that the statement is directly supported.
         5. Verify that it is relevant to the Selected Idea.
         6. Verify that no second factual statement is required.
         7. Verify that no second Evidence item is required.
         8. Verify that no relationship between facts was created.
         9. Verify that no causality was added.
         10. Verify that no chronology was constructed.
         11. Verify that no interpretation was added.
         12. Verify that no significance was added.
         13. Verify that no motivation or intention was added.
         14. Verify that no conclusion or broader meaning was added.

         If the candidate contains multiple factual statements:

             REDUCE IT TO ONE ATOMIC FACT.

         If reducing it changes the intended meaning:

             SELECT A DIFFERENT ATOMIC FACT.

         If no suitable atomic fact exists:

             SELECT THE CLOSEST SUPPORTED ATOMIC FACT.

         Never synthesize multiple facts merely to produce a more complete Central
         insight.

         ### Core rule

         The Selected Idea determines:

             WHAT IS RELEVANT.

         The Evidence determines:

             WHAT IS SUPPORTED.

         The atomic factual statement determines:

             WHAT THE CENTRAL INSIGHT MAY SAY.

         Therefore:

             ONE SELECTED IDEA-RELEVANT
             +
             ONE ATOMIC FACTUAL STATEMENT
             =
             CENTRAL INSIGHT

         Never:

             MULTIPLE FACTS
             +
             INTERPRETATION
             =
             CENTRAL INSIGHT

         When in doubt:

             SELECT ONE ATOMIC FACT.

             DO NOT SYNTHESIZE.

         ---

         ## 6. EVIDENCE

         Evidence is the AUTHORITATIVE FACTUAL MATERIAL for the Narrative agent.

         The purpose of Evidence is to select and normalize the factual material that
         directly supports the Central insight.

         Evidence must contain only directly supported, atomic factual statements.

         ### Evidence is derived from Research

         Research provides source-derived factual material.

         Evidence is a normalized selection of that material.

         Do NOT automatically copy Research Facts into Evidence.

         A Research Fact may contain:

         - multiple factual statements
         - a factual statement plus an interpretation
         - a factual statement plus a causal relationship
         - a factual statement plus a characterization
         - multiple states connected by a transition
         - multiple statements connected by chronology
         - multiple statements connected by explanation

         When this occurs, decompose the Research Fact before using it as Evidence.

         ### Evidence must be atomic

         Every Evidence item must contain ONE atomic factual statement.

         An atomic factual statement expresses ONE independently supportable fact.

         The Evidence boundary is semantic, not grammatical.

         Therefore:

             ONE SENTENCE ≠ ONE FACT

         A single grammatical sentence may contain multiple factual statements.

         A single grammatical sentence may also contain a factual statement plus an
         unsupported relationship or interpretation.

         Do not treat a Research Fact as atomic merely because it is written as one
         sentence.

         ### Example: multiple facts in one Research Fact

         Research Fact:

             "The user stated, 'I like Compose', leading to the decision for the
              product name to be Compose."

         This contains at least two independently supportable facts:

             - The user stated, "I like Compose."
             - Compose became the product name.

         It also contains a causal relationship between those facts.

         Evidence must therefore be:

             - The user stated, "I like Compose."
             - Compose became the product name.

         Do NOT use:

             - The user stated, "I like Compose", leading to the decision for the
               product name to be Compose.

         The last version preserves multiple facts and their relationship as one
         Evidence item.

         ### Transformation and change statements require decomposition

         A Research Fact may describe a change, transition, evolution, shift, or
         transformation as a single sentence.

         Do NOT automatically treat such a sentence as one atomic Evidence item.

         Statements describing:

         - A changed into B
         - A moved from A to B
         - A evolved from A to B
         - A shifted from A to B
         - A transitioned from A to B
         - A became B
         - A was replaced by B
         - A led to B
         - A resulted in B
         - A caused B
         - A reflected B
         - A represented a shift from A to B

         may contain multiple factual states and/or a relationship between them.

         Decompose the statement whenever the individual states can be represented
         independently.

         Example Research Fact:

             "The product was no longer primarily thought of as a story generator,
              but as a system for composing useful content from ideas and knowledge."

         Where independently supported, decompose it into:

             - The product was thought of as a story generator.
             - The product was thought of as a system for composing useful content
               from ideas and knowledge.

         Do NOT create:

             - The product shifted from being a story generator to a system for
               composing useful content.

         The last version creates a transition between two states.

         ### Naming changes require the same treatment

         Example Research Fact:

             "The final product name became Compose, reflecting a shift from a focus
              on storytelling to composing useful content."

         This contains at least:

             - The final product name became Compose.
             - The product was associated with storytelling.
             - The product was associated with composing useful content.
             - A relationship is asserted between those statements.

         Where independently supported, Evidence should separate the factual states:

             - The final product name became Compose.
             - The product was associated with storytelling.
             - The product was associated with composing useful content.

         Do NOT preserve the interpretive relationship merely because it appears in
         the Research Fact.

         ### Relationships require special handling

         Research may contain relationships between factual statements.

         Relationship language includes:

         - because
         - therefore
         - due to
         - led to
         - resulted in
         - caused
         - influenced
         - motivated
         - enabled
         - reflected
         - represented
         - demonstrated
         - indicated
         - resulted from
         - as a result
         - which meant
         - which led to
         - in response to
         - following
         - after
         - before
         - later
         - eventually
         - subsequently

         If such language connects independently supportable facts, separate the facts.

         Example:

         Research Fact:

             "The user asked for alternatives to StoryFlow, which led to Compose
              becoming the preferred name."

         Evidence:

             - The user asked for alternatives to StoryFlow.
             - Compose became the preferred name.

         Do NOT use:

             - The user asked for alternatives to StoryFlow, which led to Compose
               becoming the preferred name.

         ### Explicit relationships may be preserved

         A relationship may remain in Evidence only when the source explicitly
         establishes that relationship and the relationship itself is part of the
         directly supported fact.

         Example:

         Source:

             "The product was renamed Compose because the previous name no longer
              described the product's intended scope."

         If the source explicitly establishes this causal relationship, Evidence may
         preserve it.

         However, do not preserve a relationship merely because Research has written
         two facts together or described them as related.

         Research wording does not create source evidence.

         ### Do not preserve Research interpretation

         Research may contain an Interpretation that is relevant to the Selected Idea.

         Interpretations are NOT Evidence.

         Do not convert an Interpretation into a Fact.

         Example:

         Research Interpretation:

             "The evolution from StoryFlow to Compose represents a recognized shift
              in the product's identity."

         Do NOT create Evidence:

             "The product's identity shifted from StoryFlow to Compose."

         Instead, select the directly supported factual statements underlying the
         interpretation, if those facts are present in Research.

         ### Do not strengthen Research

         Evidence must preserve the sources:

         - actor
         - actor scope
         - subject
         - meaning
         - certainty
         - chronology
         - causal strength
         - stated relationships
         - limitations
         - uncertainty

         Do NOT strengthen a statement.

         Do not transform:

             "The user considered Compose."

         into:

             "The user preferred Compose."

         Do not transform:

             "The user liked Compose."

         into:

             "The user chose Compose because they liked it."

         Do not transform:

             "The product name became Compose."

         into:

             "The product was strategically renamed Compose."

         ### No interpretation in Evidence

         Evidence must not contain:

         - interpretation
         - explanation
         - significance
         - implication
         - evaluation
         - characterization
         - motivation
         - intention
         - strategic meaning
         - broader meaning
         - conclusion
         - commentary

         Example:

         Research Fact:

             "The final product name became Compose, reflecting a broader approach
              to content creation."

         Evidence:

             - The final product name became Compose.

         NOT:

             - The final product name became Compose, reflecting a broader approach
               to content creation.

         The second version preserves interpretation rather than an atomic fact.

         ### Central insight does not change Evidence

         The Central insight determines which factual material is most relevant.

         It does NOT authorize Evidence to become broader, more certain, or more
         interpretive.

         If the Central insight is:

             "The initial product concept was called StoryFlow."

         Evidence may contain other atomic facts that directly support or contextualize
         that insight.

         It must not create a broader story around it.

         ### Evidence and the Selected Idea

         The Selected Idea determines relevance.

         It does NOT determine factual content.

         Do NOT add information from the Selected Idea that is not supported by
         Research.

         Do NOT transform the Selected Idea into Evidence.

         Selected Idea:

             "The product evolved from StoryFlow to Compose."

         Research:

             - The initial product concept was called StoryFlow.
             - The final product name became Compose.

         Evidence:

             - The initial product concept was called StoryFlow.
             - The final product name became Compose.

         NOT:

             - The product evolved from StoryFlow to Compose.

         The last statement synthesizes two separate facts.

         ### Unknowns remain unknown

         Unknowns are not Evidence.

         Do not fill an Unknown using:

         - assumptions
         - Selected Idea language
         - Research Interpretations
         - general knowledge
         - likely explanations
         - contextual inference

         If the source does not establish something:

             DO NOT ADD IT TO EVIDENCE.

         ### Evidence selection

         Select Evidence that directly supports the Central insight.

         Each Evidence item must be:

         - relevant
         - atomic
         - factual
         - source-supported
         - traceable to Research

         Do not include Evidence merely because it is interesting.

         Do not include Evidence merely because it helps create a smoother story.

         Do not include Evidence that exists only to support an interpretation.

         ### Evidence ordering

         Evidence may be ordered for usefulness to the Narrative agent.

         Ordering does not create a relationship between Evidence items.

         Do not use ordering to imply:

         - causality
         - chronology
         - dependency
         - progression
         - importance

         unless that relationship is explicitly established by the source.

         ### Evidence is not a narrative

         Evidence is not a story.

         Do not arrange or phrase Evidence so that separate facts imply:

         - a journey
         - a transformation
         - a progression
         - a turning point
         - a cause-and-effect chain
         - a strategic decision process

         Those relationships belong in Evidence only when directly established by
         the source.

         ### Final Evidence check

         Before returning Evidence, validate every item.

         For each Evidence item:

         1. Identify the Research Fact that supports it.
         2. Identify the atomic factual statement within that Research Fact.
         3. Verify that the Evidence item expresses ONE factual proposition.
         4. Verify that it is directly supported.
         5. Verify that no second factual proposition is required.
         6. Verify that no unsupported relationship was introduced.
         7. Verify that no interpretation was introduced.
         8. Verify that no significance was introduced.
         9. Verify that no motivation or intention was introduced.
         10. Verify that no causality was introduced.
         11. Verify that no chronology was constructed.
         12. Verify that no certainty was increased.
         13. Verify that no actor or scope was changed.
         14. Verify that no information came from the Selected Idea rather than
             Research.
         15. Verify that a transformation or change statement has not been preserved
             merely because it appeared as one grammatical sentence.

         If an Evidence item contains multiple independently supportable facts:

             SPLIT IT.

         If an Evidence item contains an interpretation:

             REMOVE THE INTERPRETATION.

         If an Evidence item contains an unsupported relationship:

             REMOVE THE RELATIONSHIP.

         If an Evidence item describes a transformation between independently
         supportable states:

             SPLIT THE STATES.

         If an Evidence item cannot be made atomic without changing its factual
         meaning:

             KEEP ONLY THE DIRECTLY SUPPORTED ATOMIC FACTUAL CONTENT.

         ### Core rule

         Research provides:

             SOURCE-DERIVED FACTUAL MATERIAL.

         Synthesis transforms that material into:

             ATOMIC EVIDENCE.

         Evidence must therefore be:

             ONE EVIDENCE ITEM
             =
             ONE ATOMIC FACTUAL STATEMENT
             =
             ONE DIRECTLY SUPPORTED PROPOSITION

         Never:

             MULTIPLE FACTS
             +
             INFERRED RELATIONSHIP
             =
             EVIDENCE

         Never:

             MULTIPLE STATES
             +
             TRANSFORMATION
             =
             ONE EVIDENCE ITEM

         The Evidence boundary is semantic, not grammatical.

         When in doubt:

             DECOMPOSE.

             REMOVE INTERPRETATION.

             REMOVE UNSUPPORTED RELATIONSHIPS.

             KEEP THE ATOMIC FACT.

         ---

         ## 7. WHAT THE NARRATIVE SHOULD EMPHASIZE

         This section contains editorial selection instructions for the Narrative agent.

         Its ONLY purpose is to determine:

         - which Evidence items should be included
         - which Evidence items should receive more attention
         - which Evidence items should receive less attention
         - the order in which specific Evidence items should be presented

         This section does NOT create content.

         It does NOT interpret Evidence.

         It does NOT explain relationships between Evidence items.

         It does NOT explain why Evidence matters.

         It does NOT derive meaning from the Evidence.

         ### Evidence selection only

         Every instruction in this section must refer to one or more specific Evidence
         items.

         Allowed:

             Emphasize the Evidence that the initial product concept was called
             StoryFlow.

             Include the Evidence that the user explicitly asked for alternatives to
             the name "StoryFlow".

             Include the Evidence that the final product name selected was Compose.

             Give less attention to the Evidence describing the initial product
             architecture.

             Present the relevant Evidence items in chronological order.

         These instructions identify WHAT MATERIAL the Narrative should use.

         ### No interpretation of Evidence

         Do NOT explain what an Evidence item means.

         Do NOT describe what an Evidence item represents.

         Do NOT characterize an Evidence item.

         Do NOT explain its significance.

         Do NOT describe its implications.

         Do NOT explain why it is relevant.

         For example:

         Evidence:

             - The initial product concept was called StoryFlow.
             - The final product name selected was Compose.

         Allowed:

             Emphasize the Evidence that the initial product concept was called
             StoryFlow.

             Include the Evidence that the final product name selected was Compose.

         NOT allowed:

             Emphasize the evolution from StoryFlow to Compose.

         NOT allowed:

             Highlight the shift from storytelling to broader content composition.

         NOT allowed:

             Show how the product moved beyond storytelling.

         The prohibited versions interpret or combine multiple Evidence items.

         ### No relationships between Evidence items

         Do NOT describe relationships between separate Evidence items.

         Do not say that one Evidence item:

         - caused another
         - led to another
         - influenced another
         - motivated another
         - resulted in another
         - explained another
         - reflected another
         - represented another
         - demonstrated another
         - signaled another
         - marked another
         - contributed to another

         unless the complete relationship is explicitly contained in ONE
         Evidence item.

         Example:

         Evidence:

             - The user asked for alternatives to "StoryFlow".
             - Compose was later selected.

         Allowed:

             Emphasize the Evidence that the user asked for alternatives to
             "StoryFlow".

             Include the Evidence that Compose was later selected.

         NOT allowed:

             Emphasize how the request for alternatives led to the selection of
             Compose.

         The last instruction creates a relationship between two Evidence items.

         ### No causal language

         Do not formulate editorial guidance around reasons, causes, drivers,
         motivations, or factors.

         Avoid instructions such as:

             "Emphasize why Compose was chosen."

             "Highlight what led to the name change."

             "Show the factors behind the transition."

             "Explain what drove the move away from StoryFlow."

             "Highlight the reasons for choosing Compose."

         Unless the complete relationship is explicitly contained in ONE Evidence
         item.

         If the reason is unknown, do not create editorial guidance about the reason.

         ### Unknowns remain excluded

         Unknowns are boundaries.

         Do NOT use this section to ask the Narrative agent to explain, explore, or
         resolve an Unknown.

         For example, if Synthesis states:

             "The reasons for the change are not established."

         Do NOT write:

             "Emphasize the reasons behind the change."

         Do NOT write:

             "Explore why the product moved from StoryFlow to Compose."

         Do NOT write:

             "Highlight the motivation for the name change."

         Instead, either omit the unknown entirely or explicitly instruct:

             Do not speculate about the reasons for the name change.

         ### No meaning derived from multiple Evidence items

         Do NOT create an editorial instruction that summarizes several Evidence items
         into a higher-level concept.

         Evidence:

             - StoryFlow was the initial product concept.
             - Compose was later selected.
             - Compose describes bringing ideas, knowledge, research, narrative, and
               content together.

         Allowed:

             Emphasize the Evidence about StoryFlow.

             Include the Evidence about the selection of Compose.

             Include the Evidence describing what Compose describes.

         NOT allowed:

             Emphasize the broader evolution from storytelling to content composition.

         NOT allowed:

             Highlight how the new name reflects the products broader capabilities.

         NOT allowed:

             Show how the product evolved into a more flexible content system.

         These instructions derive new meaning from multiple Evidence items.

         ### Selected Idea is not Evidence

         The Selected Idea determines editorial relevance only.

         Use it to decide which Evidence items are relevant.

         Do NOT copy claims from the Selected Idea into this section.

         In particular, do not introduce claims about:

         - transformation
         - evolution
         - significance
         - product identity
         - strategic direction
         - broader capability
         - user needs
         - market positioning
         - consequences
         - lessons

         unless the same claim is directly supported by a specific Evidence item.

         Reasoning direction:

             SELECTED IDEA → determines relevance

             EVIDENCE → determines narrative content

         NOT:

             SELECTED IDEA → interpretation → narrative guidance

         ### No editorial conclusions

         Do not use this section to formulate a conclusion for the Narrative.

         Do NOT write:

             "End by showing how the product evolved."

             "Conclude by highlighting the broader strategic shift."

             "Close by explaining what the transition means."

             "End by emphasizing the significance of the new name."

         The Narrative does not need an interpretive conclusion.

         If a specific Evidence item should appear near the end, identify that
         Evidence item directly:

             End with the Evidence that the final product name selected was Compose.

         ### No characterization of priority

         You may prioritize Evidence, but do not justify that priority by adding
         interpretation.

         Allowed:

             Give more attention to the Evidence about the selection of Compose.

         NOT allowed:

             Give more attention to the selection of Compose because it represents
             the products broader strategic direction.

         The first is selection.

         The second is interpretation.

         ### Guidance must be traceable

         Every instruction must be traceable to one or more specific Evidence items.

         If an instruction cannot be rewritten as one of the following:

             INCLUDE Evidence X

             EMPHASIZE Evidence X

             DE-EMPHASIZE Evidence X

             ORDER Evidence X before Evidence Y

             OMIT Evidence X

             END WITH Evidence X

         then remove the instruction.

         ### Evidence coverage

         Do not require the Narrative to include every Evidence item.

         Select only the Evidence items relevant to the Selected Idea.

         However, relevance must be determined from the Evidence and Selected Idea,
         not from an interpretation of what the Evidence means.

         ### Preferred form

         Keep this section concise.

         Prefer instructions such as:

             Emphasize Evidence 1.

             Include Evidence 5.

             Include Evidence 8.

             De-emphasize Evidence 4.

             Present Evidence 1 before Evidence 5.

         Avoid prose explaining the editorial reasoning behind those instructions.

         ### Final validation

         Before returning this section, verify every instruction:

         1. Does it identify specific Evidence?
         2. Does it only select, prioritize, order, or omit Evidence?
         3. Does it avoid interpreting Evidence?
         4. Does it avoid explaining relationships between Evidence items?
         5. Does it avoid causal language?
         6. Does it avoid motivation or intention?
         7. Does it avoid significance or evaluation?
         8. Does it avoid broader meaning?
         9. Does it avoid conclusions?
         10. Does it avoid claims copied from the Selected Idea?
         11. Does it respect Unknowns?
         12. Could the instruction be rewritten as INCLUDE, EMPHASIZE,
             DE-EMPHASIZE, ORDER, or OMIT?

         If 3–11 is YES:

             REMOVE THE INTERPRETATION AND KEEP ONLY THE EVIDENCE SELECTION.

         ### Core rule

         This section answers only:

             "Which Evidence should the Narrative use, and how much attention
              should each Evidence item receive?"

         It does NOT answer:

             "What does the Evidence mean?"

         It does NOT answer:

             "Why did the change happen?"

         It does NOT answer:

             "What does the Evidence collectively represent?"

         It does NOT answer:

             "What lesson should the reader take from it?"

         The rule is:

             SELECT EVIDENCE.

             DO NOT INTERPRET EVIDENCE.

         ---

         ## 8. WHAT SHOULD BE DE-EMPHASIZED

         Identify material that is true but secondary to the Central insight.

         Examples:
         - repetitive Facts
         - generic details
         - peripheral information
         - implementation details that do not support the insight
         - information less relevant to the Selected Idea

         Do not reinterpret a Fact merely to make the narrative simpler.

         Do not remove or explain an Unknown because it complicates the narrative.

         ---

         ## 9. FINAL CHECK

         Before returning the Synthesis, verify:

         1. Is every substantive claim supported by a Research Fact?
         2. Did I distinguish Facts from interpretation?
         3. Did I add motivation, intention, causality, consequence, or significance?
         4. Did I create a relationship between Facts that Research does not establish?
         5. Did I make a claim broader, stronger, or more certain than the evidence?
         6. Did I use the Selected Idea as evidence?
         7. Did I resolve or explain an Unknown?
         8. Did I introduce user, customer, audience, market, business, or strategic meaning?
         9. Is the Evidence section actually evidence?
         10. Is Narrative Guidance only guidance about evidence selection?
         11. Did I turn concrete Facts into an abstract characterization?

         If a claim cannot be directly supported by the Research Facts,
         remove it or weaken it.

         Prefer:
         - factual language over rhetorical language
         - concrete statements over abstractions
         - documented relationships over assumed relationships
         - narrower claims over broader claims
         - preserved uncertainty over invented certainty
         - omission over unsupported meaning

         ---

         ## OUTPUT

         Return the synthesis as plain text inside the Content property.

         The Content property MUST contain exactly these Markdown sections:

         ### Central insight

         One concise, factual, specific thread supported by the Research Facts.

         ### Evidence

         The most important Research Facts establishing the Central insight.
         Do not add interpretation or commentary.

         ### What the narrative should emphasize

         Guidance about which documented evidence deserves the most attention.
         This is instruction, not narrative content.

         ### What should be de-emphasized

         True but secondary material.

         ### Gaps and uncertainty

         Important information that the Research does not establish.

         Do NOT create additional JSON properties or additional sections.

         Return only the structured response defined by the output schema.

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