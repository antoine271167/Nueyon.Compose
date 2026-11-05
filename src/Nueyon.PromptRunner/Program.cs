using System.Text.Json;
using Microsoft.Extensions.AI;
using Nueyon.PromptRunner;
using OpenAI;
using YamlDotNet.Core;

// Exit codes: 0 success, 1 args/config/input, 2 rendering, 3 execution.
return await Run(args);

static async Task<int> Run(string[] args)
{
    var options = new Dictionary<string, string>(StringComparer.Ordinal);
    var validate = false;
    var printRendered = false;

    for (var i = 0; i < args.Length; i++)
    {
        switch (args[i])
        {
            case "--help":
                Console.Out.WriteLine(HelpText.Main);
                return 0;
            case "--help-templates":
                Console.Out.WriteLine(HelpText.Templates);
                return 0;
            case "--example":
                Console.Out.WriteLine(HelpText.Example());
                return 0;
            case "--validate":
                validate = true;
                break;
            case "--print-rendered":
                printRendered = true;
                break;
            case "--system":
            case "--user":
            case "--data":
            case "--config":
                var name = args[i];
                if (i + 1 >= args.Length || args[i + 1].StartsWith("--", StringComparison.Ordinal))
                {
                    return Fail(1, $"Missing value for {name}.");
                }

                if (!options.TryAdd(name, args[++i]))
                {
                    return Fail(1, $"Duplicate argument {name}.");
                }

                break;
            default:
                return Fail(1,
                    args[i].StartsWith("--", StringComparison.Ordinal)
                        ? $"Unknown argument '{args[i]}'. Use --help."
                        : $"Unexpected positional argument '{args[i]}'. Use --help.");
        }
    }

    options.TryGetValue("--system", out var system);
    options.TryGetValue("--user", out var user);
    options.TryGetValue("--data", out var data);
    options.TryGetValue("--config", out var explicitConfig);

    if (system is null || user is null || data is null)
    {
        return Fail(1, "--system, --user and --data are all required. Use --help.");
    }

    if (validate && printRendered)
    {
        return Fail(1, "--validate and --print-rendered cannot be combined.");
    }

    string systemText, userText;
    object? yaml;
    try
    {
        systemText = File.ReadAllText(system);
        userText = File.ReadAllText(user);
        var dataText = File.ReadAllText(data);
        yaml = PromptTemplate.LoadYaml(dataText);
    }
    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException
                                   or NotSupportedException or YamlException)
    {
        return Fail(1, $"Cannot load input: {ex.Message}");
    }

    string renderedSystem, renderedUser;
    try
    {
        renderedSystem = PromptTemplate.Render(systemText, yaml, system);
        renderedUser = PromptTemplate.Render(userText, yaml, user);
    }
    catch (RenderException ex)
    {
        return Fail(2, ex.Message);
    }

    if (validate)
    {
        Console.Out.WriteLine("OK: inputs are valid and all placeholders resolve.");
        return 0;
    }

    if (printRendered)
    {
        Console.Out.WriteLine("===== SYSTEM =====");
        Console.Out.WriteLine(renderedSystem);
        Console.Out.WriteLine("===== USER =====");
        Console.Out.WriteLine(renderedUser);
        Console.Out.WriteLine("===== END =====");
        return 0;
    }

    string provider, model;
    try
    {
        var cwdConfig = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var configPath = explicitConfig is not null
            ? Path.GetFullPath(explicitConfig)
            : File.Exists(cwdConfig)
                ? cwdConfig
                : Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(configPath));
        var llm = doc.RootElement.GetProperty("Llm");
        provider = llm.GetProperty("Provider").GetString() ?? "";
        model = llm.GetProperty("Model").GetString() ?? "";
    }
    catch (Exception ex) when (ex is IOException or JsonException or KeyNotFoundException or InvalidOperationException
                                   or UnauthorizedAccessException or ArgumentException or NotSupportedException)
    {
        return Fail(1, $"Invalid or missing appsettings.json (Llm:Provider, Llm:Model required): {ex.Message}");
    }

    if (!string.Equals(provider, "OpenAI", StringComparison.OrdinalIgnoreCase))
    {
        return Fail(1, $"Unsupported Llm:Provider '{provider}'. Only 'OpenAI' is supported.");
    }

    if (string.IsNullOrWhiteSpace(model))
    {
        return Fail(1, "Llm:Model must not be empty.");
    }

    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Fail(1, "Environment variable OPENAI_API_KEY is not set.");
    }

    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        // ReSharper disable once AccessToDisposedClosure
        cts.Cancel();
    };

    try
    {
        var chatClient = new OpenAIClient(apiKey).GetChatClient(model).AsIChatClient();
        var agent = chatClient.AsAIAgent(renderedSystem, "PromptRunner");
        var response = await agent.RunAsync(renderedUser, cancellationToken: cts.Token);
        Console.Out.WriteLine(response.Text);
        return 0;
    }
    catch (OperationCanceledException)
    {
        return Fail(3, "Execution cancelled.");
    }
    catch (Exception ex)
    {
        return Fail(3, $"Agent execution failed: {ex.Message}");
    }
}

static int Fail(int code, string message)
{
    Console.Error.WriteLine($"error: {message}");
    return code;
}