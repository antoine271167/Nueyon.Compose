using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Research;

/// <summary>
///     Independent assessment of Research and its actual downstream projection.
/// </summary>
public sealed class ResearchSemanticJudge(AIAgent agent)
{
    internal static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        AllowDuplicateProperties = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    private static readonly string[] _categories =
        ["sourceFidelity", "evidenceCoverage", "epistemicAccuracy", "selectedIdeaRelevance", "downstreamIntegrity"];

    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));
    public static string GetSystemInstructions() => PromptLoader.Load("Research/ResearchValidation.system.prompt.md");

    public async Task<ResearchVerdict> EvaluateAsync(ResearchInput input, ResearchResult research,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(research);
        var payload = JsonSerializer.Serialize(new
        {
            source = input.Input.Content, selectedIdea = input.SelectedIdea.Idea, research,
            synthesisInput = SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content)
        }, JsonOptions);
        var response = await _agent.RunAsync(payload, null, new ChatClientAgentRunOptions(new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<ResearchVerdict>(JsonOptions, nameof(ResearchVerdict))
        }), cancellationToken);
        ResearchVerdict verdict;
        try
        {
            verdict = JsonSerializer.Deserialize<ResearchVerdict>(response.Text, JsonOptions)
                      ?? throw new JsonException("Null verdict");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Research judge returned invalid JSON.", ex);
        }

        if (verdict.BlockingIssues is null || verdict.BlockingIssues.Count > 5 ||
            verdict.Accepted != (verdict.BlockingIssues.Count == 0))
        {
            throw new InvalidOperationException("Research judge acceptance must agree with at most five blockers.");
        }

        foreach (var issue in verdict.BlockingIssues)
        {
            if (!_categories.Contains(issue.Category, StringComparer.Ordinal) ||
                string.IsNullOrWhiteSpace(issue.Problem) || string.IsNullOrWhiteSpace(issue.ResearchLocation) ||
                string.IsNullOrWhiteSpace(issue.SourceQuote) || string.IsNullOrWhiteSpace(issue.Constraint) ||
                !input.Input.Content.Contains(issue.SourceQuote, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "Research judge diagnosis needs an exact original-source anchor and complete fields.");
            }
        }

        return verdict with { BlockingIssues = verdict.BlockingIssues.ToArray() };
    }
}

public sealed record ResearchVerdict
{
    public required bool Accepted { get; init; }
    public required IReadOnlyList<ResearchBlockingIssue> BlockingIssues { get; init; }
}

public sealed record ResearchBlockingIssue
{
    public required string Category { get; init; }
    public required string ResearchLocation { get; init; }
    public required string Problem { get; init; }
    public required string SourceQuote { get; init; }
    public required string Constraint { get; init; }
}