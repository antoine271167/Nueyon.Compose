using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console;

public enum CliCommandKind
{
    RunWorkflow,
    RunAgent,
    AgentsList,
    AgentsDescribe,
    AgentsSchema,
    Describe
}

/// <param name="Formats">Accepted values, when the option is an enumeration.</param>
/// <param name="Agents">Agents the option applies to; null means every agent (or no agent).</param>
public sealed record CliOption(
    string Name,
    string ValueType,
    bool Required,
    string Description,
    string[]? Formats = null,
    string[]? Agents = null)
{
    public bool AppliesTo(AgentContract? agent) =>
        Agents is null || (agent is not null && Agents.Contains(agent.Name, StringComparer.Ordinal));
}

/// <summary>
///     The single description of a command: dispatch, help and JSON discovery are all derived from it.
///     <see cref="Path" /> holds the literal tokens; <see cref="TakesAgent" /> appends an agent placeholder.
/// </summary>
public sealed record CliCommand(
    CliCommandKind Kind,
    string[] Path,
    bool TakesAgent,
    string Description,
    CliOption[] Options,
    string[] Examples)
{
    public string Syntax => string.Join(' ', Path) + (TakesAgent ? " <agent>" : "");
    public bool RequiresRuntime => Kind is CliCommandKind.RunWorkflow or CliCommandKind.RunAgent;
}

public sealed record CliExitCode(int Code, string Meaning);

public sealed record CliDefault(string Command, string Description, int InputErrorExitCode);

public sealed record IdeaTrace(string Status, IReadOnlyList<IdeaValidationAttempt> Attempts);

public interface IAgentDecorator
{
    IAgent<TInput, TOutput> Decorate<TInput, TOutput>(string name, IAgent<TInput, TOutput> agent);
}

public abstract class AgentContract(string name, string description, object validation)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public abstract Type InputType { get; }
    public abstract Type OutputType { get; }
    public abstract object ReadInput(string json);
    public abstract Task<object> ExecuteAsync(IServiceProvider services, object input, CancellationToken token);

    public JsonNode InputSchema() =>
        CliContracts.ExportSchema(InputType, $"Input for the '{Name}' agent: the JSON read by 'run {Name} --input'.");

    /// <summary>Agent metadata; commands and exit codes are described once, globally.</summary>
    public JsonObject Describe() => new()
    {
        ["name"] = Name,
        ["description"] = Description,
        ["inputType"] = InputType.Name,
        ["outputType"] = OutputType.Name,
        ["inputSchema"] = InputSchema(),
        ["outputSchema"] = CliContracts.ExportSchema(OutputType,
            $"Result of the '{Name}' agent: the JSON written by 'run {Name} --output'.", true),
        ["traceSchema"] = Name == CliContracts.IdeaContract.Name
            ? CliContracts.ExportSchema(typeof(IdeaTrace),
                "Opt-in Idea validation trace written by 'run idea --trace'.", true)
            : null,
        ["validation"] = JsonSerializer.SerializeToNode(validation, CliContracts.OutputOptions)
    };
}

public sealed class AgentContract<TInput, TOutput>(string name, string description, object validation)
    : AgentContract(name, description, validation)
    where TInput : class
{
    public override Type InputType => typeof(TInput);
    public override Type OutputType => typeof(TOutput);

    public override object ReadInput(string json) =>
        JsonSerializer.Deserialize<TInput>(json, CliContracts.Options) ??
        throw new JsonException("Input must not be null.");

    public IAgent<TInput, TOutput> Resolve(IServiceProvider services, IAgentDecorator decorator) =>
        decorator.Decorate(Name, services.GetRequiredService<IAgent<TInput, TOutput>>());

    public override async Task<object> ExecuteAsync(IServiceProvider services, object input, CancellationToken token) =>
        (await services.GetRequiredService<IAgent<TInput, TOutput>>()
            .ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), (TInput)input, token))!;
}

public static class CliContracts
{
    public const string DefaultInputDirectory = "data/input";
    public static readonly string SourcePromptPath = Path.Combine("src", "Nueyon.Compose.Application", "Prompts");
    public static readonly CliOption Help = new("--help", "flag", false, "Show this help.");

    public static readonly CliDefault NoArguments =
        new("run workflow", "Run the production workflow with the default input directory.", 1);

    public static readonly JsonSerializerOptions Options = CreateOptions(EvidenceMayBeNull);

    // Written results are not re-validated: the production parser can leave fields null.
    public static readonly JsonSerializerOptions OutputOptions = new(Options) { RespectNullableAnnotations = false };
    private static readonly JsonSerializerOptions _ideaOutputOptions = CreateOptions(AllFieldsMayBeNull);

