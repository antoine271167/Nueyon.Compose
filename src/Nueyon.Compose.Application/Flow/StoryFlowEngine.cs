using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Flow;

/// <summary>
/// The flow engine that orchestrates the IDEA flow.
/// Delegates to the Idea Agent for generating content ideas from user input.
/// </summary>
public sealed class StoryFlowEngine : IFlowEngine
{
    private readonly IAgent<ChatInput, IReadOnlyList<Idea>> _ideaAgent;

    /// <summary>
    /// Initializes a new instance of the StoryFlowEngine with the specified agent.
    /// </summary>
    /// <param name="ideaAgent">The agent for executing the IDEA flow (includes validation and retry via LoopAgent).</param>
    /// <exception cref="ArgumentNullException">Thrown when ideaAgent is null.</exception>
    public StoryFlowEngine(IAgent<ChatInput, IReadOnlyList<Idea>> ideaAgent) => 
        _ideaAgent = ideaAgent ?? throw new ArgumentNullException(nameof(ideaAgent));

    /// <summary>
    /// Executes the IDEA flow on the provided workspace.
    /// </summary>
    /// <param name="workspace">The workspace to process. Its Input will be passed to the flow.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The workspace with Ideas populated by the flow execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when workspace is null.</exception>
    public async Task<StoryWorkspace> ExecuteAsync(
        StoryWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workspace);

        var ideas = await _ideaAgent.ExecuteAsync(
            workspace.Input,
            cancellationToken);

        workspace.Ideas = ideas;

        return workspace;
    }
}
