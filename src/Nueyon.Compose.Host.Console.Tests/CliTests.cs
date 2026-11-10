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
        foreach (var args in new[]
                 {
                     new[] { "agents", "describe", "IDEA", "--format", "json" },
                     ["agents", "schema", "compose", "--format", "json"],
                     ["describe", "--format", "json"], ["run", "idea", "--help"]
                 })
        {
            var (code, _, error) = await InvokeAll(args);
            Assert.Equal(0, code);
            Assert.DoesNotContain("Prompts:", error);
        }
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
        using var directory = new TempDirectory();
        await File.WriteAllTextAsync(directory.Input, """{"content":"source"}""");

        var (code, _, error) = await InvokeWith(Services(_ => [Sample]), "run", "idea", "--input", directory.Input,
            "--output", directory.Output, "--prompts", directory.Path);

        Assert.Equal(2, code);
        Assert.Contains("Input error: Prompt file is missing", error);
        Assert.DoesNotContain("Prompts:", error);
        Assert.False(File.Exists(directory.Output));
    }

    [Fact]
    public void CompletePromptSetIsSelectedAsOneDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            CreatePromptSet(directory);

            Assert.Equal(directory, Cli.ResolvePrompts(directory));
            Assert.NotNull(Cli.ResolvePrompts(null));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public void PromptSourceFollowsPrecedence()
    {
        using var root = new TempDirectory();
        var working = Path.Combine(root.Path, "work");
        var baseDirectory = Path.Combine(root.Path, "bin");
        Directory.CreateDirectory(working);
        Directory.CreateDirectory(baseDirectory);
        Assert.Null(Cli.ResolvePrompts(null, working, baseDirectory));
        Assert.Contains("embedded", Cli.DescribePrompts(null));

        var adjacent = Path.Combine(baseDirectory, "Prompts");
        CreatePromptSet(adjacent);
        Assert.Equal(adjacent, Cli.ResolvePrompts(null, working, baseDirectory));

        var source = Path.Combine(working, CliContracts.SourcePromptPath);
        CreatePromptSet(source);
        Assert.Equal(source, Cli.ResolvePrompts(null, working, baseDirectory));
        Assert.Equal($"Prompts: disk ({source}).", Cli.DescribePrompts(source));

        var explicitSet = Path.Combine(root.Path, "explicit");
        CreatePromptSet(explicitSet);
        Assert.Equal(explicitSet, Cli.ResolvePrompts(explicitSet, working, baseDirectory));
    }

    [Fact]
    public async Task RuntimeReportsExplicitPromptDirectoryOnStandardError()
    {
        using var directory = new TempDirectory();
        var prompts = Path.Combine(directory.Path, "prompts");
        CreatePromptSet(prompts);
        await File.WriteAllTextAsync(directory.Input, """{"content":"source"}""");

        var (code, _, error) = await InvokeWith(Services(_ => [Sample]), "run", "idea", "--input", directory.Input,
            "--output", directory.Output, "--prompts", prompts);

        Assert.Equal(0, code);
        Assert.Contains($"Prompts: disk ({prompts}).", error);

        // A blank explicit value is a usage error, not a request for discovery.
        var (blankCode, _, blankError) = await InvokeWith(Services(_ => [Sample]), "run", "idea", "--input",
            directory.Input, "--output", Path.Combine(directory.Path, "other.json"), "--prompts", " ");
        Assert.Equal(2, blankCode);
        Assert.DoesNotContain("Prompts:", blankError);
    }

    [Fact]
    public async Task AcceptedTraceIsWrittenAfterTheResult()
    {
        using var directory = new TempDirectory();
        var (code, _, _) = await RunIdea(directory, Services(context =>
        {
            Observe(context, new IdeaValidationAttempt(1, "first", "rejected", "fix it"));
            Observe(context, new IdeaValidationAttempt(2, "second", "accepted", null));
            return [Sample];
        }));

        Assert.Equal(0, code);
        using var trace = JsonDocument.Parse(File.ReadAllText(directory.Trace));
        Assert.Equal("accepted", trace.RootElement.GetProperty("status").GetString());
        var attempts = trace.RootElement.GetProperty("attempts").EnumerateArray().ToArray();
        Assert.Equal(["rejected", "accepted"], attempts.Select(a => a.GetProperty("status").GetString()));
        Assert.Equal(["first", "second"], attempts.Select(a => a.GetProperty("response").GetString()));
        Assert.Equal("fix it", attempts[0].GetProperty("feedback").GetString());
        Assert.True(File.Exists(directory.Output));
        Assert.DoesNotContain(Directory.GetFiles(directory.Path), f => f.EndsWith(".tmp"));
    }

    [Fact]
    public async Task ExhaustedIdeaValidationWritesTraceWithoutPublishingResult()
    {
        using var directory = new TempDirectory();
        const string diagnosis =
            """[{"category":"scope","field":"audience","problem":"Audience shifted to staff","sourceEvidence":"Notice for visitors","constraint":"Visitor-facing notice"}]""";
        var (code, _, error) = await RunIdea(directory, Services(context =>
        {
            for (var attempt = 1; attempt <= IdeaValidationLoopEvaluator.MaxAttempts; attempt++)
            {
                Observe(context, new IdeaValidationAttempt(attempt, "rejected generation", "rejected", diagnosis));
            }

            throw new InvalidOperationException("Idea validation exhausted.");
        }));

        Assert.Equal(1, code);
        Assert.Contains("No result was produced.", error);
        Assert.False(File.Exists(directory.Output));
        using var trace = JsonDocument.Parse(File.ReadAllText(directory.Trace));
        Assert.Equal("exhausted", trace.RootElement.GetProperty("status").GetString());
        var attempts = trace.RootElement.GetProperty("attempts").EnumerateArray().ToArray();
        Assert.Equal([1, 2, 3], attempts.Select(a => a.GetProperty("attempt").GetInt32()));
        Assert.All(attempts, a => Assert.Equal(diagnosis, a.GetProperty("feedback").GetString()));
    }

    [Fact]
    public async Task ResultFailureAfterValidationKeepsTheExistingFileAndWritesIncompleteTrace()
    {
        using var directory = new TempDirectory();
        var (code, _, error) = await RunIdea(directory, Services(context =>
        {
            Observe(context, new IdeaValidationAttempt(1, "response", "accepted", null));
            File.WriteAllText(directory.Output, "existing");
            return [Sample];
        }));

        Assert.Equal(1, code);
        Assert.Equal("existing", File.ReadAllText(directory.Output));
        using var trace = JsonDocument.Parse(File.ReadAllText(directory.Trace));
        Assert.Equal("incomplete", trace.RootElement.GetProperty("status").GetString());
        Assert.Equal("accepted",
            Assert.Single(trace.RootElement.GetProperty("attempts").EnumerateArray()).GetProperty("status")
                .GetString());
        Assert.Contains("Runtime error in stage 'idea'", error);
        Assert.Contains("No result was produced.", error);
        Assert.DoesNotContain("The result was written", error);
        Assert.DoesNotContain(Directory.GetFiles(directory.Path), f => f.EndsWith(".tmp"));
    }

    [Fact]
    public async Task TraceFailureAfterPublicationKeepsTheResultAndReportsTruthfully()
    {
        using var directory = new TempDirectory();
        var (code, _, error) = await RunIdea(directory, Services(_ =>
        {
            File.WriteAllText(directory.Trace, "existing");
            return [Sample];
        }));

        Assert.Equal(1, code);
        Assert.Equal("existing", File.ReadAllText(directory.Trace));
        using var result = JsonDocument.Parse(File.ReadAllText(directory.Output));
        Assert.Equal("t", result.RootElement[0].GetProperty("title").GetString());
        Assert.Contains("The result was written, but the requested trace could not be written.", error);
        Assert.DoesNotContain("No result was produced", error);
        Assert.DoesNotContain(Directory.GetFiles(directory.Path), f => f.EndsWith(".tmp"));
    }

    private static readonly Idea Sample = new("t", "d", "a", "r", "e");

    private static void Observe(AgentExecutionContext context, IdeaValidationAttempt attempt) =>
        ((Action<IdeaValidationAttempt>)context.AdditionalProperties![IdeaValidationLoopEvaluator.ObserverKey]!)(
            attempt);

    private static Func<string?, Cli.StageTracker, ServiceProvider> Services(
        Func<AgentExecutionContext, IReadOnlyList<Idea>> idea) =>
        (_, _) => new ServiceCollection()
            .AddSingleton<IAgent<StoryInput, IReadOnlyList<Idea>>>(new ContextAgent(idea))
            .BuildServiceProvider();

    private static async Task<(int Code, string Out, string Error)> RunIdea(TempDirectory directory,
        Func<string?, Cli.StageTracker, ServiceProvider> services)
    {
        await File.WriteAllTextAsync(directory.Input, """{"content":"source"}""");
        return await InvokeWith(services, "run", "idea", "--input", directory.Input, "--output", directory.Output,
            "--trace", directory.Trace);
    }

    private static void CreatePromptSet(string directory)
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

    private static Task<(int Code, string Out, string Error)> InvokeAll(params string[] args) =>
        InvokeWith(null, args);

    private static async Task<(int Code, string Out, string Error)> InvokeWith(
        Func<string?, Cli.StageTracker, ServiceProvider>? services, params string[] args)
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        await using var output = new StringWriter();
        await using var error = new StringWriter();
        System.Console.SetOut(output);
        System.Console.SetError(error);
        try
        {
            return (await Cli.RunAsync(args, services), output.ToString(), error.ToString());
        }
        finally
        {
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
        }
    }

    private sealed class TempDirectory : IDisposable
    {
        public TempDirectory() => Directory.CreateDirectory(Path);

        public string Path { get; } =
            System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"nueyon-cli-test-{Guid.NewGuid():N}");

        public string Input => System.IO.Path.Combine(Path, "input.json");
        public string Output => System.IO.Path.Combine(Path, "result.json");
        public string Trace => System.IO.Path.Combine(Path, "trace.json");

        public void Dispose() => Directory.Delete(Path, true);
    }

    private sealed class ContextAgent(Func<AgentExecutionContext, IReadOnlyList<Idea>> func)
        : IAgent<StoryInput, IReadOnlyList<Idea>>
    {
        public Task<IReadOnlyList<Idea>> ExecuteAsync(AgentExecutionContext executionContext, StoryInput input,
            CancellationToken cancellationToken = default) => Task.FromResult(func(executionContext));
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