    private static readonly object _ideaValidation = new
    {
        kind = "idea-validation-loop",
        appliesTo = "firstRankedIdea",
        alternativesValidated = false,
        structural = new { requiredNonEmptyFields = new[] { "title", "description", "audience", "rationale" } },
        semantic = new { enabled = true, judge = "IdeaSemanticJudge", blocking = true },
        maxAttempts = IdeaValidationLoopEvaluator.MaxAttempts
    };

    private static readonly object _contractValidation = new
    {
        kind = "json-contract",
        description = "JSON contract validation only; the agent may apply further runtime validation."
    };

    public static readonly AgentContract<StoryInput, IReadOnlyList<Idea>> IdeaContract =
        new("idea", "Generate and validate ranked ideas.", _ideaValidation);

    public static readonly AgentContract<ResearchInput, ResearchResult> ResearchContract =
        new("research", "Research a selected idea.", _contractValidation);

    public static readonly AgentContract<SynthesisInput, SynthesisResult> SynthesisContract =
        new("synthesis", "Synthesize research.", _contractValidation);

    public static readonly AgentContract<NarrativeInput, NarrativeResult> NarrativeContract =
        new("narrative", "Create a narrative.", _contractValidation);

    public static readonly AgentContract<ComposeInput, ComposeResult> ComposeContract =
        new("compose", "Compose the final content.", _contractValidation);

    public static readonly AgentContract[] Agents =
        [IdeaContract, ResearchContract, SynthesisContract, NarrativeContract, ComposeContract];

    private static readonly CliOption
        _prompts = new("--prompts", "directory", false, "Use a complete disk prompt set.");

    public static readonly CliCommand[] Commands =
    [
        new(CliCommandKind.RunWorkflow, ["run", "workflow"], false, "Run the complete production workflow.",
        [
            new CliOption("--input-directory", "directory", false,
                $"Markdown source directory (default {DefaultInputDirectory})."),
            new CliOption("--capture-inputs", "directory", false, "Capture agent inputs in a unique directory."),
            _prompts
        ], ["run workflow --capture-inputs .\\captures"]),
        new(CliCommandKind.RunAgent, ["run"], true, "Run one agent from JSON.",
        [
            new CliOption("--input", "json file", true, "Input JSON file."),
            new CliOption("--output", "json file", true, "New output JSON file."),
            _prompts,
            new CliOption("--trace", "json file", false, "New Idea validation trace file.", Agents: ["idea"])
        ], ["run idea --input .\\idea.json --output .\\result.json --trace .\\trace.json"]),
        new(CliCommandKind.AgentsList, ["agents", "list"], false, "List available agents.", [], ["agents list"]),
        new(CliCommandKind.AgentsDescribe, ["agents", "describe"], true, "Describe an agent.",
        [
            new CliOption("--format", "text|json", false, "Output format (default text).", ["text", "json"])
        ], ["agents describe idea --format json"]),
        new(CliCommandKind.AgentsSchema, ["agents", "schema"], true, "Export the standalone input JSON schema.",
        [
            new CliOption("--format", "json", true, "Output format; must be json.", ["json"])
        ], ["agents schema idea --format json"]),
        new(CliCommandKind.Describe, ["describe"], false,
            "Describe all commands, agents, help, defaults, schemas, exit codes, prompts and runtime requirements.",
            [
                new CliOption("--format", "json", true, "Output format; must be json.", ["json"])
            ], ["describe --format json"])
    ];

    public static readonly CliExitCode[] ExitCodes =
    [
        new(0, "success"),
        new(1, "runtime or configuration failure (also input errors when run without arguments)"),
        new(2, "usage or input error"),
        new(130, "cancelled")
    ];

