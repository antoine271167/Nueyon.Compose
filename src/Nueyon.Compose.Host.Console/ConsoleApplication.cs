using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Services;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console;

/// <summary>
///     The console application for Nueyon.Compose.
///     Loads source material from Markdown files, executes the workflow, and displays results.
/// </summary>
public sealed class ConsoleApplication
{
    /// <summary>
    ///     Initializes a new instance of the ConsoleApplication.
    /// </summary>
    /// <param name="sourceContextLoader">The loader for source material from files.</param>
    /// <param name="storyWorkflow">The application-level workflow to generate content.</param>
    /// <param name="logger">The logger for diagnostics.</param>
    /// <param name="console">The console interface for output.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public ConsoleApplication(
        ISourceContextLoader sourceContextLoader,
        IStoryWorkflow storyWorkflow,
        ILogger<ConsoleApplication> logger,
        IConsole console)
    {
        _sourceContextLoader = sourceContextLoader ?? throw new ArgumentNullException(nameof(sourceContextLoader));
        _storyWorkflow = storyWorkflow ?? throw new ArgumentNullException(nameof(storyWorkflow));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    private readonly IConsole _console;
    private readonly ILogger<ConsoleApplication> _logger;

    private readonly ISourceContextLoader _sourceContextLoader;
    private readonly IStoryWorkflow _storyWorkflow;

    /// <summary>
    ///     Runs the console application.
    ///     Loads Markdown source material from the data/input directory,
    ///     executes the workflow, and displays results.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="inputDirectory">The directory to load Markdown files from.</param>
    /// <param name="errorsToStandardError">Whether to write errors to standard error.</param>
    /// <param name="describeError">A function to describe an exception.</param>
    /// <param name="inputErrorExitCode">The exit code to return for input errors.</param>
    /// <returns>The exit code (0 for success, 1 for error, 130 for cancellation).</returns>
    public async Task<int> RunAsync(CancellationToken cancellationToken = default, string inputDirectory = "data/input",
        bool errorsToStandardError = false, Func<Exception, string>? describeError = null,
        int? inputErrorExitCode = null)
    {
        try
        {
            DisplayWelcomeMessage();

            StoryInput input;
            try
            {
                _console.WriteLine($"Loading Markdown files from: {inputDirectory}");
                _console.WriteLine("");
                input = await _sourceContextLoader.LoadAsync(inputDirectory, cancellationToken);
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError(ex, "Input directory not found.");
                _console.WriteLine("");
                WriteError($"Error: {ex.Message}", errorsToStandardError);
                return inputErrorExitCode ?? (errorsToStandardError ? 2 : 1);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "No Markdown files found in input directory.");
                _console.WriteLine("");
                WriteError($"Error: {ex.Message}", errorsToStandardError);
                return inputErrorExitCode ?? (errorsToStandardError ? 2 : 1);
            }

            await ExecuteFlowAsync(input, cancellationToken);

            return 0;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Application cancelled.");
            _console.WriteLine("");
            WriteError("Operation cancelled.", errorsToStandardError);
            return 130;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            _console.WriteLine("");
            WriteError(describeError?.Invoke(ex) ?? "An unexpected error occurred. Please try again.",
                errorsToStandardError);
            return 1;
        }
    }

    private void WriteError(string message, bool errorsToStandardError)
    {
        if (errorsToStandardError)
        {
            System.Console.Error.WriteLine(message);
        }
        else
        {
            _console.WriteLine(message);
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
    }

    private async Task ExecuteFlowAsync(
        StoryInput input,
        CancellationToken cancellationToken)
    {
        _console.WriteLine("Processing...");
        _console.WriteLine("");

        var result = await _storyWorkflow.RunAsync(input, cancellationToken);

        // An interrupted run must not present its result as a success.
        cancellationToken.ThrowIfCancellationRequested();
        DisplayResult(result);
    }

    private void DisplayResult(StoryWorkflowResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var idea = result.SelectedIdea.Idea;

        _console.WriteLine("Selected Idea");
        _console.WriteLine("-------------");
        _console.WriteLine("");
        _console.WriteLine(idea.Title);
        _console.WriteLine(idea.Description);
        _console.WriteLine("");

        _console.WriteLine("Research");
        _console.WriteLine("--------");
        _console.WriteLine("");
        _console.WriteLine(result.Research.Content);
        _console.WriteLine("");

        _console.WriteLine("Synthesis");
        _console.WriteLine("---------");
        _console.WriteLine("");
        _console.WriteLine(result.Synthesis.Content);
        _console.WriteLine("");

        _console.WriteLine("Narrative");
        _console.WriteLine("---------");
        _console.WriteLine("");
        _console.WriteLine(result.Narrative.Content);
        _console.WriteLine("");

        _console.WriteLine("Compose");
        _console.WriteLine("-------");
        _console.WriteLine("");
        _console.WriteLine(result.Compose.Content);
        _console.WriteLine("");
    }
}