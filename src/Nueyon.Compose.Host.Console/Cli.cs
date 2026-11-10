using System.ClientModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Application.Services;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Infrastructure;
using Nueyon.Compose.Infrastructure.Options;

namespace Nueyon.Compose.Host.Console;

public static class Cli
{
    private static readonly JsonSerializerOptions _print = new(CliContracts.OutputOptions)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static Task<int> RunAsync(string[] args) => RunAsync(args, null);

    // The factory lets tests supply offline services to the otherwise unchanged runtime path.
    internal static async Task<int> RunAsync(string[] args,
        Func<string?, StageTracker, ServiceProvider>? createServices)
    {
        // No arguments keep the original behavior: run the workflow, with input errors reported as exit code 1.
        var legacy = args.Length == 0;
        try
        {
            var parsed = Parse(args);
            if (parsed.Help)
            {
                PrintHelp(parsed.Command);
                return 0;
            }

            switch (parsed.Command!.Kind)
            {
                case CliCommandKind.AgentsList:
                    foreach (var agent in CliContracts.Agents)
                    {
                        System.Console.WriteLine($"{agent.Name}\t{agent.Description}");
                    }

                    return 0;
                case CliCommandKind.AgentsDescribe:
                    var description = CliContracts.DescribeAgent(parsed.Agent!).ToJsonString(_print);
                    System.Console.WriteLine(parsed.Options.GetValueOrDefault("--format") == "json"
                        ? description
                        : $"{parsed.Agent!.Name}: {parsed.Agent.Description}\n" +
                          $"Input: {parsed.Agent.InputType.Name}; output: {parsed.Agent.OutputType.Name}\n{description}");
                    return 0;
                case CliCommandKind.AgentsSchema:
                    System.Console.WriteLine(parsed.Agent!.InputSchema().ToJsonString(_print));
                    return 0;
                case CliCommandKind.Describe:
                    System.Console.WriteLine(CliContracts.DescribeAll().ToJsonString(_print));
                    return 0;
                default:
                    return await RunRuntimeAsync(parsed, legacy, createServices ?? CreateServices);
            }
        }
        catch (UsageException ex)
        {
            await System.Console.Error.WriteLineAsync($"Input error: {ex.Message}");
            return legacy ? 1 : 2;
        }
        catch (OperationCanceledException)
        {
            await System.Console.Error.WriteLineAsync("Cancelled.");
            return 130;
        }
        catch (ConfigurationException ex)
        {
            await System.Console.Error.WriteLineAsync($"Configuration error: {ex.Message}");
            return 1;
        }
        catch (Exception ex)
        {
            await System.Console.Error.WriteLineAsync(Diagnose(ex, null));
            return 1;
        }
    }

