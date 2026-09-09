using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

public sealed record StoryWorkflowResult(
    StoryInput Input,
    SelectedIdea SelectedIdea,
    ResearchResult Research,
    SynthesisResult Synthesis,
    NarrativeResult Narrative,
    ComposeResult Compose);
