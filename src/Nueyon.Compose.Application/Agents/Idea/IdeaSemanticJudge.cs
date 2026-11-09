using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>Checks generated ideas against the unchanged source using a separate model call.</summary>
public sealed class IdeaSemanticJudge(AIAgent agent)
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));

    private const int MaxBlockingIssues = 3;

    public static string GetSystemInstructions() => PromptLoader.Load("Idea/IdeaValidation.system.prompt.txt");

    internal async Task<IReadOnlyList<BlockingIssue>> FindBlockingIssuesAsync(
        string sourceContent,
        Domain.Idea selectedIdea,
        CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Serialize(new { source = sourceContent, idea = selectedIdea }, _jsonOptions);
        var options = new ChatClientAgentRunOptions(new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<Verdict>(_jsonOptions, nameof(Verdict))
        });
        var response = await _agent.RunAsync(message, null, options, cancellationToken);

        Verdict? verdict;
        try
        {
            verdict = JsonSerializer.Deserialize<Verdict>(response.Text, _jsonOptions);
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

        foreach (var issue in issues)
        {
            if (issue is null || string.IsNullOrWhiteSpace(issue.Problem) ||
                string.IsNullOrWhiteSpace(issue.Correction))
            {
                throw new InvalidOperationException(
                    "Idea semantic validation returned an invalid verdict: each blocker needs a problem and correction.");
            }
        }

        return issues.ToArray();
    }

    private sealed record Verdict
    {
        public required IReadOnlyList<BlockingIssue> BlockingIssues { get; init; }
    }

    internal sealed record BlockingIssue
    {
        public required string Problem { get; init; }
        public required string Correction { get; init; }
    }
}