    private static Parsed Parse(string[] args)
    {
        var workflow = CliContracts.Commands.First(c => c.Syntax == CliContracts.NoArguments.Command);
        if (args.Length == 0)
        {
            return new Parsed(workflow, null, [], false);
        }

        if (args is ["--help"])
        {
            return new Parsed(null, null, [], true);
        }

        var command = CliContracts.Commands
            .Where(c => args.Length >= c.Path.Length && c.Path.SequenceEqual(args.Take(c.Path.Length)))
            .OrderByDescending(c => c.Path.Length).FirstOrDefault();
        if (command is null)
        {
            // A bare group such as "agents --help".
            if (args is [var group, "--help"] && CliContracts.Commands.Any(c => c.Path[0] == group))
            {
                return new Parsed(null, null, [], true);
            }

            throw new UsageException("Unknown command. Use --help.");
        }

        var index = command.Path.Length;
        AgentContract? agent = null;
        if (command.TakesAgent && index < args.Length && !args[index].StartsWith("--", StringComparison.Ordinal))
        {
            agent = CliContracts.FindAgent(args[index++]) ?? throw new UsageException("Unknown agent.");
        }

        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        var help = false;
        while (index < args.Length)
        {
            var option = args[index++];
            if (option == CliContracts.Help.Name)
            {
                if (help)
                {
                    throw new UsageException("Duplicate --help.");
                }

                help = true;
                continue;
            }

            var spec = command.Options.FirstOrDefault(o => o.Name == option)
                       ?? throw new UsageException($"Unknown option or argument: {option}");
            if (!spec.AppliesTo(agent))
            {
                throw new UsageException($"{spec.Name} is available only for: {string.Join(", ", spec.Agents!)}.");
            }

            if (index >= args.Length || args[index].StartsWith("--", StringComparison.Ordinal) ||
                string.IsNullOrWhiteSpace(args[index]))
            {
                throw new UsageException($"Missing value for {spec.Name}.");
            }

            var value = args[index++];
            if (spec.Formats is not null && !spec.Formats.Contains(value, StringComparer.Ordinal))
            {
                throw new UsageException($"Invalid value for {spec.Name}; expected {string.Join("|", spec.Formats)}.");
            }

            if (!options.TryAdd(option, value))
            {
                throw new UsageException($"Duplicate option: {option}.");
            }
        }

        if (!help)
        {
            if (command.TakesAgent && agent is null)
            {
                throw new UsageException("An agent is required.");
            }

            foreach (var option in command.Options.Where(o => o.Required && o.AppliesTo(agent)))
            {
                if (!options.ContainsKey(option.Name))
                {
                    throw new UsageException($"Missing {option.Name}.");
                }
            }
        }

        return new Parsed(command, agent, options, help);
    }

    private static void PrintHelp(CliCommand? command)
    {
        if (command is null)
        {
            System.Console.WriteLine($"Nuëyon.Compose: {CliContracts.NoArguments.Description}\nCommands:");
            foreach (var item in CliContracts.Commands)
            {
                System.Console.WriteLine($"  {item.Syntax,-26} {item.Description}");
                foreach (var option in item.Options)
                {
                    System.Console.WriteLine(
                        $"      {option.Name} <{option.ValueType}>{(option.Required ? " (required)" : "")}");
                }

                foreach (var example in item.Examples)
                {
                    System.Console.WriteLine($"      Example: {example}");
                }
            }

            System.Console.WriteLine("Use <command> --help for option details. Agent names are case-insensitive.");
            PrintExitCodes();
            return;
        }

        System.Console.WriteLine($"{command.Syntax}\n{command.Description}");
        foreach (var option in command.Options)
        {
            var scope = option.Agents is null ? "" : $" [{string.Join(", ", option.Agents)} only]";
            System.Console.WriteLine(
                $"  {option.Name} <{option.ValueType}> {(option.Required ? "(required)" : "(optional)")}  {option.Description}{scope}");
        }

        System.Console.WriteLine($"  {CliContracts.Help.Name}  {CliContracts.Help.Description}");
        foreach (var example in command.Examples)
        {
            System.Console.WriteLine($"Example: {example}");
        }

        PrintExitCodes();
    }

    private static void PrintExitCodes() =>
        System.Console.WriteLine("Exit codes: " +
                                 string.Join(", ", CliContracts.ExitCodes.Select(c => $"{c.Code} {c.Meaning}")) + ".");

