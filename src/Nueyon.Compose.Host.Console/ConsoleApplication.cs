using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console;

/// <summary>
/// The interactive console application for Nueyon.Compose.
/// Orchestrates user input, flow execution, and result presentation.
/// </summary>
public sealed class ConsoleApplication
{
    private readonly IFlowEngine _flowEngine;
    private readonly ILogger<ConsoleApplication> _logger;

    /// <summary>
    /// Initializes a new instance of the ConsoleApplication.
    /// </summary>
    /// <param name="flowEngine">The flow engine to execute the Compose flow.</param>
    /// <param name="logger">The logger for diagnostics.</param>
    /// <exception cref="ArgumentNullException">Thrown when flowEngine or logger is null.</exception>
    public ConsoleApplication(IFlowEngine flowEngine, ILogger<ConsoleApplication> logger)
    {
        _flowEngine = flowEngine ?? throw new ArgumentNullException(nameof(flowEngine));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Runs the interactive console application.
    /// Displays a welcome message, accepts user input, executes the flow, and displays results.
    /// Continues until the user enters /exit.
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
                        System.Console.WriteLine("Please enter an idea or topic.");
                        System.Console.WriteLine();
                        continue;
                    }

                    await ExecuteFlowAsync(userInput, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Operation cancelled by user.");
                    System.Console.WriteLine();
                    System.Console.WriteLine("Operation cancelled.");
                    return 130;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the request.");
                    System.Console.WriteLine();
                    System.Console.WriteLine("Unable to process the request.");
                    System.Console.WriteLine("Please try again.");
                    System.Console.WriteLine();
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
            System.Console.Error.WriteLine("An unexpected error occurred. Please try again.");
            return 1;
        }
    }

    /// <summary>
    /// Displays the welcome message at application startup.
    /// </summary>
    private static void DisplayWelcomeMessage()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("========================================");
        System.Console.WriteLine("          Nueyon.Compose");
        System.Console.WriteLine("       AI Idea Composition");
        System.Console.WriteLine("========================================");
        System.Console.WriteLine();
        System.Console.WriteLine("Enter an idea or topic.");
        System.Console.WriteLine("Type '/exit' to quit.");
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays the goodbye message when exiting.
    /// </summary>
    private static void DisplayGoodbyeMessage()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Goodbye.");
    }

    /// <summary>
    /// Reads a line of input from the console.
    /// Returns null if the user entered the exit command.
    /// </summary>
    /// <returns>The user input, or null if exit was requested.</returns>
    private static string? ReadInput()
    {
        System.Console.Write("> ");
        var input = System.Console.ReadLine();

        if (input is not null && input.Trim().Equals("/exit", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return input;
    }

    /// <summary>
    /// Executes the Compose flow with the provided user input.
    /// </summary>
    /// <param name="userInput">The user's idea or topic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task ExecuteFlowAsync(string userInput, CancellationToken cancellationToken)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Processing...");
        System.Console.WriteLine();

        try
        {
            var chatInput = new ChatInput { Content = userInput };
            var workspace = new StoryWorkspace { Input = chatInput };

            var result = await _flowEngine.ExecuteAsync(workspace, cancellationToken);

            DisplayResult(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Displays the result of the Compose flow execution.
    /// </summary>
    /// <param name="workspace">The workspace containing the generated ideas.</param>
    private static void DisplayResult(StoryWorkspace workspace)
    {
        if (workspace.Ideas is null || workspace.Ideas.Count == 0)
        {
            System.Console.WriteLine("No ideas were generated.");
            System.Console.WriteLine();
            return;
        }

        System.Console.WriteLine("Ideas");
        System.Console.WriteLine("-----");
        System.Console.WriteLine();

        for (int i = 0; i < workspace.Ideas.Count; i++)
        {
            var idea = workspace.Ideas[i];
            System.Console.WriteLine($"{i + 1}. {idea.Title}");
            System.Console.WriteLine($"   {idea.Description}");
            System.Console.WriteLine();
        }
    }
}
