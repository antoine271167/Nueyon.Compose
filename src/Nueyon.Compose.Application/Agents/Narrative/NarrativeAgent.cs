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

    /// <summary>
    ///     Gets the system instructions used to configure this agent's underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() => NarrativeAgentInstructions.GetSystemInstructions();

    private static string CreateUserMessage(NarrativeInput input) =>
        $"""
         Transform the editorial Synthesis into a coherent narrative.

         The Synthesis is the PRIMARY SOURCE and the COMPLETE SEMANTIC BOUNDARY
         of the narrative.

         Your task is to decide HOW the supplied material should be communicated,
         not WHAT it means.

         CORE PRINCIPLE:

             NARRATIVE MAY TRANSFORM EXPRESSION,
             BUT MUST NOT TRANSFORM MEANING.

         You may improve:
         - structure
         - sequence
         - wording
         - paragraph organization
         - transitions
         - pacing
         - readability
         - narrative flow

         These changes must affect expression only.

         Do not add, remove, strengthen, broaden, interpret, explain, evaluate,
         or reframe the meaning of the Synthesis.

         ---

         ## 1. SYNTHESIS SECTIONS HAVE DIFFERENT ROLES

         The Synthesis contains both SOURCE MATERIAL and EDITORIAL GUIDANCE.

         These have different roles and must not be treated as equivalent content.

         ### Central insight

         This defines the central thread of the narrative.

         Use it to understand what the narrative is about.

         Do not:
         - strengthen it
         - broaden it
         - reinterpret it
         - turn it into a stronger conclusion
         - add significance that is not explicitly established

         The Central insight is a guide to the narrative's focus, not permission
         to add meaning.

         ### Evidence

         This is the PRIMARY NARRATIVE MATERIAL.

         Substantive claims in the narrative must be grounded in the Evidence.

         You may:
         - select relevant evidence
         - order evidence
         - combine closely related evidence
         - restate evidence in clearer language
         - use evidence to construct a coherent narrative

         You may not add meaning beyond what the Evidence establishes.

         ### What the narrative should emphasize

         This section is EDITORIAL INSTRUCTION, NOT NARRATIVE CONTENT.

         Use it only to determine:
         - which supplied evidence deserves more attention
         - which supplied evidence should appear earlier
         - which supplied evidence should receive more space
         - which supplied evidence forms the main narrative thread

         DO NOT reproduce these instructions as statements in the narrative.

         For example:

             Synthesis guidance:
             "The transition from A to B should be emphasized."

         Do NOT write:

             "The transition from A to B should be emphasized."

         Instead, structure the narrative so that the supported facts about
         the transition receive appropriate attention.

         Do not convert editorial guidance into:
         - factual claims
         - evaluations
         - explanations
         - implications
         - conclusions
         - significance
         - motivations
         - causal relationships

         Words such as "important", "significant", "relevant", "implication",
         or "broader" in editorial guidance do not automatically become claims
         that may appear in the narrative.

         ### What should be de-emphasized

         This section is also EDITORIAL INSTRUCTION, NOT NARRATIVE CONTENT.

         Use it only to determine which supplied evidence receives less attention.

         Do NOT:
         - reproduce the instruction
         - explain what is being de-emphasized
         - turn the instruction into a factual statement
         - turn the instruction into an evaluation
         - introduce information merely because it appears in this section

         ### Gaps and uncertainty

         This section defines BOUNDARIES.

         Preserve relevant uncertainty when it matters to the narrative.

         Do not:
         - resolve gaps
         - explain unknowns
         - infer missing information
         - replace uncertainty with a conclusion
         - use outside knowledge to complete the story

         UNKNOWN REMAINS UNKNOWN.

         ---

         ## 2. EVIDENCE IS THE SOURCE OF NARRATIVE CLAIMS

         Only the `Evidence` section is authoritative source material for substantive
         narrative claims.

         The other Synthesis sections have different roles:

         - `Central insight` provides orientation.
         - `Evidence` provides the factual material.
         - `What the narrative should emphasize` provides editorial instructions.
         - `What should be de-emphasized` provides editorial instructions.
         - `Gaps and uncertainty` defines boundaries.

         Use `Central insight` to understand the main thread, but do not use it to
         introduce claims that are not explicitly supported by Evidence.

         Use narrative guidance to decide which Evidence to emphasize, not to create
         new claims.

         When Central insight or narrative guidance contains a characterization,
         interpretation, evaluation, implication, or abstraction that is not present
         in Evidence, do NOT reproduce it as narrative content.

         When sections conflict, Evidence takes precedence.

         Every substantive statement in the narrative must be traceable to Evidence,
         not merely to Central insight or editorial guidance.

         ---

         ## 3. SOURCE FIDELITY

         Synthesis is the semantic boundary for Narrative.

         The Narrative agent may reorganize, sequence, connect, and express the
         information in Synthesis, but it MUST NOT add meaning.

         ### Evidence is the authoritative source for substantive claims

         Within Synthesis:

         - Evidence contains the authoritative factual material.
         - Central insight identifies the factual thread.
         - Narrative guidance controls emphasis and ordering.
         - Gaps and uncertainty define what is not established.

         For every substantive statement in the Narrative, identify the specific
         Evidence item that supports it.

         If no specific Evidence item supports a statement, do NOT write that
         statement.

         Do not treat the combined Evidence as support for a new conclusion.

         ### Direct support is required

         A statement is directly supported only when the Evidence explicitly states
         the same fact or a faithful restatement of it.

         Do NOT derive a new statement by combining multiple Evidence items.

         Example Evidence:

             - The product began as StoryFlow.
             - The product was later named Compose.
             - Compose describes bringing ideas, knowledge, research, narrative, and
               content together.

         Allowed:

             "The product began as StoryFlow."

             "The product was later named Compose."

             "Compose describes bringing ideas, knowledge, research, narrative, and
             content together."

         Not allowed:

             "The product evolved from a storytelling product into a broader content
             composition system."

         The last statement combines multiple Evidence items and introduces a
         conclusion that no individual Evidence item establishes.

         ### Do not add interpretive transitions

         A transition between two factual statements must not explain what the
         relationship between those statements means.

         Allowed:

             "The product began as StoryFlow. It was later named Compose."

         Not allowed:

             "The product began as StoryFlow. This marked a significant shift in
             the product's identity."

         The second sentence is an interpretation.

         Likewise, do not introduce statements using:

         - this reflected
         - this represented
         - this demonstrated
         - this showed
         - this indicated
         - this signaled
         - this marked
         - this highlighted
         - this illustrated
         - this revealed
         - this signified
         - this underscored
         - this demonstrated the importance of
         - this represented a broader
         - this reflected a deeper understanding

         unless the complete statement is explicitly supported by a specific
         Evidence item.

         ### No inferred relationships

         Do not infer relationships between Evidence items simply because they:

         - appear in chronological order
         - concern the same subject
         - appear to explain each other
         - are logically compatible
         - form a plausible story

         For example:

         Evidence:
             - The user asked for alternatives to names containing "story".
             - Compose was later selected.

         Allowed:

             "The user asked for alternatives to names containing 'story'.
              Compose was later selected."

         Not allowed:

             "The request for alternatives led to the selection of Compose."

         The second statement introduces causality.

         ### Preserve actor and scope

         Preserve the actor specified by the Evidence.

         Do not change:

             "The user stated..."

         into:

             "The team decided..."

         or:

             "The creators wanted..."

         or:

             "Users preferred..."

         Do not broaden an individual statement into a statement about a group,
         company, customers, or users.

         ### Preserve uncertainty

         If Synthesis identifies something as unknown or uncertain, Narrative must
         not resolve it through wording.

         Do not turn:

             "The reasons for the change are not established."

         into:

             "The change happened because..."

         Do not infer missing motivations, intentions, causes, consequences, or
         significance.

         ### Core rule

         Narrative may transform EXPRESSION.

         Narrative must NOT transform MEANING.

         If a sentence sounds more insightful, more significant, more strategic, or
         more explanatory than its supporting Evidence, make the sentence narrower.

         ---

         ## 4. CLAIM-LEVEL EVIDENCE RULE

         Every substantive Narrative sentence must be derived from ONE specific
         Evidence item.

         Narrative is a structural transformation of Evidence.

         It is NOT an interpretation layer.

         The Narrative agent may change the wording of an Evidence item for grammar,
         readability, and sentence structure, but it must preserve the meaning of the
         Evidence exactly.

         ### Two permitted sentence types

         The Narrative agent may produce only:

         1. DIRECT EVIDENCE STATEMENT
         2. NEUTRAL STRUCTURAL STATEMENT

         No other substantive sentence type is allowed.

         ### Type 1 — Direct Evidence Statement

         A Direct Evidence Statement is a faithful restatement of ONE specific
         Evidence item.

         Example:

         Evidence:
             "The initial product concept was called StoryFlow."

         Allowed:

             "The initial product concept was called StoryFlow."

         Minor grammatical changes are allowed.

         However, the Narrative must preserve the Evidence item's:

         - meaning
         - actor
         - scope
         - certainty
         - relationship
         - chronology
         - specificity

         Do NOT strengthen, weaken, generalize, characterize, interpret, or evaluate
         the Evidence.

         ### Surface-Level Transformation Only

         When rewriting an Evidence item, change only its surface expression.

         Allowed changes include:

         - grammar
         - punctuation
         - sentence structure
         - removing unnecessary repetition
         - replacing a word with a direct grammatical equivalent
         - combining words into a grammatically natural sentence

         Do NOT change the semantic content.

         Example:

         Evidence:
             "The user explicitly asked for alternatives that did not necessarily
              contain the word 'story'."

         Allowed:

             "The user asked for alternatives that did not necessarily contain the
              word 'story'."

         NOT allowed:

             "The user wanted to move away from storytelling."

         The second version interprets the Evidence rather than restating it.

         Example:

         Evidence:
             "The user wanted Noëyon to start with StoryFlow, as this concept appeared
              to have the most potential."

         Allowed:

             "The user wanted Noëyon to start with StoryFlow, as this concept appeared
              to have the most potential."

         NOT allowed:

             "The user wanted to prioritize StoryFlow because it showed the most
              promise."

         The second version changes the characterization of the Evidence.

         ### Type 2 — Neutral Structural Statement

         A Neutral Structural Statement may organize separately supported Evidence
         items without adding meaning.

         Example:

             "The initial product concept was called StoryFlow."

             "Later, the product name was discussed."

             "Compose was subsequently selected as the preferred name."

         Each statement must still be directly supported by ONE Evidence item.

         Structural wording must not imply a relationship that is not explicitly
         contained in the Evidence.

         ### Single-Evidence Rule

         Every substantive sentence must be supported by exactly ONE Evidence item.

         Do NOT construct a sentence from two or more Evidence items.

         Do NOT summarize multiple Evidence items in a single sentence.

         Example:

         Evidence:

             - The initial product concept was called StoryFlow.
             - Compose was later selected as the product name.

         Allowed:

             "The initial product concept was called StoryFlow."

             "Compose was later selected as the product name."

         NOT allowed:

             "The product evolved from StoryFlow to Compose."

         The last sentence combines two Evidence items and creates a new relationship.

         ### No Factual Compression

         Do not compress multiple Evidence items into a shorter higher-level
         statement.

         Evidence:

             - StoryFlow was envisioned as a system for transforming ideas and
               knowledge into stories and content.
             - Compose describes bringing ideas, knowledge, research, narrative, and
               content together.

         Allowed:

             "StoryFlow was envisioned as a system for transforming ideas and
              knowledge into stories and content."

             "Compose describes bringing ideas, knowledge, research, narrative, and
              content together."

         NOT allowed:

             "The product evolved from transforming ideas into stories to composing
              broader content."

         The last sentence creates a new interpretation from multiple Evidence items.

         ### One Evidence Item May Contain a Relationship

         If ONE Evidence item explicitly contains a relationship, that relationship
         may be preserved.

         Example:

         Evidence:
             "The user expressed liking the name Compose, leading to it becoming the
              product name."

         Allowed:

             "The user expressed liking the name Compose, leading to it becoming the
              product name."

         Do NOT weaken, strengthen, or reinterpret that relationship.

         NOT allowed:

             "User feedback played a significant role in the naming process."

         The second sentence evaluates the importance of the relationship.

         ### No Interpretation of Evidence

         Do not explain what an Evidence item means.

         Do not characterize its importance.

         Do not describe its significance.

         Do not infer its implications.

         Do not evaluate its role.

         Example:

         Evidence:
             "The product was no longer primarily thought of as a 'story generator',
              but rather as a system for composing useful content from ideas and
              knowledge."

         Allowed:

             "The product was no longer primarily thought of as a 'story generator',
              but rather as a system for composing useful content from ideas and
              knowledge."

         NOT allowed:

             "This represented a broader content strategy."

         The second sentence interprets the Evidence.

         ### No Narrative Framing

         Do not add narrative framing that is not directly supported by an Evidence
         item.

         Do NOT introduce the Evidence with phrases such as:

             "The journey began..."
             "The story began..."
             "Early on..."
             "As development progressed..."
             "As conversations progressed..."
             "Over time..."
             "At this point..."
             "This marked..."
             "This represented..."
             "This reflected..."
             "This demonstrated..."
             "This highlighted..."
             "This revealed..."
             "This showed..."
             "This illustrated..."
             "This signaled..."

         These phrases often create chronology, significance, causality, or
         interpretation that is not explicitly supported.

         Use the Evidence statements directly instead.

         ### No Narrative Conclusions

         Do not add conclusions that summarize what the Evidence collectively means.

         Do NOT write:

             "This transition reflects a change in the product's conceptual
              framework."

             "Together, these changes show a broader strategic evolution."

             "The development illustrates a shift in product strategy."

             "In conclusion, the product evolved into a broader content platform."

         These statements combine or interpret Evidence.

         A Narrative may end after the final supported Evidence statement.

         It does not need a conclusion.

         ### Narrative May End With Evidence

         The Narrative does not need a concluding statement.

         The final Narrative sentence must be either:

         - a Direct Evidence Statement, or
         - a Neutral Structural Statement

         Do not add a final sentence merely to summarize, characterize, evaluate, or
         give meaning to the preceding Evidence.

         After the final supported Evidence statement, STOP.

         Do NOT add closing statements such as:

             "This marks..."
             "This represents..."
             "This illustrates..."
             "This demonstrates..."
             "This highlights..."
             "This shows..."
             "This reflects..."
             "Overall..."
             "In conclusion..."

         unless the complete statement is directly supported by ONE specific Evidence
         item.

         A Narrative is complete when the relevant Evidence has been structurally
         presented.

         It does not require a conclusion.

         ### No Introductory Commentary

         Do not add introductory sentences merely to make the Narrative sound more
         like an article.

         Do NOT write:

             "The development of a product often involves changes in vision."

             "Product naming can play an important role in shaping a company's
              direction."

             "The journey of product development is rarely linear."

         Such statements are not Evidence.

         Start directly with supported Evidence.

         ### No Evaluative Language

         Do not add adjectives or descriptions that evaluate or characterize the
         Evidence.

         Do not introduce words such as:

         - significant
         - important
         - notable
         - major
         - substantial
         - pivotal
         - critical
         - strategic
         - transformative
         - meaningful
         - fundamental
         - comprehensive
         - sophisticated
         - valuable
         - promising
         - successful
         - influential
         - consequential

         unless the exact characterization is contained in ONE specific Evidence
         item.

         Do not replace these words with equivalent wording that performs the same
         semantic function.

         ### No Inferred Relationships

         Do not infer relationships between Evidence items because they:

         - appear in chronological order
         - concern the same subject
         - appear next to each other
         - seem logically connected
         - appear to explain each other
         - form a plausible story
         - support the same Selected Idea

         Example:

         Evidence:

             - The user asked for alternatives to names containing "story".
             - Compose was later selected.

         Allowed:

             "The user asked for alternatives to names containing 'story'."

             "Compose was later selected."

         NOT allowed:

             "The request for alternatives led to the selection of Compose."

         ### Central Insight Is Not Evidence

         The Central insight is an orientation mechanism.

         It may help the Narrative agent decide which Evidence to include and how to
         order it.

         It is NOT a source of factual content.

         Every substantive Narrative sentence must be supported by a specific Evidence
         item.

         If a statement appears in the Central insight but cannot be traced to ONE
         specific Evidence item, do not use that statement in the Narrative.

         The Central insight must never be used to justify combining Evidence items.

         ### Preserve Actors

         Preserve the actor specified by the Evidence.

         Do not change:

             "The user stated..."

         into:

             "The team decided..."

         or:

             "The creators wanted..."

         or:

             "Users preferred..."

         Do not broaden an individual statement into a statement about a group,
         company, customers, or users.

         ### Preserve Uncertainty

         If Synthesis identifies something as unknown or uncertain, Narrative must
         not resolve it.

         Do not turn:

             "The reasons for the change are not established."

         into:

             "The change happened because..."

         Do not infer:

         - motivation
         - intention
         - causality
         - consequence
         - significance

         Unknowns remain unknown.

         ### No Interpretation of Names

         If Evidence says:

             "Compose describes bringing ideas, knowledge, research, narrative, and
              content together."

         Allowed:

             "Compose describes bringing ideas, knowledge, research, narrative, and
              content together."

         NOT allowed:

             "The name Compose captures the product's broader purpose."

         NOT allowed:

             "The name Compose reflects the product's evolution."

         NOT allowed:

             "Compose represents the product's broader strategic direction."

         These statements interpret the Evidence.

         ### Sentence-Level Validation

         Before returning the Narrative, validate every substantive sentence.

         For each sentence:

         1. Identify exactly ONE Evidence item that supports it.
         2. Verify that the sentence preserves that Evidence item's meaning.
         3. Verify that no second Evidence item is required.
         4. Verify that no new relationship has been created.
         5. Verify that no interpretation has been added.
         6. Verify that no significance or evaluation has been added.
         7. Verify that no causality has been added.
         8. Verify that no motivation or intention has been added.
         9. Verify that no conclusion has been derived.
         10. Verify that the Central insight is not being used as its factual source.
         11. Verify that the actor and scope are unchanged.
         12. Verify that uncertainty is unchanged.
         13. Verify that the sentence is not merely narrative framing or commentary.
         14. Verify that no concluding statement has been added merely to close the
             Narrative.

         If a sentence requires two or more Evidence items:

             SPLIT THE SENTENCE.

         If a sentence changes the meaning of an Evidence item:

             RESTORE THE ORIGINAL MEANING.

         If a sentence explains what the Evidence means:

             REMOVE THE INTERPRETATION.

         If a sentence evaluates the Evidence:

             REMOVE THE EVALUATION.

         If a sentence exists only to make the text sound more like a story:

             REMOVE THE SENTENCE.

         If the Narrative has reached the final supported Evidence statement:

             STOP.

         ### Output Preference

         Prefer:

             several simple, directly supported sentences

         over:

             one elegant sentence that combines or interprets multiple Evidence items.

         Prefer:

             direct factual statements

         over:

             narrative framing.

         Prefer:

             explicit Evidence

         over:

             implied relationships.

         Prefer:

             semantic fidelity

         over:

             rhetorical quality.

         Prefer:

             ending with the final supported Evidence statement

         over:

             adding a conclusion.

         ### Core Rule

         Narrative structures Evidence.

         Narrative may transform EXPRESSION.

         Narrative must NOT transform MEANING.

         Narrative must not tell the reader:

             what the Evidence means

             why the Evidence matters

             how separate Evidence items relate

             what conclusion should be drawn

         Narrative should only tell the reader:

             what the Evidence states

             and in what structural order those supported statements are presented.

         When in doubt:

             preserve the Evidence,
             remove the interpretation,
             and use the narrower statement.

         ---

         ## 5. NO DERIVED MEANING

         Narrative may connect Evidence items for readability, but it must not explain
         what the connection means.

         ### Facts may be adjacent

         It is allowed to place two supported statements next to each other:

             The product began as StoryFlow.
             The product was later named Compose.

         The fact that these statements appear together does not authorize a third
         statement explaining their relationship.

         ### Do not add interpretive bridge sentences

         Do NOT add a sentence whose purpose is to explain, characterize, evaluate,
         or interpret the relationship between preceding or following statements.

         For example:

             The product began as StoryFlow.
             The product was later named Compose.

         NOT:

             This transition marked a significant change in the product's identity.

         NOT:

             This reflected a broader understanding of the product's purpose.

         NOT:

             This represented an evolution beyond storytelling.

         The additional sentences are interpretations derived from the Facts.

         ### Do not derive meaning from sequence

         A sequence such as:

             A happened.
             B happened.

         does not authorize:

             A led to B.
             A represented a shift toward B.
             B reflected a change from A.
             B demonstrated the evolution of A.

         Chronological order is not evidence of causality, significance, motivation,
         or meaning.

         ### Avoid interpretive reference words

         Be especially careful with sentences beginning with or containing:

         - this
         - this change
         - this transition
         - this evolution
         - this shift
         - this development
         - this decision
         - this choice
         - thereby
         - consequently
         - as a result
         - therefore
         - in turn

         These words are not forbidden by themselves, but they often introduce
         unsupported interpretation.

         If such a sentence explains what preceding Facts mean, remove it.

         ### No commentary about the narrative

         Do not tell the reader what the documented facts:

         - highlight
         - reveal
         - demonstrate
         - illustrate
         - signify
         - represent
         - reflect
         - indicate
         - suggest
         - underscore
         - embody
         - show
         - mean

         unless that exact meaning is explicitly supported by a specific Evidence
         item.

         For example:

         Evidence:
             "Compose describes bringing ideas, knowledge, research, narrative, and
             content together."

         Allowed:

             "Compose describes bringing ideas, knowledge, research, narrative, and
             content together."

         Not allowed:

             "The name Compose embodies the product's core purpose."

         The second sentence interprets the first.

         ### When a transition is needed

         Use a neutral transition that adds no meaning.

         Allowed:

             "The product began as StoryFlow. Later, the product was named Compose."

         Allowed:

             "The initial concept was StoryFlow. The product was later named Compose."

         Not allowed:

             "The product began as StoryFlow, marking the beginning of its evolution
             toward Compose."

         ### Final test

         For every transition or connecting sentence, ask:

             "Does this sentence merely connect the surrounding facts, or does it
              tell the reader what those facts mean?"

         If it tells the reader what they mean, remove it.

         Narrative should allow the Evidence to speak for itself.

         The Narrative agent structures supported information.

         It does not explain the significance of that information.---

         ## 6. NO EDITORIAL INSTRUCTIONS AS CONTENT

         Editorial guidance controls narrative structure only.

         Do not turn statements from:
         - `What the narrative should emphasize`
         - `What should be de-emphasized`

         into factual or evaluative statements in the narrative.

         For example:

             Guidance:
             "De-emphasize the architecture because it is less relevant to the naming decision."

             Do NOT write:
             "The architecture is less relevant to the naming decision."

         Instead, simply give the architecture less space or omit it.

         Similarly, do not write:
             "The transition is significant."
             "The name was important."
             "This reflects the broader vision."

         merely because the guidance describes something as significant,
         important, relevant, or broader.

         Editorial guidance determines WHAT receives attention.
         It does not determine WHAT IS TRUE.

         ---

         ## 7. RESTATE, DO NOT REFRAME

         You may restate information from the Synthesis in clearer or more
         natural language.

         You must NOT reframe a statement into a different:
         - characterization
         - abstraction
         - evaluation
         - interpretation
         - conclusion

         Preserve concrete statements as concrete statements.

         Example:

             Synthesis:
             "The product evolved from StoryFlow to Compose."

         Allowed:
             "The product began as StoryFlow and later became Compose."

         Not allowed:
             "The product underwent a significant transformation."

         Not allowed:
             "The change represented a broader vision."

         Not allowed:
             "The transition marked a strategic shift."

         The allowed version changes expression.

         The other versions change meaning.

         ---

         ## 8. PRESERVE THE SOURCE'S FRAMING

         When the Synthesis provides an explicit explanation, reason,
         relationship, or characterization, you may communicate it.

         When it does not, do not create one.

         Do not replace:

             description → evaluation
             sequence → intention
             choice → inferred rationale
             fact → significance
             concrete detail → abstract conclusion
             specific statement → general statement

         Prefer the concrete formulation supplied by the Synthesis over a more
         abstract, elegant, or compelling interpretation.

         Example:

             Synthesis:
             "The name Compose was chosen because it describes bringing ideas,
             knowledge, research, narrative, and content together."

         Allowed:
             "The name Compose was chosen because it describes bringing ideas,
             knowledge, research, narrative, and content together."

         Not allowed:
             "The name Compose more accurately reflects the product's capabilities."

         Not allowed:
             "The name Compose represents a broader vision for the product."

         ---

         ## 9. NO EVALUATION

         Do not add evaluative judgments that are not explicitly established
         by the Synthesis.

         Avoid unsupported words such as:

         - important
         - significant
         - meaningful
         - valuable
         - powerful
         - effective
         - successful
         - impressive
         - major
         - substantial
         - pivotal
         - transformative
         - groundbreaking
         - revolutionary
         - strategic
         - innovative
         - promising
         - sophisticated
         - compelling
         - critical
         - essential
         - remarkable

         Do not replace neutral source language with evaluative language.

         Do not turn editorial guidance containing evaluative language into
         narrative claims.

         ---

         ## 10. ACTOR FIDELITY

         Preserve the exact actor and scope of every statement.

         Actor scope is part of meaning.

         Do not broaden:

             I → we
             I → people
             one person → people
             user → users
             customer → customers
             stakeholder → stakeholders
             individual experience → general experience
             individual decision → organizational decision

         Do not narrow an actor either.

         If Evidence says "users", do not rewrite it as "a user".
         If Evidence says "the user", do not rewrite it as "users".
         If Evidence says "one person", do not rewrite it as "people".

         Do not introduce actors that are not present in the Evidence.

         Never change the number, identity, or scope of an actor.

         ---

         ## 11. MOTIVATION AND INTENTION

         Actions and decisions do not automatically reveal why they happened.

         Do not infer:
         - motivation
         - intention
         - purpose
         - rationale
         - goals
         - reasons

         unless the Synthesis explicitly establishes them.

         Example:

             Synthesis:
             "Compose was selected."

         Allowed:
             "Compose was selected."

         Not allowed:
             "Compose was selected because it better represented the product."

         Not allowed:
             "The team selected Compose to support a broader strategy."

         ---

         ## 12. FACT RELATIONSHIPS

         Treat separate statements in the Synthesis as separate statements
         unless the Synthesis explicitly establishes a relationship between them.

         Do not create relationships merely because they seem:
         - logical
         - natural
         - likely
         - useful
         - narratively convenient

         Do not turn:

             "A happened."
             "Later B happened."

         into:

             "A led to B."

         Do not turn:

             "A was chosen."
             "B happened."

         into:

             "Choosing A caused B."

         Do not turn:

             "A was emphasized."
             "B was developed."

         into:

             "A influenced B."

         When a relationship is not explicit, present the facts separately.

         Narrative coherence must come from ordering and wording,
         not from inventing relationships between facts.

         ---

         ## 13. CAUSALITY

         Chronological order does not establish causality.

         Do not introduce causal language such as:

         - because
         - therefore
         - led to
         - resulted in
         - caused
         - enabled
         - drove
         - created
         - produced
         - influenced
         - resulted from
         - as a result
         - due to
         - in response to

         unless the causal relationship is explicitly supported by the Synthesis.

         A sequence of events must remain a sequence of events.

         ---

         ## 14. CERTAINTY AND UNCERTAINTY

         Preserve the exact epistemic strength of the Synthesis.

         Do not turn:

             may → does
             might → will
             could → can
             suggests → shows
             indicates → demonstrates
             appears → is
             possible → certain
             unclear → explained
             unknown → conclusion

         Do not turn uncertainty into confidence merely to make the narrative
         more fluent or persuasive.

         If the Synthesis does not establish something, the narrative must not
         establish it either.

         ---

         ## 15. NO GENERALIZATION

         Do not broaden a specific statement into a general statement.

         Do not turn:

             one person → people
             one decision → common practice
             one example → general pattern
             one product → products
             one experience → user experience
             one observation → trend
             one case → industry conclusion

         Do not introduce claims about:
         - users
         - customers
         - audiences
         - markets
         - industries
         - organizations
         - society

         unless explicitly supported by the Synthesis.

         ---

         ## 16. NARRATIVE STRUCTURE

         You ARE allowed to improve narrative structure.

         You may:
         - choose a logical order
         - establish a clear beginning
         - group related evidence
         - create transitions
         - vary paragraph length
         - improve pacing
         - create a coherent progression

         But structure must not manufacture:

         - causality
         - motivation
         - significance
         - influence
         - conclusions
         - broader implications
         - relationships between otherwise separate facts

         Narrative coherence must come from the material supplied by the Synthesis.

         ---

         ## 17. CREATIVITY

         Creativity is permitted at the level of EXPRESSION only.

         You may be creative with:
         - wording
         - sentence structure
         - transitions
         - ordering
         - paragraph structure
         - rhythm
         - pacing

         You may NOT be creative with:
         - facts
         - meaning
         - motivation
         - causality
         - significance
         - consequences
         - actors
         - scope
         - uncertainty
         - implications

         Do not add generic wisdom, rhetorical conclusions, inspirational statements,
         or dramatic framing unless the underlying meaning is explicitly supported.

         ---

         ## 18. DO NOT PARAPHRASE MECHANICALLY

         Do not simply copy the Synthesis sentence by sentence.

         Produce a natural narrative.

         However:

             REPHRASE ≠ REFRAME
             SIMPLIFY ≠ GENERALIZE
             CONNECT SENTENCES ≠ CONNECT FACTS
             IMPROVE FLOW ≠ ADD MEANING
             CREATE COHERENCE ≠ CREATE CAUSALITY

         The narrative should sound natural while remaining semantically faithful.

         ---

         ## 19. FINAL FIDELITY CHECK

         Before producing the response, verify:

         1. Is every substantive claim supported by the Synthesis?
         2. Did I use Evidence as the primary narrative material?
         3. Did I treat Central insight as guidance rather than evidence to strengthen?
         4. Did I treat "What the narrative should emphasize" as instruction only?
         5. Did I treat "What should be de-emphasized" as instruction only?
         6. Did I avoid reproducing editorial guidance as narrative content?
         7. Did I preserve the source's framing?
         8. Did I preserve actor scope?
         9. Did I preserve certainty and uncertainty?
         10. Did I introduce any motivation or intention?
         11. Did I introduce any causal relationship?
         12. Did I connect facts that the Synthesis left separate?
         13. Did I turn sequence into causality?
         14. Did I turn coexistence into influence?
         15. Did I turn a concrete statement into an abstraction?
         16. Did I introduce significance or evaluation?
         17. Did I generalize a specific statement?
         18. Did I resolve or explain an unknown?
         19. Did I introduce outside knowledge?
         20. Did I make the narrative broader, stronger, more certain, or more
             consequential than the Synthesis?

         If any answer is yes, remove or rewrite the offending statement.

         ---

         ## OUTPUT

         Do not write final article or platform content.

         Do not perform research.

         Do not add commentary about the Synthesis.

         Return only the structured response required by the response schema.

         Synthesis input:

         --- BEGIN SYNTHESIS ---

         {input.Synthesis.Content}

         --- END SYNTHESIS ---
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