    private static async Task<int> RunRuntimeAsync(Parsed parsed, bool legacy,
        Func<string?, StageTracker, ServiceProvider> createServices)
    {
        var tracker = new StageTracker();
        using var cts = new CancellationTokenSource();
        ConsoleCancelEventHandler handler = (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        // Installed before any asynchronous read so Ctrl+C is observed from the start.
        System.Console.CancelKeyPress += handler;
        try
        {
            object? input = null;
            string? output = null;
            string? trace = null;
            var sourceDirectory =
                parsed.Options.GetValueOrDefault("--input-directory", CliContracts.DefaultInputDirectory);
            try
            {
                if (parsed.Agent is not null)
                {
                    output = ValidateNewFile(parsed.Options["--output"]);
                    if (parsed.Options.TryGetValue("--trace", out var tracePath))
                    {
                        trace = ValidateNewFile(tracePath);
                        if (string.Equals(output, trace,
                                OperatingSystem.IsWindows()
                                    ? StringComparison.OrdinalIgnoreCase
                                    : StringComparison.Ordinal))
                        {
                            throw new UsageException("Output and trace paths must differ.");
                        }
                    }

                    input = parsed.Agent.ReadInput(await File.ReadAllTextAsync(parsed.Options["--input"], cts.Token));
                }
                else if (!legacy && (!Directory.Exists(sourceDirectory) ||
                                     !Directory.EnumerateFiles(sourceDirectory, "*.md").Any()))
                {
                    throw new UsageException("The input directory must contain Markdown files.");
                }
            }
            catch (UsageException)
            {
                throw;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException
                                           or ArgumentException or NotSupportedException)
            {
                throw new UsageException($"Cannot read or validate input/path: {ex.Message}");
            }

            var promptPath = ResolvePrompts(parsed.Options.GetValueOrDefault("--prompts"));
            await System.Console.Error.WriteLineAsync(DescribePrompts(promptPath));
            using var promptScope = promptPath is null ? null : PromptDirectory.Use(promptPath);
            string? capture = null;
            if (parsed.Options.TryGetValue("--capture-inputs", out var captureRoot))
            {
                capture = CreateCaptureDirectory(captureRoot);
                await System.Console.Error.WriteLineAsync($"Capturing agent inputs in: {capture}");
            }

            var attempts = new List<IdeaValidationAttempt>();
            var published = false;
            try
            {
                await using var services = createServices(capture, tracker);
                if (parsed.Agent is null)
                {
                    return await services.GetRequiredService<ConsoleApplication>().RunAsync(cts.Token, sourceDirectory,
                        true, ex => Diagnose(ex, tracker),
                        legacy ? CliContracts.NoArguments.InputErrorExitCode : 2);
                }

                // Scoped to this invocation: nothing is registered on the shared evaluator.
                Action<IdeaValidationAttempt>? observer = trace is null
                    ? null
                    : attempt =>
                    {
                        if (attempts.Count > 0 && attempts[^1].Attempt == attempt.Attempt)
                        {
                            attempts[^1] = attempt;
                        }
                        else
                        {
                            attempts.Add(attempt);
                        }
                    };

                tracker.Stage = parsed.Agent.Name;
                var context = new AgentExecutionContext(Guid.NewGuid())
                {
                    AdditionalProperties = observer is null
                        ? null
                        : new Dictionary<string, object?> { [IdeaValidationLoopEvaluator.ObserverKey] = observer }
                };
                var result = await parsed.Agent.ExecuteAsync(services, input!, context, cts.Token);
                // Do not report success when interrupted before the result is persisted.
                cts.Token.ThrowIfCancellationRequested();
                await WriteNewAsync(output!, result, parsed.Agent.OutputType, cts.Token);

                // The published result is the commit point: the requested trace is finished even if cancelled now.
                published = true;
                if (trace is not null)
                {
                    try
                    {
                        await WriteNewAsync(trace, new IdeaTrace("accepted", attempts));
                    }
                    catch (Exception)
                    {
                        await System.Console.Error.WriteLineAsync(
                            "The result was written, but the requested trace could not be written.");
                        return 1;
                    }
                }

                return 0;
            }
            catch (Exception ex) when (ex is not (UsageException or ConfigurationException))
            {
                if (trace is not null && !published)
                {
                    await TryWriteTraceAsync(trace, attempts);
                }

                if (ex is OperationCanceledException && cts.IsCancellationRequested)
                {
                    throw;
                }

                await System.Console.Error.WriteLineAsync(Diagnose(ex, tracker));
                return 1;
            }
        }
        finally
        {
            System.Console.CancelKeyPress -= handler;
        }
    }

    private static async Task TryWriteTraceAsync(string trace, List<IdeaValidationAttempt> attempts)
    {
        var exhausted = attempts.Count > 0 && attempts[^1].Attempt == IdeaValidationLoopEvaluator.MaxAttempts &&
                        attempts[^1].Status == "rejected";
        try
        {
            await WriteNewAsync(trace, new IdeaTrace(exhausted ? "exhausted" : "incomplete", attempts));
        }
        catch (Exception)
        {
            // The original failure or cancellation is what the caller needs to see.
            await System.Console.Error.WriteLineAsync("The trace could not be written.");
        }
    }

    /// <summary>A sanitized description: never the exception message, which can quote payloads or secrets.</summary>
    internal static string Diagnose(Exception ex, StageTracker? tracker)
    {
        var stage = tracker?.Stage is { } name ? $" in stage '{name}'" : "";
        for (var current = ex; current is not null; current = current.InnerException)
        {
            switch (current)
            {
                case ClientResultException client:
                    return
                        $"Runtime error{stage}: the model service request failed (HTTP {client.Status}). No result was produced.";
                case HttpRequestException { StatusCode: { } status }:
                    return
                        $"Runtime error{stage}: a network request failed (HTTP {(int)status}). No result was produced.";
                case HttpRequestException:
                    return $"Runtime error{stage}: a network request failed. No result was produced.";
                case TimeoutException or TaskCanceledException:
                    return $"Runtime error{stage}: a request timed out. No result was produced.";
            }
        }

        return $"Runtime error{stage} ({ex.GetType().Name}). No result was produced.";
    }

    private static string ValidateNewFile(string path)
    {
        var full = Path.GetFullPath(path);
        if (File.Exists(full) || Directory.Exists(full))
        {
            throw new UsageException("Output or trace already exists.");
        }

        if (!Directory.Exists(Path.GetDirectoryName(full)))
        {
            throw new UsageException("Output or trace parent directory does not exist.");
        }

        return full;
    }

    internal static async Task WriteNewAsync(string path, object value, Type? type = null,
        CancellationToken cancellationToken = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, type ?? value.GetType(), CliContracts.OutputOptions);
        var temporary = Path.Combine(Path.GetDirectoryName(path)!, $".{Guid.NewGuid():N}.tmp");
        try
        {
            await using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await stream.WriteAsync(bytes, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            File.Move(temporary, path, false);
        }
        finally
        {
            if (File.Exists(temporary))
            {
                File.Delete(temporary);
            }
        }
    }

    internal static string CreateCaptureDirectory(string root)
    {
        try
        {
            Directory.CreateDirectory(root);
            var capture = Path.Combine(Path.GetFullPath(root), $"run-{Guid.NewGuid():N}");
            Directory.CreateDirectory(capture);
            return capture;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException
                                       or NotSupportedException)
        {
            throw new UsageException($"Cannot create capture directory: {ex.Message}");
        }
    }

    internal static string DescribePrompts(string? promptPath) =>
        promptPath is null
            ? "Prompts: embedded (no disk prompt directory found)."
            : $"Prompts: disk ({promptPath}).";

    internal static string? ResolvePrompts(string? explicitPath, string? workingDirectory = null,
        string? baseDirectory = null)
    {
        var source = Path.Combine(workingDirectory ?? Directory.GetCurrentDirectory(), CliContracts.SourcePromptPath);
        var adjacent = Path.Combine(baseDirectory ?? AppContext.BaseDirectory, "Prompts");
        var path = explicitPath ?? (Directory.Exists(source) ? source : Directory.Exists(adjacent) ? adjacent : null);
        if (path is null)
        {
            return null;
        }

        try
        {
            path = Path.GetFullPath(path);
            const string prefix = "Nueyon.Compose.Application.Prompts.";
            foreach (var resource in typeof(PromptDirectory).Assembly.GetManifestResourceNames()
                         .Where(n => n.StartsWith(prefix, StringComparison.Ordinal)))
            {
                var relative = resource[prefix.Length..];
                var separator = relative.IndexOf('.');
                var file = Path.Combine(path, relative[..separator], relative[(separator + 1)..]);
                if (!File.Exists(file))
                {
                    throw new UsageException($"Prompt file is missing: {file}");
                }
            }

            return path;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException
                                       or NotSupportedException)
        {
            throw new UsageException($"Invalid prompt directory: {ex.Message}");
        }
    }

    private static ServiceProvider CreateServices(string? capture, StageTracker tracker)
    {
        var cwd = Directory.GetCurrentDirectory();
        var configDir = File.Exists(Path.Combine(cwd, "appsettings.json"))
            ? cwd
            : File.Exists(Path.Combine(cwd, "src", "Nueyon.Compose.Host.Console", "appsettings.json"))
                ? Path.Combine(cwd, "src", "Nueyon.Compose.Host.Console")
                : AppContext.BaseDirectory;
        IConfigurationRoot configuration;
        try
        {
            configuration = new ConfigurationBuilder().SetBasePath(configDir)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true, false)
                .AddEnvironmentVariables().AddUserSecrets(typeof(Cli).Assembly, true).Build();
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or FormatException
                                       or UnauthorizedAccessException)
        {
            // JSON parse failures carry a line position but never the offending value.
            var json = ex.InnerException as JsonException;
            var position = json?.LineNumber is { } line
                ? $" (line {line + 1}, position {json.BytePositionInLine + 1})"
                : "";
            throw new ConfigurationException(
                $"Cannot load configuration from '{configDir}'{position}. Check that appsettings*.json is valid JSON.");
        }

        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(configuration.GetSection("OpenAI"));
        services.AddLogging(builder => builder.ClearProviders());
        services.AddInfrastructure();
        services.AddSingleton<ISourceContextLoader, SourceContextLoader>();
        services.AddSingleton<IConsole, SystemConsole>();
        services.AddSingleton<IStoryWorkflow>(provider =>
            CreateWorkflow(provider, new WorkflowDecorator(capture, tracker)));
        services.AddSingleton<ConsoleApplication>();
        var provider = services.BuildServiceProvider();
        try
        {
            provider.GetRequiredService<IOptions<OpenAiOptions>>().Value.Validate();
        }
        catch (InvalidOperationException ex)
        {
            provider.Dispose();
            throw new ConfigurationException(ex.Message);
        }

        return provider;
    }

