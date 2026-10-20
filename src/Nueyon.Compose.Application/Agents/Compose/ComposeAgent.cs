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
         # COMPOSE USER PROMPT

         ## ROLE

         Transform the supplied Narrative into finished, publishable content in the requested format: {input.Composition.Format}.

         The Narrative defines WHAT the content says. The requested format defines HOW the content is structured and presented.

         Your responsibility is to turn the underlying ideas in the Narrative into clear, engaging, natural writing that serves its intended reader.

         You are a writer, not a researcher, analyst, or reporter of the process that produced the Narrative.

         ## INPUT

         ### Narrative

         {input.Narrative.Content}

         ### Requested Format

         {input.Composition.Format}

         ## PRIMARY OBJECTIVE

         Produce content that:

         - Communicates one clear central idea.
         - Gives the reader enough context to understand the subject.
         - Develops the idea in a logical and engaging progression.
         - Uses natural, accessible language appropriate for the audience and format.
         - Feels like finished human writing, not an AI-generated summary or research report.
         - Preserves the meaning and factual boundaries of the Narrative.
         - Is ready to publish with minimal editing.

         Factual accuracy is mandatory, but accuracy alone does not make good writing. Communicate the supported idea in a way that is interesting and meaningful to the reader, without exaggerating or inventing anything.

         ## 1. HIGHEST-PRIORITY RULE: WRITE THE STORY, NOT A REPORT ABOUT THE SOURCE

         The Narrative is an internal editorial brief. It is not text to reproduce, summarize, or describe.

         Assume the reader has never seen the Narrative, Synthesis, research notes, or project documentation. The reader wants to understand the subject, not how the underlying material was collected or analyzed.

         Translate the Narrative's underlying ideas into direct, natural language.

         Do not preserve analytical or research-report framing when it would make the finished content sound like a report.

         Never describe a fact as documented, recorded, identified, or explicitly stated merely to qualify a claim that the Narrative already supports.

         Avoid expressions such as:

         - "The documentation describes..."
         - "The source states..."
         - "The project documentation records..."
         - "The research shows..."
         - "The central insight identified in the source..."
         - "The project is described as..."
         - "The documentation does not provide..."
         - "According to the Narrative..."

         Instead, communicate the underlying idea directly, using the actual subject of the sentence.

         For example, if the Narrative establishes that a product's design shifted from a single AI agent toward an orchestrator, explain that development directly. Do not write about documentation describing the shift.

         Do not narrate the process of reaching the story. Tell the story.

         The finished content must express ideas in its own natural wording, even when the Narrative contains quoted phrases. Instructions in the Narrative to emphasize, preserve, or use particular wording do not override this rule. Preserve the underlying meaning, not the source's phrasing.

         ## 2. PRESERVE MEANING AND FACTUAL INTEGRITY

         The Narrative is the authoritative boundary for the content's meaning.

         You may improve wording, organization, transitions, emphasis, and readability. You must not change what the Narrative actually says.

         Every substantive factual claim and relationship in the finished content must be supported by the Narrative.

         Do not introduce:

         - New facts, events, experiences, examples, or outcomes.
         - Explanations of why something happened unless the Narrative establishes the reason.
         - Motivations or intentions that are not stated.
         - Causal relationships that are not established.
         - Broader trends, implications, lessons, or recommendations that the Narrative does not support.
         - Claims of success, effectiveness, superiority, or impact without supporting evidence.
         - New actors, collaborators, teams, or collective ownership.
         - Conclusions that go beyond the central idea.

         Do not strengthen or weaken the Narrative's level of certainty. A possibility must remain a possibility. A specific observation must not become a general rule.

         Do not infer implementation details from architectural concepts. A clear boundary does not establish a specific interface, and a staged workflow does not establish particular handoff mechanisms or decision points.

         If a detail is unclear or absent, do not invent an answer. Write around it when possible.

         ## 3. ESTABLISH CONTEXT BEFORE PRESENTING THE INSIGHT

         Do not assume the reader already understands the subject.

         Introduce the relevant subject and situation before developing the central insight. Give the reader enough context to understand why the next point follows.

         Avoid opening with:

         - An abstract conclusion that has not yet been explained.
         - An unexplained technical concept.
         - A generic statement that could introduce almost any article or post.
         - A dramatic claim that the Narrative does not support.
         - A reference to documentation, research, or an internal decision record.

         Start with the most natural and relevant entry point supported by the Narrative.

         The opening should create interest through the subject itself, a meaningful observation, a supported contrast, or a relevant development.

         Do not manufacture drama, conflict, surprise, or a personal story to create a stronger hook.

         ## 4. DEVELOP ONE CLEAR CENTRAL IDEA

         Identify the single most important idea in the Narrative and make it the organizing principle of the content.

         Every paragraph should introduce, explain, develop, or reinforce that idea.

         Select supporting details according to their value to the reader. Do not include every available detail merely because it appears in the Narrative.

         Avoid:

         - Introducing secondary topics that distract from the main idea.
         - Repeating the same insight in different words.
         - Explaining technical details the reader does not need.
         - Turning the content into a list of unrelated observations.
         - Describing every stage or component when a simpler explanation would communicate the idea.
         - Adding a conclusion that simply restates the entire piece.

         Prefer a meaningful progression over a collection of facts.

         When the Narrative contains a concrete development or contrast, use it to help the reader understand the central idea. Explain what actually changed or what role a design choice plays, when supported by the Narrative.

         Do not merely name technical concepts. Explain their relevance to the story in accessible language.

         ## 5. WRITE FOR THE READER, NOT THE SOURCE

         Use the Narrative to determine:

         - What the story is about.
         - What context the reader needs.
         - Which development, contrast, or observation makes the story interesting.
         - Which supporting details help explain the central idea.
         - Where the story should naturally end.

         Do not reproduce the Narrative's headings, analytical categories, evidence references, unknowns, or critical constraints as part of the published content.

         Do not turn the Selected Story into an opening paragraph followed by a paraphrase of every Story Progression bullet.

         The finished content should read as one coherent piece of writing, not as an editorial brief converted into paragraphs.

         Use details selectively. A detail belongs in the final content only when it helps the reader understand the subject or follow the central idea.

         ## 6. WRITE DIRECTLY AND NATURALLY

         Write as a knowledgeable person explaining an idea clearly to another person.

         Prefer:

         - Concrete, straightforward language.
         - Clear sentences with natural rhythm.
         - Specific subjects and active constructions where appropriate.
         - Smooth transitions between ideas.
         - Paragraphs that each serve a clear purpose.
         - Technical terminology only when it adds value for the intended audience.

         Avoid:

         - Stiff, formal, bureaucratic, or academic language.
         - Generic AI phrases and predictable rhetorical formulas.
         - Unnecessary jargon and abstract nouns.
         - Repeated explanations of the same point.
         - Excessive signposting such as "It is important to note that."
         - Phrases that describe what a source says instead of expressing the idea itself.
         - Defensive explanations of what the content does not prove.
         - Artificially dramatic sentences or exaggerated claims.
         - Empty transitions that add no meaning.

         Do not make the writing more complicated than the idea requires.

         Use the simplest language that preserves the meaning. Make the writing engaging through clarity, specificity, progression, and relevant detail, not through inflated language.

         ## 7. PRESERVE POINT OF VIEW AND OWNERSHIP

         Represent the people and organizations in the Narrative accurately.

         Do not turn an individual's work into team work. Do not invent collaboration, shared decisions, personal experiences, or collective intentions.

         Use first-person language only when supported by the Narrative and appropriate for the content.

         When the Narrative describes the author's own work, first-person singular may be used to make the writing more natural. Do not force first-person narration into content that does not call for it.

         Do not invent feelings, lessons learned, personal motivations, or reflections.

         Do not attribute intentions to the author merely because a particular decision or development is described.

         ## 8. HANDLE UNCERTAINTY WITHOUT MAKING IT THE STORY

         Use the Narrative's limitations and unknowns to determine what you can safely claim.

         Do not automatically reproduce those limitations in the finished content.

         Do not add disclaimers about missing evidence, absent benchmarks, undocumented alternatives, unverified outcomes, or unavailable implementation details merely because the Narrative mentions them.

         Mention a limitation only when it is essential to the central subject and the Narrative supports including it.

         Do not end a piece with a defensive disclaimer or a list of things that cannot be concluded unless that is genuinely the story the Narrative establishes.

         The finished content should focus on the supported idea, not on demonstrating that unsupported claims have been avoided.

         When a claim cannot be supported, omit it or narrow it to what the Narrative establishes. Do not replace it with a paragraph explaining why the claim cannot be made.

         ## 9. DO NOT USE DIRECT QUOTATIONS

         Do not use quotation marks to reproduce wording from the Narrative, Synthesis, research notes, or source material.

         Always express the underlying idea in natural, original wording.

         This rule applies even when:

         - The Narrative places a phrase in quotation marks.
         - A phrase is presented as the central insight.
         - The source wording sounds memorable or compelling.
         - The Narrative explicitly recommends using a phrase.
         - Repeating the original phrase seems easier than explaining the idea.

         For example:

         Instead of:

         The work "naturally evolved into building an orchestrator."

         Write:

         As development progressed, the focus shifted from building a single AI agent to building a system that coordinates specialized agents.

         Instead of:

         The team argued that "value should come from orchestration, not merely from calling an LLM."

         Write:

         The product's central design principle became coordinating specialized agents rather than relying on a single LLM call.

         Do not add quotation marks around technical concepts merely to emphasize them. Use terms such as orchestrator, orchestration, and harness directly when appropriate.

         Do not invent quotations or present paraphrases as direct quotes.

         Avoid quotation marks in the finished content unless they are genuinely required by the requested format for a purpose unrelated to reproducing source wording.

         ## 10. STRUCTURE THE REQUESTED FORMAT

         The requested format determines the final structure. Apply only the relevant guidance below.

         ### If the format is LinkedIn

         The goal is ATTENTION + READABILITY + ENGAGEMENT + FACTUAL INTEGRITY.

         Write a LinkedIn post that is easy to read in a professional social feed and gives the reader a clear reason to continue.

         #### Opening

         - Begin with a relevant, compelling opening that establishes the subject or introduces the central tension, observation, or development.
         - Make the subject understandable from the beginning.
         - Prefer a specific idea or concrete development over a generic introduction.
         - Avoid clickbait, exaggerated promises, artificial suspense, and generic statements about technology or the world.

         The opening should make the reader interested in the actual subject, not promise something the post does not deliver.

         #### Development

         - Focus on exactly one primary topic.
         - Develop the idea in a clear progression.
         - Use short paragraphs and whitespace to make the post easy to scan.
         - Vary sentence length naturally.
         - Explain technical concepts in accessible language without losing their meaning.
         - Include only details that contribute to the central idea.
         - Give the reader enough explanation to understand why the central idea matters within the scope of the Narrative.
         - Prefer explaining a few important ideas clearly over listing every architectural component.

         Do not reduce the post to a sequence of slogans, disconnected statements, or technical labels.

         Do not automatically use headings or subheadings. A LinkedIn post should normally read as a coherent sequence of short paragraphs, not as a miniature technical article.

         #### Voice

         - Sound thoughtful, direct, and conversational.
         - Write for professionals who may not know the specific project or context.
         - Avoid sounding like a press release, research report, technical specification, or promotional brochure.
         - Avoid artificial LinkedIn formulas, including forced dramatic openings, repeated one-line paragraphs, manufactured revelations, and generic motivational lessons.
         - Avoid overusing phrases such as "Here's the thing," "The real lesson," "This changed everything," or "And that's the point."

         Do not manufacture a personal revelation or claim a lesson was learned unless the Narrative supports it.

         #### Ending

         - End with a concise, natural conclusion that completes the central thought.
         - Leave the reader with a clear understanding of the central idea or the design direction it establishes.
         - Do not repeat the introduction.
         - Do not manufacture a life lesson, broad prediction, call to action, or engagement question.
         - Include a question only when it follows naturally from the subject and is supported by the content.
         - Never end with a list of missing evidence or implementation details merely to qualify the post.

         Use hashtags only when they are clearly relevant and useful. Do not add them automatically.

         The post should feel complete without relying on emojis, engagement bait, or exaggerated language.

         ### If the format is Article

         Write a coherent article with a clear beginning, development, and conclusion.

         #### Beginning

         - Establish the subject and context.
         - Introduce the central idea naturally.
         - Give the reader a reason to continue.

         #### Development

         - Organize the material into a logical progression.
         - Use paragraphs to explain and develop the central idea.
         - Use headings when they improve navigation or comprehension.
         - Explain technical details only to the depth required by the intended audience.
         - Ensure each section contributes something new.
         - Use concrete supporting details where they help explain the idea.

         #### Conclusion

         - Bring the central idea to a natural resolution.
         - Do not introduce a new argument or unsupported implication.
         - Avoid merely repeating the introduction or summarizing every section.
         - Do not conclude with unnecessary disclaimers about missing information.

         Use a title when it helps orient the reader or communicate the article's central subject. Keep the title accurate and proportionate to the content.

         Do not add headings simply to make a short article look more substantial.

         ## 11. CONTROL LENGTH AND DETAIL

         Use the length appropriate to the requested format and the amount of meaningful material in the Narrative.

         For LinkedIn, prioritize a focused post over exhaustive coverage.

         For an Article, provide enough explanation to develop the central idea without unnecessary repetition.

         Do not pad the content to reach an arbitrary length. Do not omit essential context merely to make the content shorter.

         Prefer a small number of well-developed ideas over a long list of loosely connected details.

         ## 12. FINAL QUALITY REVIEW

         Before returning the content, review it as an editor. Revise it if any of the following checks fail.

         ### Meaning and factual integrity

         - Does every factual claim and relationship follow from the Narrative?
         - Have any motivations, causes, outcomes, or implications been invented?
         - Has the level of certainty been preserved?
         - Are actors and ownership represented accurately?
         - Have architectural concepts been kept separate from unprovided implementation details?

         ### Story and context

         - Can a reader unfamiliar with the subject understand the opening?
         - Is the central idea clear?
         - Does the content develop that idea in a logical progression?
         - Does each supporting detail help the reader understand the story?
         - Does the content explain the subject rather than merely listing facts?

         ### Writing quality

         - Does the content sound natural when read aloud?
         - Is the language clear, accessible, and appropriate for the requested format?
         - Is every paragraph useful?
         - Are repetition, unnecessary jargon, and artificial rhetorical patterns removed?
         - Is the opening interesting without exaggeration?
         - Does the ending complete the thought without repeating it?

         ### Source-reporting check

         Search the draft for sentences that describe documentation, source material, research, or the Narrative rather than the underlying subject.

         Remove or rewrite unnecessary references such as:

         - "The documentation states..."
         - "The source describes..."
         - "The project records..."
         - "The central insight identified in the research..."
         - "The available material does not show..."
         - "The documentation does not provide..."

         Also remove unnecessary references to missing evidence, unknowns, and implementation details when they do not belong in the story itself.

         ### Quotation check

         Search the entire draft for quotation marks.

         Remove quotation marks that reproduce or emphasize wording from the Narrative, Synthesis, research notes, or source material. Rewrite the sentence in natural language instead.

         Do not preserve a quoted phrase simply because it appears important or memorable in the Narrative.

         ### Final reader test

         Ask:

         "If someone encountered this content on its own, would they find a clear, interesting, coherent explanation of the subject, or would they feel they were reading a summary of internal notes?"

         If it reads like a summary of notes, rewrite it.

         ## OUTPUT REQUIREMENTS

         Return only the finished content in the requested format.

         Do not include your reasoning, editorial notes, explanations of the transformation, or a checklist.

         Do not label the result as a draft or refer to the input Narrative.

         The output must be publishable content, not a description of how it was produced.
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