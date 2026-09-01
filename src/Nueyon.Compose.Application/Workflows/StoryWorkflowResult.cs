using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public sealed class StoryWorkflowResult
{
    public required ChatInput Input { get; init; }
    public required SelectedIdea SelectedIdea { get; init; }
    public required ResearchResult Research { get; init; }
    public required SynthesisResult Synthesis { get; init; }
}