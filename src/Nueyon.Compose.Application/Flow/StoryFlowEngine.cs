using Nueyon.Compose.Application.Harness;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Flow;

/// <summary>
/// The flow engine that orchestrates the IDEA flow.
/// Coordinates the execution of the flow by delegating to the harness.
/// </summary>
public sealed class StoryFlowEngine : IFlowEngine
{
    private readonly IAgentHarness<ChatInput, IReadOnlyList<Idea>> _ideaHarness;

    /// <summary>
    /// Initializes a new instance of the StoryFlowEngine with the specified harness.
    /// </summary>
    /// <param name="ideaHarness">The harness for executing the IDEA flow.</param>
    /// <exception cref="ArgumentNullException">Thrown when ideaHarness is null.</exception>
    public StoryFlowEngine(IAgentHarness<ChatInput, IReadOnlyList<Idea>> ideaHarness) => 
        _ideaHarness = ideaHarness ?? throw new ArgumentNullException(nameof(ideaHarness));

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

        var ideas = await _ideaHarness.ExecuteAsync(
            workspace.Input,
            cancellationToken);

        workspace.Ideas = ideas;

        return workspace;
    }
}
