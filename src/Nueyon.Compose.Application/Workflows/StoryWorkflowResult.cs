using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public sealed record StoryWorkflowResult(
    ChatInput Input,
    SelectedIdea SelectedIdea,
    ResearchResult Research,
    SynthesisResult Synthesis,
    NarrativeResult Narrative,
    ComposeResult Compose);
