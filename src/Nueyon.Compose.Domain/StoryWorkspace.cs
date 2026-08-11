namespace Nueyon.Compose.Domain;

public sealed class StoryWorkspace
{
    public required ChatInput Input { get; init; }

    public IReadOnlyList<Idea> Ideas { get; set; } = [];
}
