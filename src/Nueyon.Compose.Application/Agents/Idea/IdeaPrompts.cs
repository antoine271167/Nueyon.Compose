using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>Maps an Idea workflow mode to its complete prompt set; a missing file is never replaced.</summary>
internal static class IdeaPrompts
{
    public static string GeneratorSystemPath(IdeaWorkflowMode mode) => Path(mode, "Idea.system.prompt.txt");
    public static string GeneratorUserPath(IdeaWorkflowMode mode) => Path(mode, "Idea.user.prompt.txt");
    public static string JudgeSystemPath(IdeaWorkflowMode mode) => Path(mode, "IdeaValidation.system.prompt.txt");

    public static string GeneratorSystem(IdeaWorkflowMode mode) => PromptLoader.Load(GeneratorSystemPath(mode));

    public static string GeneratorUser(IdeaWorkflowMode mode, string sourceContent) =>
        PromptLoader.Render(GeneratorUserPath(mode), ("SourceContent", sourceContent));

    public static string JudgeSystem(IdeaWorkflowMode mode) => PromptLoader.Load(JudgeSystemPath(mode));

    private static string Path(IdeaWorkflowMode mode, string file) => mode switch
    {
        IdeaWorkflowMode.Legacy => $"Idea/{file}",
        IdeaWorkflowMode.Simplified => $"Idea/Simplified/{file}",
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown Idea workflow mode.")
    };
}
