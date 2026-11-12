using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>The single production Idea prompt set; missing overrides never fall back.</summary>
internal static class IdeaPrompts
{
    public static string GeneratorSystem() => PromptLoader.Load("Idea/Idea.system.prompt.md");

    public static string GeneratorUser(string sourceContent) =>
        PromptLoader.Render("Idea/Idea.user.prompt.md", ("SourceContent", sourceContent));

    public static string JudgeSystem() => PromptLoader.Load("Idea/IdeaValidation.system.prompt.md");
}