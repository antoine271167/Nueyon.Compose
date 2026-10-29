using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Narrative;

/// <summary>
///     Contains the system instructions (prompt) for the agent.
/// </summary>
internal static class NarrativeAgentInstructions
{
    /// <summary>
    ///     Gets the system instructions used to configure the underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() =>
        PromptLoader.Load("Narrative/Narrative.system.prompt.txt");
}
