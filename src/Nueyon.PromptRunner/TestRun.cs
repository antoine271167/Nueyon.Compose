using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Validation;

#pragma warning disable MAAI001

namespace Nueyon.PromptRunner;

internal sealed class TestRun(string output, string apiKey) : LoopEvaluator
{
    private readonly IdeaValidationLoopEvaluator _validator = new(new IdeaValidator());

    public async Task ExecuteAsync(AIAgent agent, string user, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(output);
        var loop = new LoopAgent(agent, this, new LoopAgentOptions { MaxIterations = 3 });
        var options = new ChatClientAgentRunOptions(new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<IdeaResponse>(null, nameof(IdeaResponse))
        });

        var response = await loop.RunAsync(user, null, options, cancellationToken);
        var ideas = JsonSerializer.Deserialize<IdeaResponse>(response.Text,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })?.Ideas
            ?? throw new InvalidOperationException("Response ideas property is null.");

        if (ideas.Count == 0)
            throw new InvalidOperationException("Cannot select an idea because no ideas were generated.");

        Save("raw-output.json", response.Text);
        Save("selected-idea.json", JsonSerializer.Serialize(ideas[0], new JsonSerializerOptions { WriteIndented = true }));
    }

    public override async ValueTask<LoopEvaluation> EvaluateAsync(
        LoopContext context, CancellationToken cancellationToken = default)
    {
        var prefix = $"iteration-{context.Iteration}";
        Save($"{prefix}.raw.txt", context.LastResponse.Text);
        Save($"{prefix}.usage.json", JsonSerializer.Serialize(context.LastResponse.Usage));
        return await _validator.EvaluateAsync(context, cancellationToken);
    }

    private void Save(string name, string content) =>
        File.WriteAllText(Path.Combine(output, name), content.Replace(apiKey, "[REDACTED]", StringComparison.Ordinal));
}
