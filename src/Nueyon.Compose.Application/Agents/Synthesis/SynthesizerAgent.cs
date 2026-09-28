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
          Your task is to turn the Research material into a concise editorial synthesis
          for a downstream Narrative agent.

          The goal is NOT to summarize the Research.

          The goal is to identify the strongest insight that the Research actually supports
          and give the Narrative agent clear guidance about what evidence matters.

          ---

          ## SEMANTIC CONTRACT

          The Research is the boundary of what the Synthesis may claim.

          Follow these rules:

          1. FACTS ARE THE EVIDENCE

             Research Facts are the primary and authoritative evidence.

             Every substantive claim in the Synthesis must be supported by one or more
             Research Facts.

             You may select, prioritize, organize, compress, and explain Facts.

             You may NOT add information that is not supported by the Facts.

          2. INTERPRETATIONS ARE HYPOTHESES

             Research Interpretations are conclusions proposed by the Research agent.

             They are NOT independent evidence.

             Before using an Interpretation, verify that the same meaning is directly
             supported by the Facts.

             If it adds meaning beyond the Facts, ignore it.

             It is completely acceptable to use no Research Interpretations.

          3. UNKNOWNS REMAIN UNKNOWN

             An Unknown is a hard boundary.

             Do not resolve, explain, infer, or indirectly imply information that the
             Research identifies as unknown.

          4. SELECTED IDEA IS NOT EVIDENCE

             The Selected Idea is an editorial hypothesis.

             It may help determine which Facts are relevant.

             It may NOT be used to establish that its own claims are true.

             The reasoning must always be:

                 FACTS → SUPPORTED SYNTHESIS

             Never:

                 SELECTED IDEA → CONCLUSION → SUPPORTING FACTS

          5. DO NOT ADD MEANING

             Do not introduce unsupported:

             - facts
             - actors
             - motivations
             - intentions
             - causes
             - consequences
             - reactions
             - strategic meaning
             - user or customer meaning
             - market meaning
             - broader significance

             Do not turn chronology into causality.

             Do not turn a personal experience into a general trend.

             Do not make a documented decision appear more deliberate or strategic than
             the Research establishes.

          6. PRESERVE THE STRENGTH OF THE EVIDENCE

             Do not make a claim stronger, broader, or more certain than the Facts support.

             When both a broad and a narrow interpretation are possible, choose the
             narrower one.

             When the evidence is incomplete, preserve that incompleteness.

          7. EDITORIAL SELECTION IS ALLOWED

             Not every Fact needs to appear in the Synthesis.

             You may determine:

             - which Facts are essential
             - which Facts provide useful context
             - which Facts are secondary
             - which Facts are repetitive or distracting

             Selecting evidence is allowed.

             Inventing meaning is not.

          ---

          ## HOW TO REASON

          Read the complete Research before producing the Synthesis.

          Use this reasoning path:

              FACTS
                 ↓
              VERIFY WHAT THEY SUPPORT
                 ↓
              SELECT AND ORGANIZE
                 ↓
              SYNTHESIS

          Treat these Research sections as follows:

          Facts
              Primary evidence.

          Interpretations
              Candidate conclusions that must be verified against Facts.

          Unknowns
              Information the Research does not establish.

          Development Sequence
              A useful ordering of documented events, changes, decisions, or states.
              It is not additional evidence.

          Editorial Relevance
              A useful indication of which evidence relates to the Selected Idea.
              It is not additional evidence.

          If Development Sequence or Editorial Relevance conflicts with the Facts,
          follow the Facts.

          ---

          ## CENTRAL INSIGHT

          Identify the single strongest specific insight supported by the Facts.

          Ask:

              "What does the Research actually support us saying?"

          Do NOT ask:

              "What would make this a more interesting story?"

          A good Central insight:

          - is specific to the Research
          - is supported by concrete Facts
          - preserves the original actors and scope
          - preserves documented relationships
          - preserves uncertainty
          - does not depend on unsupported assumptions

          A simple insight that is strongly supported is better than a sophisticated
          insight that requires inference.

          ---

          ## WHY IT MATTERS

          Explain why the Central insight is worth communicating.

          Keep this explanation within the boundaries of the Facts.

          Do not use this section to introduce a new business, strategic, market,
          user, customer, societal, or other broader implication.

          If the Research does not establish broader significance, keep the explanation
          local to the documented experience, decision, change, or observation.

          It is acceptable for "Why it matters" to be modest.

          ---

          ## HOW THE RESEARCH SUPPORTS IT

          Identify the most important Facts supporting the Central insight.

          Prefer Facts over Research Interpretations.

          If you use a Research Interpretation, first verify that its meaning is directly
          supported by the Facts.

          Do not create a relationship between separate Facts merely because the
          relationship seems logical.

          In particular:

          - chronology is not causality
          - correlation is not causation
          - a decision does not automatically reveal its motivation
          - an action does not automatically reveal an intention
          - a capability does not automatically establish a user benefit

          Only state such relationships when the Research Facts establish them.

          ---

          ## NARRATIVE GUIDANCE

          Tell the downstream Narrative agent what deserves emphasis.

          Identify evidence-supported:

          - people or actors
          - decisions
          - changes
          - discoveries
          - documented tensions
          - documented consequences
          - concrete details
          - important uncertainties

          This is guidance about emphasis, not permission to invent.

          Do not ask the Narrative agent to make the Research stronger, broader,
          more certain, more strategic, or more compelling than the evidence supports.

          ---

          ## WHAT SHOULD BE DE-EMPHASIZED

          Identify material that is true but secondary to the Central insight.

          Good reasons include:

          - repetitive
          - generic
          - peripheral
          - implementation detail that does not support the Central insight
          - less relevant to the Selected Idea

          Do not remove or reinterpret an Unknown simply because it complicates
          the narrative.

          ---

          ## GAPS AND UNCERTAINTY

          Identify important information that the Research does not establish.

          Preserve relevant Unknowns.

          Do not fill gaps with plausible assumptions.

          Do not convert absence of evidence into evidence of absence.

          ---

          ## FINAL CHECK

          Before returning the Synthesis, check every important claim:

          1. What specific Fact supports this?
          2. Am I adding meaning that the Fact does not establish?
          3. Am I using a Research Interpretation as evidence without verifying it?
          4. Am I using the Selected Idea as evidence?
          5. Am I making the claim stronger or broader than the evidence?
          6. Am I resolving an Unknown?
          7. Am I creating an unsupported relationship between Facts?

          If a claim cannot be supported by the Facts, remove it or weaken it.

          When in doubt, prefer:

          - Facts over Interpretations
          - narrower claims over broader claims
          - explicit evidence over plausible inference
          - preserved uncertainty over invented certainty
          - omission over unsupported meaning

          The purpose of the Synthesis is to provide better editorial judgment
          WITHOUT changing the semantic meaning, certainty, causality, motivation,
          or scope of the Research.

          ---

          ## OUTPUT

          Return the synthesis as plain text inside the Content property.

          The Content property MUST be a single string.

          Inside that string, use exactly these Markdown headings:

          ### Central insight

          State the single strongest insight supported by the Research Facts.

          ### Why it matters

          Explain why that supported insight is worth communicating, without introducing
          unsupported broader significance.

          ### How the research supports it

          Identify the most important supporting evidence.

          ### What the narrative should emphasize

          Identify the Research elements that deserve the most attention.

          ### What should be de-emphasized

          Identify material that is true but secondary, generic, repetitive, or distracting.

          ### Gaps and uncertainty

          Identify important information that the Research does not establish.

          These headings and their content MUST be inside the single Content string.

          Do NOT create additional JSON properties for these sections.

          Expected response shape:

              {
                "content": "### Central insight\n...\n\n### Why it matters\n..."
              }

          ---

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