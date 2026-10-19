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

    private static string CreateLinkedInPostUserMessage(ComposeInput input) =>
        $"""
         # Task: Write a Natural LinkedIn Post

         Transform the supplied Narrative into a finished LinkedIn post.

         Your goal is to create a post that is interesting, easy to read, natural, and faithful to the meaning of the Narrative. The post should feel like thoughtful human communication, not like generic AI-generated content.

         The Narrative defines WHAT the post can say. Your task is to decide HOW to present that information effectively for LinkedIn.

         ## 1. Find the Central Idea

         Read the Narrative and identify the single most important idea that could make an interesting LinkedIn post.

         Build the post around that idea. Select only the details that help explain, develop, or support it. Leave out details that distract from it.

         Do not attempt to summarize the entire Narrative. A focused post with one well-developed idea is better than a broad post that mentions many ideas without explaining any of them properly.

         Choose the central idea based on the strongest support in the Narrative, not on what would make the most dramatic or controversial post.

         ## 2. Introduce the Subject Before Developing the Idea
         
         Do not assume the reader already knows the subject, the background, or the situation behind the Narrative.
         
         **Start by orienting the reader.** Briefly introduce what the post is about before presenting the main insight, conclusion, or argument. Give the reader enough context to understand why the subject matters and why the following discussion is relevant.
         
         The introduction should answer the questions that matter for this particular subject. Depending on the topic, the reader may need to understand:
         - What something is or what it is intended to do.
         - What situation, problem, or development is being discussed.
         - What happened or what was observed.
         - What question or idea the post will explore.
         
         Do not try to answer every question in every post. Include only the context needed to make the central idea understandable.
         
         After introducing the subject, develop the main idea in a natural order. Explain the insight, observation, decision, or argument only after the reader has enough background to follow it.
         
         Do not open with an abstract conclusion, slogan, or technical claim that requires context the reader has not yet received. Do not introduce important terms, products, concepts, or changes in direction halfway through the post without explaining the context first.
         
         Keep the introduction concise. It should orient the reader, not become a long background section or a generic opening.
         
         The introduction must fit the subject. Do not force every post into a personal story, a project description, or a problem–solution structure. Choose the simplest natural introduction that helps the reader understand this particular post.

         ## 3. Write in a Natural Voice

         Write like a thoughtful professional explaining an idea clearly to another professional.

         Use straightforward, everyday English. Many LinkedIn readers are not native English speakers, so make the post accessible without making it simplistic.

         Follow these rules:
         - Prefer short and medium-length sentences.
         - Use familiar words whenever possible.
         - Explain necessary technical terms in plain English.
         - Avoid unnecessarily complex sentence structures.
         - Avoid abstract or vague expressions when a concrete explanation is possible.
         - Use a natural mix of sentence lengths.
         - Connect related thoughts into coherent paragraphs.
         - Use transitions where they help the reader follow the argument.

         Do not make every sentence sound like a polished slogan. Natural writing can be precise and thoughtful without making every line sound memorable.

         Do not force a personal or first-person perspective onto a subject that does not call for one. Choose the point of view that fits the subject and is supported by the Narrative.

         When writing about the author's own experience, use first-person singular ("I" and "my") when supported by the Narrative. Do not switch to "we", "us", or "our", and do not invent a team, colleagues, or other participants.

         ## 4. Develop the Idea Naturally

         The post should have a clear progression from its opening to its conclusion.

         Develop the central idea in an order that helps the reader understand it. Depending on the subject, this might involve explaining a problem, describing an observation, presenting supporting details, exploring an implication, or showing how an understanding changed.

         Use only the progression that fits the material. Do not force every post into a problem–solution story, a personal journey, or a lesson learned.

         Each paragraph should contribute something meaningful to the central idea. Avoid repeating the same point in different words merely to make the post longer.

         You may reorganize, combine, simplify, or shorten information from the Narrative to improve clarity and flow. Preserve the meaning and the relationships between the ideas.

         Do not introduce a new topic late in the post that has not been established or connected to the central idea.

         ## 5. Write for the Reader, Not About the Source Material

         The Narrative is input for writing the post, not a subject to discuss.

         Never mention the Narrative, Synthesis, research process, source material, documentation, evidence collection, or the process used to prepare the post.

         Do not write phrases such as:
         - "The research shows..."
         - "According to the source material..."
         - "The available evidence suggests..."
         - "Based on the documentation..."
         - "The information provided does not establish..."

         Instead, communicate the supported idea directly and naturally.

         The reader should see a finished LinkedIn post, not an explanation of how the post was produced or what information was available to the model.

         ## 6. Preserve Meaning and Factual Accuracy

         The Narrative is the boundary of what the post may claim.

         Every factual statement, conclusion, and relationship in the post must be supported by the Narrative.

         You may improve the expression, structure, and readability, but you must not change the underlying meaning.

         In particular:
         - Do not invent facts, examples, events, people, experiences, or results.
         - Do not invent motivations, intentions, or reasons for decisions.
         - Do not introduce unsupported causes, effects, or relationships.
         - Do not turn a specific observation into a general industry trend.
         - Do not turn a possibility into a certainty.
         - Do not present plans or intentions as completed achievements.
         - Do not imply that something has been tested, proven, measured, or validated unless the Narrative supports that claim.
         - Preserve important limitations and uncertainty when they affect the meaning of the central idea.
         - Do not exaggerate the significance or impact of the subject.

         You may draw attention to an implication only when that implication follows directly from the Narrative. Do not add a broader lesson or conclusion simply because it would make the post more engaging.

         Do not include defensive disclaimers about claims the post does not make, whether something has been proven, or what the author has not done. Simply describe the supported information accurately. Leave out unsupported details rather than explaining their absence to the reader.

         If the Narrative does not provide enough information to support a particular detail, omit that detail. Do not fill the gap with assumptions.

         ## 7. Avoid Artificial LinkedIn Writing

         Do not rely on familiar AI-generated LinkedIn patterns. In particular, avoid:

         - A series of dramatic one-line paragraphs.
         - Artificial cliffhangers or exaggerated suspense.
         - Generic openings such as "In today's rapidly changing world..."
         - Formulaic contrasts such as "It's not about X. It's about Y." unless that contrast genuinely expresses the central idea.
         - Empty statements about how important, powerful, exciting, or transformative something is.
         - Repeated rhetorical questions.
         - Unnecessary summaries that restate the entire post at the end.
         - Generic advice or motivational lessons that do not follow directly from the subject.
         - Grand claims about the future or the wider world that are not supported by the Narrative.
         - Buzzwords, slogans, and unnecessarily abstract language.
         - Forced emotional language or artificial enthusiasm.

         Avoid quotation marks around ordinary phrases or ideas. Express the meaning directly in your own words. Use direct quotations only when reproducing someone's exact words is essential and supported by the Narrative.

         Do not turn every subject into a personal story or a lesson for the reader. Let the subject determine the form of the post.

         Do not end with a generic invitation such as "What do you think?", "If you're working on something similar...", or "It's worth asking whether..." unless a genuine, relevant question is a natural part of the post.

         Finish with a concise conclusion that follows naturally from the discussion. It may reinforce the central idea, explain its significance, or leave the reader with a concrete observation. Do not manufacture a dramatic ending.

         ## 8. Final Review

         Before producing the post, check it against the following questions:

         1. Is there one clear central idea?
         2. Does the opening establish enough context for the reader to understand the subject?
         3. Does every new concept follow naturally from what came before?
         4. Is the post easy to read for someone who is not a native English speaker?
         5. Does it sound like natural professional communication rather than generic AI writing?
         6. Does every paragraph contribute something useful?
         7. Have repeated ideas and distracting details been removed?
         8. Are all factual claims and relationships supported by the Narrative?
         9. Have unsupported details, invented experiences, exaggerated claims, and artificial disclaimers been avoided?
         10. Does the ending follow naturally from the central idea?

         Revise the post if necessary before returning it.

         ## Output Requirements

         Return only the finished LinkedIn post in the output structure required by the system.

         Do not include an explanation of your choices, a summary of the Narrative, alternative versions, or commentary about the writing process.

         Do not add hashtags unless they are explicitly required by the system or supported by the task instructions.

         ## Input

         Narrative:

         {input.Narrative.Content}

         ## Output

         Return only the finished LinkedIn post, using the output structure required by the Compose system prompt.

         Do not include analysis, explanations, or commentary about the writing process.
         """;

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