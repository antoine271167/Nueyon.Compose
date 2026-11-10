using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>Checks generated ideas against the unchanged source using a separate model call.</summary>
public sealed class IdeaSemanticJudge(AIAgent agent)
{
    internal static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        AllowDuplicateProperties = false
    };

    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));

    private const int MaxBlockingIssues = 3;

    public static string GetSystemInstructions() => PromptLoader.Load("Idea/IdeaValidation.system.prompt.txt");

    internal async Task<IReadOnlyList<BlockingIssue>> FindBlockingIssuesAsync(
        string sourceContent,
        Domain.Idea selectedIdea,
        CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Serialize(new { source = sourceContent, idea = selectedIdea }, JsonOptions);
        var options = new ChatClientAgentRunOptions(new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<Verdict>(JsonOptions, nameof(Verdict))
        });
        var response = await _agent.RunAsync(message, null, options, cancellationToken);

        Verdict? verdict;
        try
        {
            verdict = JsonSerializer.Deserialize<Verdict>(response.Text, JsonOptions);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Idea semantic validation returned an invalid verdict.", ex);
        }

        if (verdict?.BlockingIssues is not { } issues)
        {
            throw new InvalidOperationException(
                "Idea semantic validation returned an invalid verdict: missing blocker list.");
        }

        if (issues.Count > MaxBlockingIssues)
        {
            throw new InvalidOperationException(
                $"Idea semantic validation returned an invalid verdict: more than {MaxBlockingIssues} blockers.");
        }

        if (verdict.Accepted != (issues.Count == 0))
        {
            throw new InvalidOperationException(
                "Idea semantic validation returned an invalid verdict: acceptance must agree with the blocker list.");
        }

        foreach (var issue in issues)
        {
            if (issue is null || string.IsNullOrWhiteSpace(issue.Category) ||
                string.IsNullOrWhiteSpace(issue.Field) || string.IsNullOrWhiteSpace(issue.Problem) ||
                string.IsNullOrWhiteSpace(issue.SourceEvidence) || string.IsNullOrWhiteSpace(issue.Constraint))
            {
                throw new InvalidOperationException(
                    "Idea semantic validation returned an invalid verdict: each blocker needs a complete diagnosis.");
            }
        }

        return issues.ToArray();
    }

    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    private sealed record Verdict
    {
        public required bool Accepted { get; init; }
        public required IReadOnlyList<BlockingIssue> BlockingIssues { get; init; }
    }

    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    internal sealed record BlockingIssue
    {
        public required string Category { get; init; }
        public required string Field { get; init; }
        public required string Problem { get; init; }
        public required string SourceEvidence { get; init; }
        public required string Constraint { get; init; }
    }
}
