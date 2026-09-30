using System.Diagnostics;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Compose;

/// <summary>
///     An agent that transforms a narrative into finished content for a specified content format.
/// </summary>
public sealed class ComposeAgent(
    AIAgent agent,
    ILogger<ComposeAgent> logger)
    : IAgent<ComposeInput, ComposeResult>
{
    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));
    private readonly ILogger<ComposeAgent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<ComposeResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        ComposeInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        const string agentName = "ComposeAgent";
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
            var composeResult = ParseComposeResultFromJson(responseText);

            stopwatch.Stop();

            _logger.LogInformation(
                "Agent {AgentName} invocation completed in {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            return composeResult;
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

    private static string CreateUserMessage(ComposeInput input)
    {
        var formatInstruction = input.Format switch
        {
            ContentFormat.Article =>
                """
                For Article, produce a complete article with:
                - a title
                - an introduction
                - a coherent body structure
                - natural transitions
                - a conclusion where appropriate

                All of these elements must be derived from the supplied Narrative.
                Do not introduce new subject matter merely to make the article feel
                more complete.
                """,

            _ => throw new InvalidOperationException(
                $"Unsupported content format: {input.Format}")
        };

        return $"""
                Transform the supplied Narrative into finished content for the requested format.

                {formatInstruction}

                ---

                ## ROLE OF COMPOSE

                Compose is the final content-expression layer.

                The Narrative is the PRIMARY SOURCE and the semantic boundary of the content.

                Your job is to turn the Narrative into polished, readable content for the
                requested format.

                You may improve:
                - structure
                - wording
                - paragraph organization
                - transitions
                - readability
                - pacing
                - formatting
                - presentation appropriate to the requested format

                You may NOT change the meaning of the Narrative.

                The core rule is:

                    COMPOSE MAY TRANSFORM EXPRESSION,
                    BUT MUST NOT TRANSFORM MEANING.

                ---

                ## SOURCE FIDELITY

                Every substantive claim in the finished content must be supported by the
                Narrative.

                Preserve:
                - facts
                - evidence
                - actors
                - relationships
                - scope
                - uncertainty
                - stated limitations
                - level of certainty
                - intended meaning

                Do not introduce:
                - new facts
                - new events
                - new actors
                - motivations
                - intentions
                - explanations
                - causal relationships
                - consequences not established by the Narrative
                - user or customer reactions
                - audience reactions
                - market or business implications
                - strategic implications
                - broader significance
                - general lessons
                - predictions
                - future expectations
                - outside knowledge

                Do not use general knowledge to expand or contextualize the Narrative.

                ---

                ## NO SEMANTIC AMPLIFICATION

                Do not make the Narrative stronger, broader, more certain, or more
                consequential than it is.

                In particular, do not transform:

                    "X happened"
                into:
                    "X was significant"

                or:

                    "X changed"
                into:
                    "X was a major transformation"

                or:

                    "X was chosen"
                into:
                    "X was the strategic choice"

                unless the Narrative explicitly establishes those meanings.

                Avoid unsupported evaluative or rhetorical language such as:

                - revolutionary
                - groundbreaking
                - transformative
                - profound
                - pivotal
                - significant
                - powerful
                - promising
                - visionary
                - strategic
                - sophisticated
                - innovative
                - remarkable
                - exciting
                - game-changing

                Do not use such language merely to make the content more engaging.

                Engagement must come from clear writing and effective structure, not from
                adding unsupported significance.

                ---

                ## ACTOR AND SCOPE FIDELITY

                Preserve the scope of statements exactly.

                Do not broaden:

                    one person → people
                    user → users
                    customer → customers
                    individual experience → general experience
                    documented observation → general trend

                Do not introduce an audience, market, customer group, stakeholder group,
                or other actor unless the Narrative explicitly contains it.

                ---

                ## MOTIVATION AND CAUSALITY

                Do not invent explanations for why something happened.

                Do not infer:
                - motivation
                - intention
                - purpose
                - cause
                - consequence

                unless explicitly established by the Narrative.

                Chronological order does not establish causality.

                Do not turn:

                    "X happened, followed by Y"

                into:

                    "X led to Y"

                unless the Narrative establishes that relationship.

                ---

                ## UNCERTAINTY

                Preserve all meaningful uncertainty from the Narrative.

                Do not silently convert:

                    may → does
                    might → will
                    could → can
                    suggests → shows
                    indicates → proves
                    appears → is
                    unknown → explained

                If the Narrative identifies something as unknown, do not resolve it.

                Do not fill gaps with plausible assumptions.

                ---

                ## TITLE AND INTRODUCTION
                
                A title and introduction are OPTIONAL.
                
                If included, both must be derived directly from the Narrative.
                
                They must not introduce a new interpretation, relationship, significance,
                motivation, context, or conclusion.
                
                ### Title rule
                
                A title must be traceable to ONE specific Narrative statement.
                
                A title may:
                - reproduce a short phrase from one Narrative statement
                - use a shortened version of one Narrative statement
                - use a direct factual description of one Narrative statement
                
                A title must NOT:
                - combine multiple Narrative statements
                - describe a relationship between Narrative statements
                - describe an evolution, journey, transition, development, or progression
                  unless that complete relationship is explicitly present in ONE Narrative
                  statement
                - characterize the subject
                - describe significance
                - describe importance
                - describe motivation
                - describe implications
                - introduce a broader theme
                - introduce information not present in the Narrative
                
                Example Narrative:
                
                    "The final name selected for the product became Nuëyon.Compose."
                    "The initial product concept was called StoryFlow."
                
                Allowed:
                
                    "Nuëyon.Compose"
                
                    "The Final Product Name"
                
                    "The Initial Product Concept"
                
                NOT allowed:
                
                    "The Evolution of Product Naming"
                
                    "From StoryFlow to Nuëyon.Compose"
                
                    "The Journey from StoryFlow to Nuëyon.Compose"
                
                The prohibited titles combine separate Narrative statements and create a
                relationship between them.
                
                ### Introduction rule
                
                An introduction must contain only statements directly supported by the
                Narrative.
                
                Every substantive sentence in the introduction must be traceable to ONE
                specific Narrative statement.
                
                The introduction may:
                - state the subject directly
                - introduce one directly supported fact
                - briefly establish the subject using one Narrative statement
                - repeat a Narrative statement for structural purposes
                
                The introduction must NOT:
                - explain why the subject matters
                - explain why the subject is important
                - describe a journey
                - describe an evolution
                - describe a transition
                - explain relationships between Narrative statements
                - provide background not present in the Narrative
                - introduce a broader context
                - create a thesis
                - preview an interpretation
                - create a conclusion
                - address the reader
                - use generic statements about the subject
                
                Do NOT write statements such as:
                
                    "Naming a product is a crucial step in its development."
                
                    "The journey from StoryFlow to Compose reflects an important shift."
                
                    "Choosing a product name can shape how a product is understood."
                
                    "The story behind the name reveals how the product evolved."
                
                These statements are not derived from a specific Narrative statement.
                
                ### No relationship creation
                
                Do not use the introduction to connect separate Narrative statements.
                
                Narrative:
                
                    "The final name selected for the product became Nuëyon.Compose."
                
                    "The initial product concept was called StoryFlow."
                
                Allowed:
                
                    "The final name selected for the product became Nuëyon.Compose."
                
                    "The initial product concept was called StoryFlow."
                
                NOT allowed:
                
                    "The product evolved from StoryFlow to Nuëyon.Compose."
                
                NOT allowed:
                
                    "The product began as StoryFlow before becoming Nuëyon.Compose."
                
                NOT allowed:
                
                    "The transition from StoryFlow to Nuëyon.Compose marked the product's
                     development."
                
                The prohibited versions create relationships that are not contained in either
                individual Narrative statement.
                
                ### No framing statements
                
                Do not add introductory sentences merely to make the article sound more
                engaging.
                
                Avoid:
                
                - "This story begins with..."
                - "At the heart of this..."
                - "The journey starts with..."
                - "One of the key moments..."
                - "An important part of this..."
                - "The story behind..."
                - "This reflects..."
                - "This illustrates..."
                - "This demonstrates..."
                - "This highlights..."
                - "This shows why..."
                
                Unless the complete meaning is explicitly present in ONE Narrative statement.
                
                ### Introduction may be minimal
                
                A complete article does NOT require a substantive introduction.
                
                If no introduction can be written without adding meaning:
                
                    OMIT THE INTRODUCTION.
                
                Starting directly with a supported Narrative statement is preferred over
                adding generic or interpretive framing.
                
                ### Validation
                
                Before returning a title or introduction:
                
                1. Identify the ONE Narrative statement supporting it.
                2. Verify that the meaning is preserved.
                3. Verify that no second Narrative statement is required.
                4. Verify that no relationship between Narrative statements was created.
                5. Verify that no interpretation was added.
                6. Verify that no significance was added.
                7. Verify that no motivation or intention was added.
                8. Verify that no outside context was added.
                9. Verify that no generic framing was added.
                
                If a title requires multiple Narrative statements:
                
                    REWRITE THE TITLE.
                
                If an introduction sentence requires multiple Narrative statements:
                
                    SPLIT THE SENTENCE OR OMIT IT.
                
                If a title or introduction adds meaning:
                
                    REMOVE THE ADDED MEANING.
                
                ### Core rule
                
                The title and introduction are part of the final expression layer.
                
                They may improve presentation.
                
                They must NOT create meaning.
                
                Every substantive title or introduction statement must therefore be traceable
                to ONE specific Narrative statement.
                
                If it cannot be traced:
                
                    DO NOT WRITE IT.

                ---

                ## CONCLUSION
                
                A conclusion is OPTIONAL.
                
                If a conclusion is included, it must be a direct restatement of information
                already present in the Narrative.
                
                The conclusion must NOT derive a new meaning from the Narrative.
                
                Do NOT use the conclusion to:
                
                - interpret the Narrative
                - explain what the Narrative means
                - explain why the Narrative matters
                - characterize the development
                - evaluate a decision
                - describe significance
                - infer a broader lesson
                - infer strategic meaning
                - describe consequences not explicitly stated
                - introduce future possibilities
                - make predictions
                - recommend an action
                - address the reader
                - generalize from the specific case
                
                Do NOT transform:
                
                    "The initial product concept was called StoryFlow."
                    "The final product name selected was Compose."
                
                into:
                
                    "The transition from StoryFlow to Compose illustrates the importance of
                     naming in product identity."
                
                The second statement derives a new conclusion from multiple Narrative
                statements.
                
                If the Narrative does not contain a suitable concluding statement, DO NOT
                write a conclusion.
                
                The article may simply end after the final supported point.
                
                ### Conclusion rule
                
                A conclusion may only:
                
                1. repeat a directly supported statement from the Narrative, or
                2. restate a single Narrative statement using surface-level wording changes.
                
                It must NOT combine multiple Narrative statements into a new conclusion.
                
                When in doubt:
                
                    OMIT THE CONCLUSION.

                ---

                ## WRITING QUALITY
                
                The finished content should be clear, readable, and appropriate for the
                requested format.
                
                Writing quality comes from improving the EXPRESSION of the Narrative.
                
                It must never come from creating new relationships, interpretations, or
                meaning between Narrative statements.
                
                ### Narrative is a sequence of independent source statements
                
                Treat each Narrative sentence or statement as an independent source unit.
                
                Compose may:
                
                - rewrite one Narrative statement
                - split one Narrative statement into multiple sentences
                - remove unnecessary repetition within one Narrative statement
                - organize Narrative statements into paragraphs
                - reorder complete Narrative statements when appropriate for the format
                
                Compose must NOT:
                
                - merge separate Narrative statements into one new meaning
                - connect separate Narrative statements causally
                - connect separate Narrative statements chronologically
                - connect separate Narrative statements logically
                - explain the relationship between separate Narrative statements
                - infer that one Narrative statement caused another
                - infer that one Narrative statement explains another
                - infer that one Narrative statement led to another
                - infer that one Narrative statement followed from another
                - infer that one Narrative statement represents a change from another
                
                ### One-source-unit rule
                
                Every substantive sentence must be derived from ONE Narrative source unit.
                
                A source unit is one individual Narrative sentence or statement.
                
                The sentence may be rewritten for readability, but the meaning of that source
                unit must remain unchanged.
                
                Example:
                
                Narrative:
                
                    "The initial product concept was called StoryFlow."
                
                    "The user explicitly asked for alternatives that did not necessarily
                     contain the word 'story'."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                Allowed:
                
                    "The initial product concept was called StoryFlow."
                
                    "The user explicitly asked for alternatives that did not necessarily
                     contain the word 'story'."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                Also allowed:
                
                    "The initial product concept was called StoryFlow."
                
                    "Alternatives that did not necessarily contain the word 'story' were
                     requested."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow, but the user later
                     asked for alternatives."
                
                This combines two Narrative statements.
                
                NOT allowed:
                
                    "The user asked for alternatives, leading to the preference for Compose."
                
                This creates a causal relationship between two Narrative statements.
                
                NOT allowed:
                
                    "After StoryFlow, the user moved toward Compose."
                
                This creates a chronological relationship that is not contained in either
                individual Narrative statement.
                
                ### Connective words are semantic operations
                
                Do not assume that a transition is merely stylistic.
                
                Words and phrases that connect separate statements can introduce meaning.
                
                When a connective word implies a relationship between two Narrative
                statements, it is prohibited unless that complete relationship exists within
                ONE Narrative source unit.
                
                Avoid using connective language such as:
                
                - because
                - therefore
                - consequently
                - as a result
                - leading to
                - resulted in
                - due to
                - in response
                - because of this
                - for this reason
                - which led to
                - which resulted in
                - after
                - before
                - later
                - eventually
                - subsequently
                - meanwhile
                - however
                - therefore
                - thus
                - hence
                - this change
                - this shift
                - this transition
                - this development
                - this decision
                - this opened the door
                - this resulted in
                - from this
                - following this
                - building on this
                - in turn
                
                These words are not forbidden when they occur entirely inside a single
                Narrative source unit and faithfully preserve its meaning.
                
                But Compose must not introduce them to connect two separate Narrative
                statements.
                
                ### No causal connections
                
                Do not create causality between Narrative statements.
                
                Narrative:
                
                    "The user explicitly asked for alternatives that did not necessarily
                     contain the word 'story'."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                Allowed:
                
                    "The user explicitly asked for alternatives that did not necessarily
                     contain the word 'story'."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                NOT allowed:
                
                    "The user asked for alternatives, which led to the preference for
                     Compose."
                
                NOT allowed:
                
                    "In response to the request for alternatives, the user chose Compose."
                
                The second versions create relationships between separate source units.
                
                ### No chronological connections
                
                Do not create a timeline by adding relationships that are not present in a
                single Narrative source unit.
                
                Narrative:
                
                    "The initial product concept was called StoryFlow."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                Allowed:
                
                    "The initial product concept was called StoryFlow."
                
                    "The user stated, 'I like Compose,' which became the direction for the
                     product name."
                
                NOT allowed:
                
                    "The product began as StoryFlow and later moved to Compose."
                
                NOT allowed:
                
                    "After StoryFlow, the product moved toward Compose."
                
                The Narrative may contain statements that happen to describe different
                points in time. That does not authorize Compose to construct a timeline
                between them.
                
                ### No logical connections
                
                Do not connect statements with words that imply explanation, contrast,
                consequence, or interpretation.
                
                Narrative:
                
                    "The initial product concept was called StoryFlow."
                
                    "The final product name selected was Compose."
                
                Allowed:
                
                    "The initial product concept was called StoryFlow. The final product
                     name selected was Compose."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow, but the final product
                     name selected was Compose."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow, while Compose was
                     eventually selected."
                
                NOT allowed:
                
                    "The product moved from StoryFlow to Compose."
                
                The prohibited versions add relationships that are not present in the
                individual source units.
                
                ### No semantic compression
                
                Do not compress multiple Narrative statements into a higher-level statement.
                
                Narrative:
                
                    "The product should focus on transforming ideas and knowledge into
                     content."
                
                    "The central product opportunity discussed was to create a system that
                     helps people turn their accumulated knowledge and ideas into
                     publishable content."
                
                Allowed:
                
                    "The product should focus on transforming ideas and knowledge into
                     content."
                
                    "The central product opportunity discussed was to create a system that
                     helps people turn their accumulated knowledge and ideas into
                     publishable content."
                
                NOT allowed:
                
                    "The product's vision was to transform accumulated knowledge into useful
                     content."
                
                The last version synthesizes multiple source statements into a new
                statement.
                
                ### No interpretive transitions
                
                A transition must only improve formatting.
                
                Allowed:
                
                    "The initial product concept was called StoryFlow.
                
                    The user explicitly asked for alternatives that did not necessarily
                    contain the word 'story'."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow. However, the user
                    later wanted alternatives."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow. This request marked a
                    shift in direction."
                
                NOT allowed:
                
                    "The initial product concept was called StoryFlow. This opened the door
                    to Compose."
                
                The prohibited transitions interpret the relationship between the statements.
                
                ### No framing or commentary
                
                Do not add commentary about the Narrative.
                
                Do NOT write:
                
                - "This is an important part of..."
                - "This reflects..."
                - "This demonstrates..."
                - "This highlights..."
                - "This illustrates..."
                - "This shows..."
                - "This marks..."
                - "This represents..."
                - "This reveals..."
                - "This suggests..."
                - "This is significant because..."
                - "This was a crucial step..."
                - "This was a major development..."
                - "This became an important turning point..."
                
                Unless the complete statement already exists within ONE Narrative source unit.
                
                ### No generic knowledge
                
                Do not add statements that are generally true but are not present in the
                Narrative.
                
                For example:
                
                    "Naming a product is a crucial step in its development."
                
                    "A strong product name can influence how users perceive a product."
                
                    "AI-native systems are changing how content is created."
                
                These may be reasonable statements, but they are not Narrative content.
                
                Do not add them.
                
                ### No inferred significance
                
                Do not describe Narrative content as:
                
                - important
                - significant
                - major
                - critical
                - strategic
                - transformative
                - innovative
                - valuable
                - compelling
                - promising
                - sophisticated
                - effective
                - successful
                - meaningful
                - fundamental
                - crucial
                
                unless that exact characterization is contained in ONE Narrative source unit.
                
                ### No inferred motivation
                
                Do not explain why a person, team, or organization acted unless the complete
                reason is contained in ONE Narrative source unit.
                
                Do not turn:
                
                    "The user stated, 'I like Compose.'"
                
                into:
                
                    "The user preferred Compose because it better represented the product."
                
                The second statement adds motivation.
                
                ### No inferred audience or market meaning
                
                Do not turn a statement about the user into a statement about:
                
                - users
                - customers
                - readers
                - audiences
                - markets
                - companies
                - industry
                - public reaction
                
                unless explicitly present in ONE Narrative source unit.
                
                ### Structural organization is allowed
                
                Compose may use:
                
                - paragraphs
                - whitespace
                - headings
                - lists
                - sentence breaks
                - formatting appropriate to the requested content type
                
                These are presentation changes only.
                
                They must not create new semantic relationships.
                
                When in doubt, use separate sentences and paragraphs.
                
                ### Article guidance
                
                For an Article:
                
                - Start directly with a Narrative-supported statement.
                - Use a factual title derived from ONE Narrative source unit.
                - Use only Narrative-supported sentences.
                - Use paragraphs to group material structurally.
                - Do not create a narrative arc.
                - Do not create a journey.
                - Do not create a transformation story.
                - Do not create a lesson.
                - Do not create a takeaway.
                - Do not create a broader theme.
                - Do not create a conclusion that synthesizes multiple source units.
                - Do not make the article more significant than the Narrative.
                
                The article does not need to tell a stronger story than the Narrative contains.
                
                ### Sentence-level validation
                
                Before returning the finished content, validate every substantive sentence.
                
                For each sentence:
                
                1. Identify the ONE Narrative source unit supporting it.
                2. Verify that the sentence preserves that source unit's meaning.
                3. Verify that no second source unit is required.
                4. Verify that no relationship between source units was created.
                5. Verify that no causal relationship was added.
                6. Verify that no chronological relationship was added.
                7. Verify that no logical relationship was added.
                8. Verify that no interpretation was added.
                9. Verify that no significance was added.
                10. Verify that no motivation was added.
                11. Verify that no outside knowledge was added.
                12. Verify that no rhetorical framing was added.
                13. Verify that no connective word introduces unsupported meaning.
                
                If a sentence requires two source units:
                
                    SPLIT IT.
                
                If splitting does not preserve the intended meaning:
                
                    REMOVE IT.
                
                If a transition introduces a relationship:
                
                    REMOVE THE TRANSITION.
                
                If a sentence only makes the article sound more interesting:
                
                    REMOVE THE SENTENCE.
                
                ### Output preference
                
                Prefer:
                
                    separate factual statements
                
                over:
                
                    synthesized statements.
                
                Prefer:
                
                    direct expression
                
                over:
                
                    interpretation.
                
                Prefer:
                
                    structural separation
                
                over:
                
                    semantic connection.
                
                Prefer:
                
                    simple prose
                
                over:
                
                    rhetorical prose.
                
                Prefer:
                
                    omission
                
                over:
                
                    invented meaning.
                
                ### Core rule
                
                Compose may transform the EXPRESSION of a Narrative statement.
                
                Compose may NOT transform the RELATIONSHIP between Narrative statements.
                
                Each Narrative source unit is authoritative.
                
                One source unit in.
                
                One supported statement out.
                
                If a sentence cannot be traced to exactly ONE Narrative source unit:
                
                    DO NOT WRITE IT.

                ---

                ## FINAL CHECK

                Before returning the content, verify every substantive statement:

                1. Is it supported by the Narrative?
                2. Did I preserve its original meaning?
                3. Did I preserve the original scope?
                4. Did I preserve the original certainty?
                5. Did I introduce a motivation or intention?
                6. Did I introduce a causal relationship?
                7. Did I introduce significance or evaluation?
                8. Did I introduce user, customer, audience, market, business, or strategic meaning?
                9. Did I introduce a future expectation or prediction?
                10. Did I resolve an uncertainty or fill a gap?
                11. Did I use general knowledge to expand the source?
                12. Did I add rhetorical language that implies more than the Narrative establishes?

                If any statement fails this check, remove it or rewrite it so that it remains
                faithful to the Narrative.

                When in doubt, prefer narrower wording over stronger wording.

                ---

                Return only the structured response defined by the output schema.

                Narrative input:

                --- BEGIN NARRATIVE ---

                {input.Narrative.Content}

                --- END NARRATIVE ---
                """;
    }

    private static ChatClientAgentRunOptions CreateAgentRunOptions()
    {
        var responseFormat = ChatResponseFormat.ForJsonSchema<ComposeResult>(
            null,
            nameof(ComposeResult));

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

    private static ComposeResult ParseComposeResultFromJson(string json)
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

            var response = JsonSerializer.Deserialize<ComposeResult>(
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
                    "Compose response content is empty.");
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