    internal static IStoryWorkflow CreateWorkflow(IServiceProvider services, IAgentDecorator decorator) =>
        new StoryWorkflow(
            CliContracts.IdeaContract.Resolve(services, decorator),
            CliContracts.ResearchContract.Resolve(services, decorator),
            CliContracts.SynthesisContract.Resolve(services, decorator),
            CliContracts.NarrativeContract.Resolve(services, decorator),
            CliContracts.ComposeContract.Resolve(services, decorator));

    internal sealed class StageTracker
    {
        public volatile string? Stage;
    }

    internal sealed class WorkflowDecorator(string? capture, StageTracker tracker) : IAgentDecorator
    {
        public IAgent<TInput, TOutput> Decorate<TInput, TOutput>(string name, IAgent<TInput, TOutput> agent) =>
            new StageAgent<TInput, TOutput>(
                capture is null
                    ? agent
                    : new CaptureAgent<TInput, TOutput>(agent, Path.Combine(capture, name + ".json")),
                name, tracker);
    }

    internal sealed class StageAgent<TInput, TOutput>(IAgent<TInput, TOutput> inner, string name, StageTracker tracker)
        : IAgent<TInput, TOutput>
    {
        public Task<TOutput> ExecuteAsync(AgentExecutionContext executionContext, TInput input,
            CancellationToken cancellationToken = default)
        {
            tracker.Stage = name;
            return inner.ExecuteAsync(executionContext, input, cancellationToken);
        }
    }

    internal sealed class CaptureAgent<TInput, TOutput>(IAgent<TInput, TOutput> inner, string path)
        : IAgent<TInput, TOutput>
    {
        public async Task<TOutput> ExecuteAsync(AgentExecutionContext executionContext, TInput input,
            CancellationToken cancellationToken = default)
        {
            await WriteNewAsync(path, input!, typeof(TInput), cancellationToken);
            return await inner.ExecuteAsync(executionContext, input, cancellationToken);
        }
    }

    private sealed record Parsed(
        CliCommand? Command,
        AgentContract? Agent,
        Dictionary<string, string> Options,
        bool Help);

    private sealed class UsageException(string message) : Exception(message);

    private sealed class ConfigurationException(string message) : Exception(message);
}