    public static AgentContract? FindAgent(string name) =>
        Agents.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));

    public static JsonObject DescribeAgent(AgentContract agent)
    {
        var node = agent.Describe();
        node["commands"] = JsonSerializer.SerializeToNode(Commands.Where(c => c.TakesAgent).Select(c =>
        {
            var options = c.Options.Where(o => o.AppliesTo(agent)).ToArray();
            var examples = c.Kind switch
            {
                CliCommandKind.RunAgent =>
                [
                    $"run {agent.Name} --input .\\{agent.Name}.json --output .\\{agent.Name}-result.json" +
                    (options.Any(o => o.Name == "--trace") ? $" --trace .\\{agent.Name}-trace.json" : "")
                ],
                CliCommandKind.AgentsDescribe => [$"agents describe {agent.Name} --format json"],
                CliCommandKind.AgentsSchema => [$"agents schema {agent.Name} --format json"],
                _ => c.Examples
            };
            return c with { Options = options, Examples = examples };
        }), OutputOptions);
        node["exitCodes"] = JsonSerializer.SerializeToNode(ExitCodes, OutputOptions);
        return node;
    }

    public static JsonObject DescribeAll() => new()
    {
        ["name"] = "Nueyon.Compose",
        ["commands"] = JsonSerializer.SerializeToNode(Commands, OutputOptions),
        ["help"] = JsonSerializer.SerializeToNode(new
        {
            option = Help.Name,
            description = Help.Description,
            appliesTo = "all commands"
        }, OutputOptions),
        ["noArguments"] = JsonSerializer.SerializeToNode(NoArguments, OutputOptions),
        ["agents"] = new JsonArray(Agents.Select(agent => (JsonNode?)agent.Describe()).ToArray()),
        ["exitCodes"] = JsonSerializer.SerializeToNode(ExitCodes, OutputOptions),
        ["promptDefaults"] = JsonSerializer.SerializeToNode(new
        {
            option = _prompts.Name,
            strict = true,
            resolutionOrder = new[]
            {
                "--prompts <directory>",
                $"<working directory>\\{SourcePromptPath}",
                "<executable directory>\\Prompts",
                "embedded prompts"
            }
        }, OutputOptions),
        ["runtime"] = JsonSerializer.SerializeToNode(new
        {
            discoveryNeedsConfiguration = false,
            commandsRequiringRuntime = Commands.Where(c => c.RequiresRuntime).Select(c => c.Syntax).ToArray(),
            requiredConfiguration = new[] { "OpenAI:ApiKey", "OpenAI:Model" },
            configurationSources = new[]
            {
                "appsettings.json (working directory, then src\\Nueyon.Compose.Host.Console, then executable directory)",
                "appsettings.<ASPNETCORE_ENVIRONMENT>.json", "environment variables", "user secrets"
            },
            networkAccess = true,
            mayIncurCost = true
        }, OutputOptions)
    };

    public static JsonNode ExportSchema(Type type, string? description = null, bool output = false)
    {
        var options = output && type == typeof(IReadOnlyList<Idea>) ? _ideaOutputOptions : Options;
        var exporter = new JsonSchemaExporterOptions
        {
            TransformSchemaNode = (context, node) =>
            {
                if (context.TypeInfo.Type.IsEnum)
                {
                    return new JsonObject
                    {
                        ["type"] = "string",
                        ["enum"] = new JsonArray(Enum.GetNames(context.TypeInfo.Type).Select(n => (JsonNode?)n)
                            .ToArray())
                    };
                }

                if (node is JsonObject obj && context.Path.IsEmpty && description is not null)
                {
                    obj["description"] = description;
                }

                return node;
            }
        };
        var schema = options.GetJsonSchemaAsNode(type, exporter);
        if (schema is JsonObject root && root["type"] is JsonArray types)
        {
            var nonNull = types.Select(node => node?.GetValue<string>()).Where(value => value != "null").ToArray();
            if (nonNull.Length == 1)
            {
                root["type"] = JsonValue.Create(nonNull[0]);
            }
        }

        return schema;
    }

    // The production Idea parser permits a missing or null Evidence; it still reaches later agents.
    private static void EvidenceMayBeNull(JsonTypeInfo info) => RelaxIdea(info, name => name == "evidence");

    // The parser also keeps null fields in alternatives, which are never validated.
    private static void AllFieldsMayBeNull(JsonTypeInfo info) => RelaxIdea(info, _ => true);

    private static void RelaxIdea(JsonTypeInfo info, Func<string, bool> relaxed)
    {
        if (info.Type != typeof(Idea))
        {
            return;
        }

        foreach (var property in info.Properties.Where(p => relaxed(p.Name)))
        {
            property.IsGetNullable = true;
            property.IsSetNullable = true;
            if (property.Name == "evidence")
            {
                property.IsRequired = false;
            }
        }
    }

    private static JsonSerializerOptions CreateOptions(Action<JsonTypeInfo> modifier)
    {
        var resolver = new DefaultJsonTypeInfoResolver();
        resolver.Modifiers.Add(modifier);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = false,
            RespectNullableAnnotations = true,
            RespectRequiredConstructorParameters = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            TypeInfoResolver = resolver
        };
        options.Converters.Add(new ExactEnumConverterFactory());
        return options;
    }

    /// <summary>Enums are exact, case-sensitive strings; numbers and other casings are rejected.</summary>
    private sealed class ExactEnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(typeof(ExactEnumConverter<>).MakeGenericType(type))!;
    }

    private sealed class ExactEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
            if (value is null || !Enum.GetNames<T>().Contains(value, StringComparer.Ordinal))
            {
                throw new JsonException($"Value must be one of: {string.Join(", ", Enum.GetNames<T>())}.");
            }

            return Enum.Parse<T>(value);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}