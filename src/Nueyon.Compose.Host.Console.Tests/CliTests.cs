using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Host.Console.Tests;

[Collection("Console")]
public sealed class CliTests
{
    [Fact]
    public async Task InvalidSyntaxIsUsageError()
    {
        string[][] cases =
        [
            ["run", "idea", "--input"],
            ["run", "idea", "--input", "a", "--input", "b", "--output", "c"],
            ["agents", "schema", "idea", "--format", "xml"],
            ["agents", "list", "extra"],
            ["run", "compose", "--trace", "x", "--help"]
        ];
        foreach (var args in cases)
        {
            Assert.Equal(2, await Cli.RunAsync(args));
        }
    }

    [Fact]
    public async Task DiscoveryDoesNotNeedConfiguration()
    {
        Assert.Equal(0, (await Invoke("--help")).Code);
        Assert.Equal(0, (await Invoke("agents", "describe", "IDEA", "--format", "json")).Code);
        Assert.Equal(0, (await Invoke("agents", "schema", "compose", "--format", "json")).Code);
    }

    [Fact]
    public async Task ExactRequiredCommandFormsAreRouted()
    {
        Assert.Contains("run workflow", (await Invoke("run", "workflow", "--help")).Out);
        Assert.Contains("run <agent>", (await Invoke("run", "--help")).Out);
        Assert.Equal(2, (await Invoke("describe")).Code);
        Assert.Equal(2, (await Invoke("describe", "--format", "text")).Code);
        Assert.Equal(2, (await Invoke("workflow")).Code);
        Assert.Equal(2, (await Invoke("run", "workflow", "--trace", "x")).Code);
        Assert.Equal(2,
            (await Invoke("run", "workflow", "--input-directory",
                Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}"))).Code);
        var root = (await Invoke("--help")).Out;
        foreach (var command in CliContracts.Commands)
        {
            Assert.Contains(command.Syntax, root);
        }

        Assert.Contains("describe --format json", root);
    }

    [Fact]
    public async Task GlobalDescribeIsDerivedFromCommandMetadata()
    {
        var (code, output) = await Invoke("describe", "--format", "json");
        Assert.Equal(0, code);
        using var document = JsonDocument.Parse(output);
        var root = document.RootElement;
        var syntaxes = root.GetProperty("commands").EnumerateArray().Select(c => c.GetProperty("syntax").GetString())
            .ToArray();
        Assert.Equal(CliContracts.Commands.Select(c => c.Syntax), syntaxes);
        Assert.Contains("run workflow", syntaxes);
        var run = root.GetProperty("commands").EnumerateArray()
            .Single(c => c.GetProperty("syntax").GetString() == "run <agent>");
        var trace = run.GetProperty("options").EnumerateArray()
            .Single(o => o.GetProperty("name").GetString() == "--trace");
        Assert.Equal("idea", trace.GetProperty("agents")[0].GetString());
        var agents = root.GetProperty("agents").EnumerateArray().ToArray();
        Assert.Equal(CliContracts.Agents.Select(a => a.Name), agents.Select(a => a.GetProperty("name").GetString()));
        foreach (var agent in agents)
        {
            Assert.False(agent.TryGetProperty("commands", out _));
            Assert.Equal(JsonValueKind.Object, agent.GetProperty("inputSchema").ValueKind);
            Assert.Equal(JsonValueKind.Object, agent.GetProperty("outputSchema").ValueKind);
        }

        var idea = agents[0];
        Assert.Equal(JsonValueKind.Object, idea.GetProperty("traceSchema").ValueKind);
        var validation = idea.GetProperty("validation");
        Assert.Equal(IdeaValidationLoopEvaluator.MaxAttempts, validation.GetProperty("maxAttempts").GetInt32());
        Assert.Equal("firstRankedIdea", validation.GetProperty("appliesTo").GetString());
        Assert.True(validation.GetProperty("semantic").GetProperty("enabled").GetBoolean());
        Assert.False(validation.GetProperty("alternativesValidated").GetBoolean());
        Assert.Equal(CliContracts.ExitCodes.Length, root.GetProperty("exitCodes").GetArrayLength());
        Assert.True(root.GetProperty("promptDefaults").GetProperty("strict").GetBoolean());
        Assert.Equal("--help", root.GetProperty("help").GetProperty("option").GetString());
        var noArguments = root.GetProperty("noArguments");
        Assert.Equal("run workflow", noArguments.GetProperty("command").GetString());
        Assert.Equal(1, noArguments.GetProperty("inputErrorExitCode").GetInt32());
        Assert.Contains("OpenAI:ApiKey",
            root.GetProperty("runtime").GetProperty("requiredConfiguration").EnumerateArray()
                .Select(e => e.GetString()));
    }

    [Fact]
    public async Task SchemaCommandEmitsStandaloneDocumentedInputSchema()
    {
        var (code, output) = await Invoke("agents", "schema", "idea", "--format", "json");
        Assert.Equal(0, code);
        using var schema = JsonDocument.Parse(output);
        Assert.True(schema.RootElement.TryGetProperty("properties", out var properties));
        Assert.True(properties.TryGetProperty("content", out _));
        Assert.False(schema.RootElement.TryGetProperty("outputSchema", out _));
        Assert.Contains("run idea", schema.RootElement.GetProperty("description").GetString());
        var (_, describe) = await Invoke("agents", "describe", "idea", "--format", "json");
        using var description = JsonDocument.Parse(describe);
        Assert.Equal(JsonValueKind.Object, description.RootElement.GetProperty("traceSchema").ValueKind);
        Assert.Equal(JsonValueKind.Object, description.RootElement.GetProperty("outputSchema").ValueKind);
        Assert.Equal(JsonValueKind.Array, description.RootElement.GetProperty("commands").ValueKind);
        var compose = CliContracts.DescribeAgent(CliContracts.FindAgent("compose")!);
        var run = compose["commands"]!.AsArray().Single(c => c!["syntax"]!.GetValue<string>() == "run <agent>");
        Assert.DoesNotContain("--trace", run!["examples"]!.ToJsonString());
        Assert.Contains("run compose", run["examples"]![0]!.GetValue<string>());
    }

    [Fact]
    public void NonIdeaOutputsAreNotAdvertisedAsNullable()
    {
        var research = CliContracts.ExportSchema(typeof(ResearchResult), output: true);
        Assert.Equal("string", research["properties"]!["content"]!["type"]!.GetValue<string>());
        var ideas = CliContracts.ExportSchema(typeof(IReadOnlyList<Idea>), output: true);
        Assert.Contains("null", ideas["items"]!["properties"]!["title"]!["type"]!.ToJsonString());
    }

    [Fact]
    public void InputContractsRejectMissingNullUnknownAndNoncanonicalEnum()
    {
        var idea = CliContracts.FindAgent("idea")!;
        Assert.IsType<StoryInput>(idea.ReadInput("""{"content":"source"}"""));
        foreach (var json in new[]
                     { "null", "{}", """{"content":null}""", """{"content":"x","extra":1}""", """{"Content":"x"}""" })
        {
            Assert.Throws<JsonException>(() => idea.ReadInput(json));
        }

        var compose = CliContracts.FindAgent("compose")!;
        Assert.IsType<ComposeInput>(
            compose.ReadInput("""{"narrative":{"content":"x"},"composition":{"format":"Article"}}"""));
        foreach (var value in new[] { "\"article\"", "\"Unknown\"", "1" })
        {
            Assert.Throws<JsonException>(() =>
                compose.ReadInput("""{"narrative":{"content":"x"},"composition":{"format":""" + value + "}}"));
        }

        var schema = CliContracts.ExportSchema(typeof(ComposeInput)).ToJsonString();
        Assert.Contains("additionalProperties", schema);
        Assert.Contains("LinkedInPost", schema);
    }

    [Fact]
    public async Task TypedReplayUsesRegisteredAgentAndCapturedInputIsReplayable()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var path = Path.Combine(directory, "idea.json");
            var fake = new FakeAgent();
            var capture = new Cli.CaptureAgent<StoryInput, IReadOnlyList<Idea>>(fake, path);
            var input = new StoryInput("source");
            await capture.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), input);
            var contract = CliContracts.FindAgent("idea")!;
            var replay = contract.ReadInput(await File.ReadAllTextAsync(path));
            Assert.Equal(input, replay);
            var services = new ServiceCollection().AddSingleton<IAgent<StoryInput, IReadOnlyList<Idea>>>(fake)
                .BuildServiceProvider();
            var output = await contract.ExecuteAsync(services, replay, CancellationToken.None);
            Assert.Single((IReadOnlyList<Idea>)output);
            await Assert.ThrowsAsync<IOException>(() =>
                capture.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()), input));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task AtomicWriteDoesNotOverwriteExistingResult()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var path = Path.Combine(directory, "out.json");
            await File.WriteAllTextAsync(path, "original");
            await Assert.ThrowsAsync<IOException>(() => Cli.WriteNewAsync(path, new StoryInput("secret")));
            Assert.Equal("original", await File.ReadAllTextAsync(path));
            Assert.Single(Directory.GetFiles(directory));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task IdeaResultAllowsNullFieldsProducedByProductionParser()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var path = Path.Combine(directory, "out.json");
            IReadOnlyList<Idea> result = [new("t", "d", "a", "r", null!)];
            await Cli.WriteNewAsync(path, result, typeof(IReadOnlyList<Idea>));
            using var json = JsonDocument.Parse(await File.ReadAllTextAsync(path));
            Assert.Equal(JsonValueKind.Null, json.RootElement[0].GetProperty("evidence").ValueKind);
            var schema = CliContracts.ExportSchema(typeof(IReadOnlyList<Idea>), output: true);
            Assert.Contains("null", schema["items"]!["properties"]!["evidence"]!["type"]!.ToJsonString());
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task MissingExplicitPromptSetFailsBeforeRuntime()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var input = Path.Combine(directory, "input.json");
            await File.WriteAllTextAsync(input, """{"content":"source"}""");
            Assert.Equal(2, await Cli.RunAsync([
                "run", "idea", "--input", input, "--output",
                Path.Combine(directory, "result.json"), "--prompts", directory
            ]));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public void CompletePromptSetIsSelectedAsOneDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            const string prefix = "Nueyon.Compose.Application.Prompts.";
            foreach (var resource in typeof(PromptDirectory).Assembly.GetManifestResourceNames()
                         .Where(n => n.StartsWith(prefix)))
            {
                var name = resource[prefix.Length..];
                var separator = name.IndexOf('.');
                var group = Path.Combine(directory, name[..separator]);
                Directory.CreateDirectory(group);
                File.WriteAllText(Path.Combine(group, name[(separator + 1)..]), "test");
            }

            Assert.Equal(directory, Cli.ResolvePrompts(directory));
            Assert.NotNull(Cli.ResolvePrompts(null));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task InvalidInputAndExistingOutputFailBeforeRuntime()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var input = Path.Combine(directory, "input.json");
            var output = Path.Combine(directory, "output.json");
            await File.WriteAllTextAsync(input, """{"content":null}""");
            Assert.Equal(2, await Cli.RunAsync(["run", "idea", "--input", input, "--output", output]));
            await File.WriteAllTextAsync(input, """{"content":"source"}""");
            await File.WriteAllTextAsync(output, "untouched");
            Assert.Equal(2, await Cli.RunAsync(["run", "idea", "--input", input, "--output", output]));
            Assert.Equal("untouched", await File.ReadAllTextAsync(output));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task WorkflowCaptureWithNullEvidenceRoundTripsAllFiveBoundaries()
    {
        var root = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        try
        {
            var provider = new ServiceCollection()
                .AddSingleton<IAgent<StoryInput, IReadOnlyList<Idea>>>(
                    new FuncAgent<StoryInput, IReadOnlyList<Idea>>(_ => [new Idea("t", "d", "a", "r", null!)]))
                .AddSingleton<IAgent<ResearchInput, ResearchResult>>(
                    new FuncAgent<ResearchInput, ResearchResult>(_ => new ResearchResult("r")))
                .AddSingleton<IAgent<SynthesisInput, SynthesisResult>>(
                    new FuncAgent<SynthesisInput, SynthesisResult>(_ => new SynthesisResult("s")))
                .AddSingleton<IAgent<NarrativeInput, NarrativeResult>>(
                    new FuncAgent<NarrativeInput, NarrativeResult>(_ => new NarrativeResult("n")))
                .AddSingleton<IAgent<ComposeInput, ComposeResult>>(
                    new FuncAgent<ComposeInput, ComposeResult>(_ => new ComposeResult("c")))
                .BuildServiceProvider();
            var first = Cli.CreateCaptureDirectory(root);
            var second = Cli.CreateCaptureDirectory(root);
            Assert.NotEqual(first, second);
            foreach (var capture in new[] { first, second })
            {
                var tracker = new Cli.StageTracker();
                var workflow = Cli.CreateWorkflow(provider, new Cli.WorkflowDecorator(capture, tracker));
                var result = await workflow.RunAsync(new StoryInput("source"));
                Assert.Null(result.SelectedIdea.Idea.Evidence);
                Assert.Equal("compose", tracker.Stage);
                Assert.Equal(5, Directory.GetFiles(capture).Length);
                foreach (var contract in CliContracts.Agents)
                {
                    var json = await File.ReadAllTextAsync(Path.Combine(capture, contract.Name + ".json"));
                    Assert.IsType(contract.InputType, contract.ReadInput(json));
                    if (contract.Name is "research" or "synthesis" or "narrative")
                    {
                        Assert.Contains("\"evidence\": null", json.Replace("\":null", "\": null"));
                    }
                }

                var research = (ResearchInput)CliContracts.ResearchContract.ReadInput(
                    await File.ReadAllTextAsync(Path.Combine(capture, "research.json")));
                Assert.Null(research.SelectedIdea.Idea.Evidence);
            }
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, true);
            }
        }
    }

    [Fact]
    public void ReplayStillRequiresOtherIdeaFields()
    {
        var synthesis = CliContracts.SynthesisContract;
        const string idea = """{"title":"t","description":"d","audience":"a","rationale":"r"}""";
        Assert.IsType<SynthesisInput>(
            synthesis.ReadInput("{\"selectedIdea\":" + idea + ",\"research\":{\"content\":\"x\"}}"));
        Assert.Throws<JsonException>(() =>
            synthesis.ReadInput("{\"selectedIdea\":" + idea.Replace("\"title\":\"t\",", "") +
                                ",\"research\":{\"content\":\"x\"}}"));
        Assert.Throws<JsonException>(() =>
            synthesis.ReadInput("{\"selectedIdea\":" + idea.Replace("\"d\"", "null") +
                                ",\"research\":{\"content\":\"x\"}}"));
        Assert.Throws<JsonException>(() =>
            synthesis.ReadInput("{\"selectedIdea\":null,\"research\":{\"content\":\"x\"}}"));
    }

    [Fact]
    public void DiagnosticsAreSanitizedAndNameTheStage()
    {
        var tracker = new Cli.StageTracker { Stage = "research" };
        var message = Cli.Diagnose(new InvalidOperationException("sk-secret payload"), tracker);
        Assert.DoesNotContain("secret", message);
        Assert.Contains("research", message);
        Assert.Contains("InvalidOperationException", message);
        var http = Cli.Diagnose(
            new AggregateException(new HttpRequestException("sk-secret", null, HttpStatusCode.TooManyRequests)),
            tracker);
        Assert.Contains("429", http);
        Assert.DoesNotContain("secret", http);
    }

    [Fact]
    public async Task CancelledWriteLeavesNoResult()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();
            var path = Path.Combine(directory, "out.json");
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                Cli.WriteNewAsync(path, new StoryInput("x"), null, cts.Token));
            Assert.Empty(Directory.GetFiles(directory));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task MalformedConfigurationIsReportedWithoutLeakingContent()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        var original = Directory.GetCurrentDirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory, "appsettings.json"),
                """{"attacker-controlled-config-key": sk-leaked-value}""");
            var input = Path.Combine(directory, "input.json");
            await File.WriteAllTextAsync(input, """{"content":"source"}""");
            Directory.SetCurrentDirectory(directory);
            var (code, _, error) = await InvokeAll("run", "idea", "--input", input, "--output",
                Path.Combine(directory, "result.json"));
            Assert.Equal(1, code);
            Assert.Contains("Configuration error", error);
            Assert.DoesNotContain("sk-leaked-value", error);
            Assert.DoesNotContain("attacker-controlled-config-key", error);
            Assert.False(File.Exists(Path.Combine(directory, "result.json")));
        }
        finally
        {
            Directory.SetCurrentDirectory(original);
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task NoArgumentsReportsInputErrorToStderrWithLegacyExitCode()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(directory, CliContracts.DefaultInputDirectory));
        var original = Directory.GetCurrentDirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory, "appsettings.json"),
                """{"OpenAI":{"ApiKey":"offline-placeholder","Model":"offline-placeholder"}}""");
            Directory.SetCurrentDirectory(directory);
            var (code, output, error) = await InvokeAll();
            Assert.Equal(CliContracts.NoArguments.InputErrorExitCode, code);
            Assert.Contains("No Markdown files (*.md) found", error);
            Assert.DoesNotContain("Error:", output);
        }
        finally
        {
            Directory.SetCurrentDirectory(original);
            Directory.Delete(directory, true);
        }
    }

    private static async Task<(int Code, string Out)> Invoke(params string[] args)
    {
        var (code, output, _) = await InvokeAll(args);
        return (code, output);
    }

    private static async Task<(int Code, string Out, string Error)> InvokeAll(params string[] args)
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        await using var output = new StringWriter();
        await using var error = new StringWriter();
        System.Console.SetOut(output);
        System.Console.SetError(error);
        try
        {
            return (await Cli.RunAsync(args), output.ToString(), error.ToString());
        }
        finally
        {
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
        }
    }

    private sealed class FuncAgent<TInput, TOutput>(Func<TInput, TOutput> func) : IAgent<TInput, TOutput>
    {
        public Task<TOutput> ExecuteAsync(AgentExecutionContext executionContext, TInput input,
            CancellationToken cancellationToken = default) => Task.FromResult(func(input));
    }

    private sealed class FakeAgent : IAgent<StoryInput, IReadOnlyList<Idea>>
    {
        public Task<IReadOnlyList<Idea>> ExecuteAsync(AgentExecutionContext executionContext, StoryInput input,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Idea>>([new Idea("t", "d", "a", "r", "e")]);
    }
}