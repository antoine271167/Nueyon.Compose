using Nueyon.Compose.Application.Prompts;

namespace Nueyon.Compose.Application.Agents.Synthesis;

/// <summary>
///     Contains the system instructions (prompt) for the agent.
/// </summary>
internal static class SynthesizerAgentInstructions
{
    /// <summary>
    ///     Gets the system instructions used to configure the underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() =>
        PromptLoader.Load("Synthesis/Synthesis.system.prompt.txt");
}
