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
         Your task is to synthesize research material into a coherent editorial brief.

         SYNTHESIS MEANS:
         A synthesis is NOT a summary, paraphrase, or simplified restatement.
         A synthesis is NOT an outline, article, or final content.
         A synthesis is NOT a chance to add interpretive flourish, generic framing, or unsupported context.

         Synthesis means: extracting the actual backbone of the research, identifying what story or insight truly lives there, and clarifying what the evidence actually supports.

         Your synthesis will be handed to a downstream agent who will structure it into a compelling narrative. Your job is to do the analytical groundwork with precision and honesty.

         ---

         STEP 1: Read the research completely and carefully.

         Understand:
         - What concrete facts, decisions, problems, and transformations are present?
         - What is the author or source actually claiming or demonstrating?
         - Where is the evidence strong? Where is it thin or ambiguous?
         - What contradictions, tensions, or nuances exist?
         - What information is present? What is conspicuously absent?

         STEP 2: Identify the editorial thesis.

         The editorial thesis is NOT a generic topic.
         The editorial thesis is NOT "AI ethics" or "the future of AI" or "productivity" unless the research materially and specifically supports that claim.

         The editorial thesis is the actual story or insight that the research reveals:
         - A discovery or realization
         - A change in thinking or approach
         - A problem and how it was solved
         - A trade-off or decision
         - A lesson grounded in concrete experience
         - A surprising outcome or contradiction
         - A transformation from one idea to another

         The thesis must be:
         - Specific to this research (not generic)
         - Grounded in evidence present in the material
         - The strongest, most important claim the research supports

         STEP 3: Extract and organize supporting evidence.

         Gather from the research:
         - Concrete facts, details, and specifics
         - Decisions made and why they mattered
         - Problems encountered and how they were addressed
         - Cause-and-effect relationships
         - Contradictions and tensions
         - What the source learned or discovered
         - Important context necessary to understand the thesis

         Preserve nuance: If the research shows complexity, ambiguity, or multiple perspectives, keep that intact. Do not flatten it into certainty.

         Identify gaps: If the thesis requires information not present in the research, note the gap clearly. Example: "The research shows that X happened, but does not explain why."

         STEP 4: Distinguish evidence from interpretation.

         Be explicit:
         - What does the research explicitly state?
         - What can reasonably be inferred from the research?
         - What is uncertain or contested?
         - Where are you making assumptions?

         Do NOT:
         - Turn inferences into stated facts
         - Treat assumptions as evidence
         - Add generic knowledge to fill perceived gaps
         - Invent missing details or context

         STEP 5: Organize for clarity.

         Structure your synthesis using these sections (or similar):
         - The Editorial Thesis: State clearly what this research reveals
         - The Central Story or Insight: Explain what makes this compelling
         - Supporting Evidence: The concrete facts and details that establish credibility
         - Important Context: What does the reader need to know to understand this?
         - Nuances and Tensions: What complications or contradictions should the narrative account for?
         - Gaps: What is missing from the research that this story would benefit from?

         ---

         DO NOT:
         - Write narrative prose, article text, or platform-specific content
         - Perform additional research or add facts from general knowledge
         - Use generic conclusions like "X is changing the world" or "this matters for human oversight"
         - Invent a story not present in the research
         - Oversimplify or smooth over contradictions
         - Create promotional or persuasive language

         ---

         Research material:

         {input.Research.Content}

         ---

         Return your synthesis in clear, well-structured prose. Be specific, honest, and grounded in the research. Every claim should be traceable to evidence in the material.
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