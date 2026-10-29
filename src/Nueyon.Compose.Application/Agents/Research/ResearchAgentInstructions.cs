using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Research;

/// <summary>
///     Contains the system instructions (prompt) for the agent.
/// </summary>
internal static class ResearchAgentInstructions
{
    /// <summary>
    ///     Gets the system instructions used to configure the underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() =>
        PromptLoader.Load("Research/Research.system.prompt.txt");
}
