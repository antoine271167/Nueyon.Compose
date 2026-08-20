using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     A MAF Workflow that generates content ideas from user input using the Idea Agent.
///     The workflow contains a single executor (via FunctionExecutor) that transforms ChatInput
///     into a list of Idea objects. This is the minimal workflow designed for learning
///     and understanding the MAF Workflow model.
/// </summary>
public sealed class StoryWorkflow
{
    /// <summary>
    ///     Initializes a new instance of the StoryWorkflow with the specified executor.
    /// </summary>
    /// <param name="ideaExecutor">The executor to run in the workflow.</param>
    /// <exception cref="ArgumentNullException">Thrown when ideaExecutor is null.</exception>
    public StoryWorkflow(FunctionExecutor<ChatInput, Idea[]> ideaExecutor) =>
        _ideaExecutor = ideaExecutor ?? throw new ArgumentNullException(nameof(ideaExecutor));

    private readonly FunctionExecutor<ChatInput, Idea[]> _ideaExecutor;

    /// <summary>
    ///     Builds and returns a new MAF Workflow instance.
    ///     Constructs a single-executor workflow with IdeaExecutor as both the entry point and output.
    ///     Each invocation creates a fresh workflow to support multiple independent executions.
    /// </summary>
    /// <returns>A newly constructed MAF Workflow.</returns>
    public Workflow Build()
    {
        var builder = new WorkflowBuilder(_ideaExecutor);
        return builder.Build();
    }

    /// <summary>
    ///     Executes the workflow with the provided input.
    /// </summary>
    /// <param name="input">The chat input to process.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the workflow execution containing the generated ideas.</returns>
    /// <exception cref="ArgumentNullException">Thrown when input is null.</exception>
    public async Task<IReadOnlyList<Idea>> RunAsync(
        ChatInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        var workflow = Build();

        var run = await InProcessExecution.RunAsync(
            workflow,
            input,
            cancellationToken: cancellationToken);

        return ExtractResult(run);
    }

    private static Idea[] ExtractResult(Run run)
    {
        foreach (var @event in run.OutgoingEvents)
        {
            if (@event is ExecutorCompletedEvent { ExecutorId: "idea", Data: Idea[] ideas })
            {
                return ideas;
            }
        }

        throw new InvalidOperationException(
            "The Idea Workflow completed without producing an Idea result.");
    }
}