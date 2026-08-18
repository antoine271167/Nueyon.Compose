using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console;

/// <summary>
///     The interactive console application for Nueyon.Compose.
///     Orchestrates user input, flow execution, and result presentation.
/// </summary>
public sealed class ConsoleApplication
{
    /// <summary>
    ///     Initializes a new instance of the ConsoleApplication.
    /// </summary>
    /// <param name="flowEngine">The flow engine to execute the Compose flow.</param>
    /// <param name="logger">The logger for diagnostics.</param>
    /// <param name="console">The console interface for input/output.</param>
    /// <exception cref="ArgumentNullException">Thrown when flowEngine, logger, or console is null.</exception>
    public ConsoleApplication(IFlowEngine flowEngine, ILogger<ConsoleApplication> logger, IConsole console)
    {
        _flowEngine = flowEngine ?? throw new ArgumentNullException(nameof(flowEngine));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    private readonly IConsole _console;
    private readonly IFlowEngine _flowEngine;
    private readonly ILogger<ConsoleApplication> _logger;

    /// <summary>
    ///     Runs the interactive console application.
    ///     Displays a welcome message, accepts user input, executes the flow, and displays results.
    ///     Continues until the user enters /exit.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The exit code (0 for success, 1 for error, 130 for cancellation).</returns>
    public async Task<int> RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            DisplayWelcomeMessage();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var userInput = ReadInput();

                    if (userInput is null)
                    {
                        // User requested exit
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        _console.WriteLine("Please enter an idea or topic.");
                        _console.WriteLine("");
                        continue;
                    }

                    await ExecuteFlowAsync(userInput, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Operation cancelled by user.");
                    _console.WriteLine("");
                    _console.WriteLine("Operation cancelled.");
                    return 130;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the request.");
                    _console.WriteLine("");
                    _console.WriteLine("Unable to process the request.");
                    _console.WriteLine("Please try again.");
                    _console.WriteLine("");
                }
            }

            DisplayGoodbyeMessage();
            return 0;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Application cancelled.");
            return 130;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            _console.WriteLine("An unexpected error occurred. Please try again.");
            return 1;
        }
    }

    /// <summary>
    ///     Displays the welcome message at application startup.
    /// </summary>
    private void DisplayWelcomeMessage()
    {
        _console.WriteLine("");
        _console.WriteLine("========================================");
        _console.WriteLine("          Nueyon.Compose");
        _console.WriteLine("       AI Idea Composition");
        _console.WriteLine("========================================");
        _console.WriteLine("");
        _console.WriteLine("Enter an idea or topic.");
        _console.WriteLine("Type '/exit' to quit.");
        _console.WriteLine("");
    }

    /// <summary>
    ///     Displays the goodbye message when exiting.
    /// </summary>
    private void DisplayGoodbyeMessage()
    {
        _console.WriteLine("");
        _console.WriteLine("Goodbye.");
    }

    /// <summary>
    ///     Reads a line of input from the console.
    ///     Returns null if the user entered the exit command.
    /// </summary>
    /// <returns>The user input, or null if exit was requested.</returns>
    private string? ReadInput()
    {
        _console.Write("> ");
        var input = _console.ReadLine();

        if (input is not null && input.Trim().Equals("/exit", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input;
    }

    /// <summary>
    ///     Executes the Compose flow with the provided user input.
    /// </summary>
    /// <param name="userInput">The user's idea or topic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task ExecuteFlowAsync(string userInput, CancellationToken cancellationToken)
    {
        _console.WriteLine("");
        _console.WriteLine("Processing...");
        _console.WriteLine("");

        var chatInput = new ChatInput { Content = userInput };
        var workspace = new StoryWorkspace { Input = chatInput };

        var result = await _flowEngine.ExecuteAsync(workspace, cancellationToken);

        DisplayResult(result);
    }

    /// <summary>
    ///     Displays the result of the Compose flow execution.
    /// </summary>
    /// <param name="workspace">The workspace containing the generated ideas.</param>
    private void DisplayResult(StoryWorkspace workspace)
    {
        if (workspace.Ideas.Count == 0)
        {
            _console.WriteLine("No ideas were generated.");
            _console.WriteLine("");
            return;
        }

        _console.WriteLine("Ideas");
        _console.WriteLine("-----");
        _console.WriteLine("");

        for (var i = 0; i < workspace.Ideas.Count; i++)
        {
            var idea = workspace.Ideas[i];
            _console.WriteLine($"{i + 1}. {idea.Title}");
            _console.WriteLine($"   {idea.Description}");
            _console.WriteLine("");
        }
    }
}