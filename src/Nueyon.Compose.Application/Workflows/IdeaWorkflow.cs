using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Workflows;

/// <summary>
///     A MAF Workflow that generates content ideas from user input using the Idea Agent.
///     The workflow contains a single executor (via FunctionExecutor) that transforms ChatInput
///     into a list of Idea objects. This is the minimal workflow designed for learning
///     and understanding the MAF Workflow model.
/// </summary>
public sealed class IdeaWorkflow
{
    /// <summary>
    ///     Initializes a new instance of the IdeaWorkflow with the specified executor.
    /// </summary>
    /// <param name="ideaExecutor">The executor to run in the workflow.</param>
    /// <exception cref="ArgumentNullException">Thrown when ideaExecutor is null.</exception>
    public IdeaWorkflow(FunctionExecutor<ChatInput, IReadOnlyList<Idea>> ideaExecutor) =>
        _ideaExecutor = ideaExecutor ?? throw new ArgumentNullException(nameof(ideaExecutor));

    private readonly FunctionExecutor<ChatInput, IReadOnlyList<Idea>> _ideaExecutor;
    private Workflow? _workflow;

    /// <summary>
    ///     Builds and returns the underlying MAF Workflow instance.
    ///     Constructs a single-executor workflow with IdeaExecutor as both the entry point and output.
    /// </summary>
    /// <returns>The constructed MAF Workflow.</returns>
    public Workflow Build()
    {
        if (_workflow != null)
        {
            return _workflow;
        }

        // Create a workflow builder starting with the idea executor
        var builder = new WorkflowBuilder(_ideaExecutor);

        // Build the workflow.
        // The executor is configured as both the entry point and the output source.
        _workflow = builder.Build();

        return _workflow;
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

        // Run the workflow with the input using the static InProcessExecution API
        var run = await InProcessExecution.RunAsync(
            workflow,
            input,
            cancellationToken: cancellationToken);

        // Extract the output from the emitted events
        var result = ExtractResult(run);

        return result;
    }

    /// <summary>
    ///     Extracts the workflow output from the emitted events.
    /// </summary>
    private static IReadOnlyList<Idea> ExtractResult(Run run)
    {
        // Find the output event containing ideas
        foreach (var @event in run.OutgoingEvents)
        {
            // WorkflowOutputEvent represents output yielded by executors
            // Check if this is an IReadOnlyList<Idea>
            if (@event is WorkflowOutputEvent { Data: IReadOnlyList<Idea> ideas })
            {
                return ideas;
            }
        }

        // If no output was found, return an empty list
        return [];
    }
}