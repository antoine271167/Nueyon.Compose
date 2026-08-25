using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Research;

public sealed class ResearchInput
{
    public required ChatInput Input { get; init; }

    public required SelectedIdea SelectedIdea { get; init; }
}