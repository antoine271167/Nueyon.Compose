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

                Compose must not add meaning. Every factual claim and relationship in the final content must be
                supported by the Narrative, following the same semantic-fidelity rules that apply to all formats.

                ## LINKEDIN POST

                Create a genuinely engaging LinkedIn post from the supplied Narrative.

                The goal is:

                ATTENTION + READABILITY + ENGAGEMENT + FACTUAL INTEGRITY

                The post should make someone want to stop scrolling and continue reading.

                ### Structure

                Prefer:

                - a strong opening hook
                - short paragraphs
                - generous whitespace
                - concise sentences
                - one clear central idea
                - natural conversational language
                - a strong progression toward the central point
                - a concise ending

                The post should feel like a real LinkedIn post, not a shortened article.

                ### Hook

                The opening should immediately communicate an interesting or surprising point that is supported by the Narrative.

                A hook may:

                - state a supported fact directly
                - use a supported contrast
                - use a supported observation
                - use a supported first-person statement when the Narrative uses first person

                Example:

                Narrative:
                "The user initially set out to build an AI agent, but the work naturally evolved into building an orchestrator."

                Good:

                "I set out to build an AI agent.

                I ended up building an orchestrator."

                This improves presentation without adding meaning.

                Do NOT invent drama to create a stronger hook.

                Do not add claims such as:

                "I made a huge mistake."

                "This changed everything."

                "I discovered the future of AI."

                unless the Narrative explicitly supports them.

                ### Writing style

                Use:

                - short paragraphs
                - whitespace
                - direct language
                - natural conversational phrasing
                - occasional one-line emphasis
                - bullets or numbered lists when they improve readability

                Avoid:

                - generic introductions
                - corporate marketing language
                - exaggerated claims
                - artificial drama
                - motivational clichés
                - generic statements about business, AI, innovation, or entrepreneurship
                - filler written only to increase engagement

                ### Engagement

                The post may end with a question or invitation to discussion when it follows naturally from the Narrative.

                Do not invent an opinion, question, controversy, or debate that is not supported by the Narrative.

                For example, do not manufacture:

                "What do you think the future of AI will look like?"

                unless the Narrative actually provides a basis for that question.

                ### Hashtags

                Hashtags are optional.

                If used, use only a small number of relevant hashtags directly supported by the subject of the Narrative.

                Do not add generic hashtags merely to increase reach.

                ### Semantic boundary

                LinkedIn optimization is NOT permission to add meaning.

                Attention must come from the way the supplied meaning is expressed, not from invented significance.

                Do not turn:

                "X happened"

                into:

                "X was a breakthrough."

                Do not turn:

                "X changed"

                into:

                "X changed everything."

                Do not turn:

                "X is part of the product"

                into:

                "X is the future of the product."

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