using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Idea;

/// <summary>
///     Contains the system instructions (prompt) for the agent.
/// </summary>
internal static class IdeaAgentInstructions
{
    /// <summary>
    ///     Gets the system instructions used to configure the underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() =>
        PromptLoader.Load("Idea/Idea.system.prompt.txt");
}
