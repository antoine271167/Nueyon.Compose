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

    /// <summary>
    ///     Gets the system instructions used to configure this agent's underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() => ComposeAgentInstructions.GetSystemInstructions();

    private static string CreateUserMessage(ComposeInput input)
    {
        var format = input.Composition.Format;

        return format switch
        {
            ContentFormat.Article => CreateArticleUserMessage(input),
            ContentFormat.LinkedInPost => CreateLinkedInPostUserMessage(input),
            _ => throw new InvalidOperationException(
                $"Unsupported content format: {format}")
        };
    }

    private static string CreateArticleUserMessage(ComposeInput input)
    {
        var format = input.Composition.Format;

        return $"""
                Transform the supplied Narrative into finished content for the requested CompositionSpec format: {format}.

                The CompositionSpec controls **HOW** the content is structured and presented.
                The Narrative controls **WHAT** the content says.

                ## ROLE

                Compose is a **writing and transformation layer**.

                The Narrative has already established the meaning that may appear in the final content.

                Your job is to turn that meaning into **clear, natural, engaging, polished content**.

                The goal is:

                **Genuinely good writing without adding meaning.**

                Do not perform new research.
                Do not analyze the subject.
                Do not add a new interpretation.
                Do not invent a stronger story than the Narrative supports.

                ---

                ## SEMANTIC FIREWALL

                Treat the Narrative as the complete and final semantic source.

                You may change the wording and structure.
                You may NOT derive additional meaning from the Narrative.

                In particular, do not infer:
                - why something happened
                - what something means
                - why something is important
                - what something represents
                - what something indicates
                - what a decision reflects
                - what a change signifies
                - what the broader implication is

                unless that exact meaning is explicitly stated in the Narrative.

                A fact being related to another fact does not mean that one caused the other.

                A change being described does not mean that it was significant.

                A decision being described does not mean that it had a strategic purpose.

                An evolution being described does not mean that it represents a broader shift.

                If the Narrative does not explicitly provide an interpretation, do not create one.

                ---

                ## YOU MAY IMPROVE THE WRITING

                You may:

                - paraphrase
                - combine related statements
                - split statements
                - reorder statements
                - group related information into paragraphs
                - create headings
                - create natural transitions
                - improve sentence structure
                - remove repetition
                - improve pacing and readability
                - create a factual title
                - create an introduction
                - create a conclusion

                The final result should read like a naturally written article, not like a list of Narrative statements.

                You do **not** have to preserve the Narrative's sentence boundaries.

                ---

                ## DO NOT ADD MEANING

                Every factual claim and relationship in the final content must be supported by the Narrative.

                Do not add:

                - facts
                - events
                - motivations
                - intentions
                - causes
                - consequences
                - significance
                - evaluations
                - strategic implications
                - audience or market reactions
                - predictions
                - outside knowledge

                A sentence can be rewritten or combined with other sentences, but its meaning must remain supported by the Narrative.

                When in doubt, use the simpler factual formulation.

                ---

                ## DO NOT INFER CAUSALITY

                The order in which statements appear in the Narrative does **not** mean that one statement caused another.

                Only express causality when the Narrative explicitly establishes it.

                For example:

                Narrative:
                - "The user became less certain about using 'Story' in the product name."
                - "The user asked for alternatives that did not contain 'story'."

                Allowed:

                > "The user became less certain about using 'Story' in the product name and asked for alternatives that did not contain 'story'."

                Not allowed:

                > "Because the user became less certain about 'Story', they asked for alternative names."

                The second version turns sequence into causality.

                ---

                ## DO NOT INFER MOTIVATION OR INTENTION

                Only state a motivation or intention when the Narrative explicitly establishes it.

                Narrative:
                > "The user stated, 'I like Compose.'"

                Allowed:

                > "The user eventually stated, 'I like Compose.'"

                Not allowed:

                > "The user chose Compose because it better represented the product."

                The Narrative does not establish that reason.

                ---

                ## DO NOT ADD SIGNIFICANCE

                Do not make an event sound more important, strategic, meaningful, or consequential than the Narrative establishes.

                Avoid adding language such as:

                - pivotal
                - important
                - significant
                - strategic
                - transformative
                - crucial
                - compelling
                - represents a shift
                - marks a turning point
                - reflects a broader vision

                unless that meaning is explicitly established by the Narrative.

                For example:

                Narrative:
                > "The product naming converged to Compose."

                Allowed:

                > "The product naming eventually converged to Compose."

                Not allowed:

                > "This marked a significant strategic shift for the product."

                ---

                ## DO NOT ADD A NEW CONCLUSION

                A conclusion is optional.

                Only write a conclusion if the Narrative itself contains a supported concluding statement.

                Do **not** add a conclusion merely because an article normally has one.

                Do not finish the article with:

                - a lesson
                - a takeaway
                - a broader theme
                - a strategic implication
                - an assessment of what the events mean
                - a statement about what the future holds

                If the Narrative does not establish such a conclusion, simply end the article naturally with the final relevant information.

                For example:

                Narrative:
                > "The broader company/brand concept became Nuëyon, with the product represented as Nuëyon.Compose."

                Good ending:

                > "The broader company and brand concept became Nuëyon, with the product represented as Nuëyon.Compose."

                Bad ending:

                > "The move to Nuëyon.Compose ultimately created a stronger foundation for the product's future."

                The latter introduces an unsupported consequence.

                ---

                ## PRESERVE UNCERTAINTY

                If the Narrative says that something is unknown, uncertain, or not established, preserve that uncertainty.

                Do not turn uncertainty into a likely explanation.

                Narrative:

                > "The explicit reasons for the change are not detailed in the source."

                Allowed:

                > "The explicit reasons for the change are not detailed in the source."

                Not allowed:

                > "The change appears to have been driven by the desire for a broader product identity."

                The second statement invents an explanation.

                ---

                ## COMBINING STATEMENTS

                Combining statements is encouraged when it produces better writing and does not introduce new meaning.

                Narrative:
                - "The initial product concept was called StoryFlow."
                - "The underlying product idea was an AI-native system for turning ideas and knowledge into stories and distributing them as content."

                Good:

                > "The initial product concept, StoryFlow, was an AI-native system for turning ideas and knowledge into stories and distributing them as content."

                This is a legitimate combination because both parts of the sentence are directly supported.

                However, do not use a combination to introduce a relationship that the Narrative does not establish.

                ---

                ## ARTICLE QUALITY

                Write a genuinely good article.

                Prefer:

                - natural prose
                - clear paragraphs
                - varied sentence structure
                - good rhythm
                - logical progression
                - precise wording
                - appropriate emphasis
                - concise expression
                - natural transitions

                Avoid:

                - generic filler
                - generic introductions
                - repetitive statements
                - unnecessary explanation
                - artificial drama
                - exaggerated language
                - editorial commentary
                - statements about what the reader should think
                - statements about what the article itself is doing

                Do not add a sentence simply because it sounds like something a professional article would normally contain.

                Every sentence should either:

                1. communicate meaning supported by the Narrative, or
                2. improve the structure or expression of that meaning.

                ---

                ## TITLE

                A title is optional.

                If you create one, it must be directly supported by the Narrative.

                Prefer a factual, interesting title.

                Do not make the title claim a transformation, strategic shift, motivation, consequence, or significance that the Narrative does not establish.

                ---

                ## INTRODUCTION

                An introduction is optional.

                If you create one, start with the actual subject contained in the Narrative.

                Do not begin with generic statements such as:

                > "The journey of product naming often reveals..."

                or:

                > "In today's rapidly changing world..."

                The introduction must contain Narrative-supported information.

                ---

                ## FINAL CHECK

                Before producing the final content, check:

                1. Is every factual claim supported by the Narrative?
                2. Is every relationship supported by the Narrative?
                3. Did I accidentally turn sequence into causality?
                4. Did I invent a motivation or intention?
                5. Did I add significance or strategic meaning?
                6. Did I strengthen an uncertain statement?
                7. Did I add a conclusion that the Narrative does not support?
                8. Did I use outside knowledge?
                9. Does the article read naturally and professionally?
                10. Did I improve the writing without changing the meaning?

                If a more interesting formulation requires an inference, **do not make the inference**.

                Prefer slightly simpler writing over unsupported meaning.

                **Your highest priority is factual integrity. Your second priority is genuinely good writing.**

                ---

                ## INPUT

                Narrative input:

                --- BEGIN NARRATIVE ---
                {input.Narrative.Content}
                --- END NARRATIVE ---
                """;
    }

    private static string CreateLinkedInPostUserMessage(ComposeInput input)
    {
        var format = input.Composition.Format;

        return $"""
                Transform the supplied Narrative into finished content for the requested CompositionSpec format: {format}.

                The CompositionSpec controls **HOW** the content is structured and presented.
                The Narrative controls **WHAT** the content says.

                Compose must not add meaning. Every factual claim and relationship in the final content must be supported by the Narrative. The same semantic-fidelity rules apply to every content format.

                ## LINKEDIN POST

                Write a compelling LinkedIn post from the supplied Narrative.

                The goal is:

                **ATTENTION + READABILITY + ENGAGEMENT + FACTUAL INTEGRITY**

                The result must read like a deliberately written LinkedIn post, not a summary of a discussion, a project history, or a collection of extracted statements.

                ### 1. Find the central message

                The Narrative may contain multiple topics, facts, decisions, observations, and technical details. The post must not attempt to cover them all.

                Identify the single most interesting and best-supported message that can be communicated through a LinkedIn post.

                Distinguish between:

                - **Central message:** the main point the reader should take away.
                - **Supporting material:** facts and observations that help communicate that point.
                - **Peripheral material:** relevant information that does not strengthen the selected message.

                Include the central message and only the supporting material needed to communicate it clearly. Omit peripheral material.

                Select one primary topic. Do not combine separate topics simply because they appear in the same Narrative.

                For example, if the Narrative discusses a product's naming history, agent architecture, orchestration principles, and implementation priorities, do not automatically include all four. Choose the topic that offers the strongest supported message and focus on that.

                ### 2. Build the post around that message

                Do not reproduce the Narrative in its original order.

                Create a coherent progression that serves the selected message:

                1. **Opening:** Start with the strongest supported statement, contrast, observation, or experience.
                2. **Development:** Use relevant supporting facts to make the central message clear.
                3. **Ending:** Complete the thought naturally without introducing another topic.

                These are editorial roles, not mandatory paragraph counts.

                The opening does not have to be the first statement in the Narrative. The ending does not have to use its final statement.

                When the Narrative supports a personal experience, consider using it as the thread connecting the post. When it does not, use a direct observation or supported statement instead.

                Do not invent a personal journey, turning point, emotional reaction, or causal connection to make the post more dramatic.

                ### 3. Be selective

                A LinkedIn post is not a complete record of everything discussed.

                Apply these rules:

                - Normally include no more than two or three supporting points.
                - Include additional points only when they are essential to understanding the central message.
                - Combine statements that communicate substantially the same idea.
                - Remove repeated claims, redundant explanations, and unnecessary background.
                - Omit technical concepts that do not directly strengthen the selected message.
                - Do not include lists of alternatives, implementation details, future features, or project decisions merely because they appear in the Narrative.
                - Do not explain every related concept or demonstrate everything the product can do.
                - Do not append leftover facts after the main message has already been communicated.

                Prefer one clear idea developed with a few relevant details over several ideas briefly summarized.

                **Once the central message is clear, stop.**

                ### 4. Write a strong opening

                The opening should give the reader a reason to continue.

                Prefer a direct, specific statement over a generic introduction.

                A suitable opening may:

                - State an interesting fact supported by the Narrative.
                - Express a contrast explicitly supported by the Narrative.
                - Highlight a supported observation.
                - Use a first-person statement when the Narrative supports that perspective.

                For example, if the Narrative states that the user initially set out to build an AI agent but the work naturally evolved into building an orchestrator, a possible opening is:

                "I set out to build an AI agent.

                I ended up building an orchestrator."

                This changes the presentation, not the underlying meaning.

                Avoid openings such as:

                - "A major part of the product thinking was..."
                - "In today's rapidly changing world..."
                - "I've been thinking a lot about innovation..."

                unless that wording is genuinely necessary and supported by the Narrative.

                Do not manufacture drama to make an opening stronger.

                Do not turn an ordinary development into a breakthrough, a mistake, a revelation, or a transformation unless the Narrative explicitly supports that characterization.

                ### 5. Use natural LinkedIn writing

                Write in a clear, direct, conversational style.

                Prefer:

                - Short paragraphs.
                - Generous whitespace.
                - Concise sentences.
                - Natural transitions between connected ideas.
                - Occasional standalone sentences for emphasis.
                - Bullets only when they improve the explanation.

                Avoid:

                - Corporate marketing language.
                - Generic statements about AI, business, innovation, or entrepreneurship.
                - Motivational clichés.
                - Artificial drama.
                - Filler intended only to increase engagement.
                - Mechanical transitions that make the post sound like a report.
                - Repeating the same point in different words.

                The post should sound like a person communicating a specific idea, not like a system describing the contents of a document.

                Do not force a first-person voice if the Narrative does not support it.

                ### 6. End naturally

                The ending should complete the central message, not start a new one.

                Prefer a concise final statement that follows directly from the preceding paragraphs and is supported by the Narrative.

                Do not introduce a new product feature, architectural concept, implementation decision, future ambition, or lesson learned merely to create an ending.

                A question or invitation to discussion is optional. Use one only when it follows naturally from the selected topic and has a clear basis in the Narrative.

                Do not manufacture a debate or append a generic engagement question.

                The post does not need a slogan, a call to action, or a dramatic conclusion.

                ### 7. Preserve semantic fidelity

                LinkedIn optimization is not permission to add meaning.

                You may select, combine, shorten, reorder, and omit statements to improve the post's focus and readability. You must preserve the meaning of the important claims and must not distort the relationships between them.

                Every factual claim and relationship must be supported by the Narrative.

                In particular:

                - Do not turn "X happened" into "X was a breakthrough."
                - Do not turn "X changed" into "X changed everything."
                - Do not turn "X is part of the product" into "X is the future of the product."
                - Do not infer why an event happened when the Narrative does not establish the reason.
                - Do not invent motivations, consequences, emotions, lessons, recommendations, or personal realizations.
                - Do not turn an observation into a broader conclusion that the Narrative does not establish.
                - Do not imply that two events are causally connected merely because they can be arranged into a compelling story.

                Editorial structure may make supported relationships clearer, but it must not create new relationships.

                If the Narrative does not support a compelling angle, choose a simpler supported message rather than inventing one.

                ### 8. Hashtags

                Hashtags are optional.

                If used, include only a small number directly relevant to the selected topic.

                Do not add generic hashtags merely to increase reach.

                ### 9. Final quality check

                Before returning the post, verify internally:

                - Is there exactly one primary topic?
                - Does the opening communicate the strongest supported point?
                - Does every paragraph contribute to the central message?
                - Are there no more than two or three supporting points unless additional details are essential?
                - Have repeated ideas and peripheral details been removed?
                - Does the post progress naturally rather than summarize a series of statements?
                - Does the ending complete the central thought without introducing another topic?
                - Is every factual claim and relationship supported by the Narrative?
                - Have unsupported implications, causes, motivations, and conclusions been avoided?
                - Does the result read like a LinkedIn post rather than an executive summary?

                If any answer is no, revise the post before returning it.

                Return only the finished LinkedIn post. Do not include the selected topic, internal planning, quality checks, or an explanation of the editorial decisions.

                ---

                ## INPUT

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