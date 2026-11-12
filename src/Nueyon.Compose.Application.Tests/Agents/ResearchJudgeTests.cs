using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

public sealed class ResearchJudgeTests
{
    private const string Source = "A proposal, with no measured results.";
    private const string Accepted = """{"accepted":true,"blockingIssues":[]}""";
    private const string Content = "### Facts\n1. A proposal.\n\n### Interpretations\nNone\n\n### Unknowns\nNo measured results.\n\n### Development sequence\nNone\n\n### Editorial relevance\nFact 1.";
    private static readonly Idea Idea = new("Proposal", "Explain proposal", "Readers", "Explain", Source);
    private static ResearchInput Input => new(new StoryInput(Source), new SelectedIdea(Idea));
    private static string Generation(string content = Content) => JsonSerializer.Serialize(new ResearchResult(content));
    private static string Rejected(string quote = Source, string category = "epistemicAccuracy") =>
        JsonSerializer.Serialize(new
        {
            accepted = false, blockingIssues = new[] { new
            {
                category, researchLocation = "Facts 1", problem = "Claims measured results.",
                sourceQuote = quote, constraint = "Preserve proposal status; outcomes are not measured."
            }}
        });
    private static ResearchAgent Create(FakeChatClient generator, FakeChatClient judge) =>
        new(generator.AsAIAgent("Generate", "Research"), NullLogger<ResearchAgent>.Instance,
            new ResearchSemanticJudge(judge.AsAIAgent("Judge", "ResearchJudge")));

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task AcceptedResult_HasBoundedCalls_AndExactFilteredJudgeInput(bool repair)
    {
        var generator = new FakeChatClient(Generation(), Generation(Content + "\nRepaired"));
        var judge = repair ? new FakeChatClient(Rejected(), Accepted) : new FakeChatClient(Accepted);
        var result = await Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input);
        Assert.Equal(repair ? Content + "\nRepaired" : Content, result.Content);
        Assert.Equal(repair ? 2 : 1, generator.InvocationCount);
        Assert.Equal(generator.InvocationCount, judge.InvocationCount);
        for (var index = 0; index < judge.InvocationCount; index++)
        {
            var messages = judge.Requests[index];
            Assert.DoesNotContain(messages, m => m.Role == ChatRole.Assistant);
            using var data = JsonDocument.Parse(Assert.Single(messages, m => m.Role == ChatRole.User).Text!);
            Assert.Equal(Source, data.RootElement.GetProperty("source").GetString());
            Assert.Equal(Idea.Title, data.RootElement.GetProperty("selectedIdea").GetProperty("title").GetString());
            var research = data.RootElement.GetProperty("research").GetProperty("content").GetString()!;
            Assert.Equal(SynthesisResearchFilter.ExtractFactsAndUnknowns(research), data.RootElement.GetProperty("synthesisInput").GetString());
            Assert.NotNull(judge.Options[index]?.ResponseFormat);
        }
        if (repair)
        {
            var original = Assert.Single(generator.Requests[0], m => m.Role == ChatRole.User).Text!;
            var revised = Assert.Single(generator.Requests[1], m => m.Role == ChatRole.User).Text!;
            Assert.StartsWith(original, revised);
            Assert.Contains("Claims measured results.", revised);
            Assert.DoesNotContain(generator.Requests[1], m => m.Role == ChatRole.Assistant);
        }
    }

    [Fact]
    public async Task RejectedTwice_ThrowsTypedFailure()
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = new FakeChatClient(Rejected(), Rejected());
        var ex = await Assert.ThrowsAsync<ResearchValidationException>(() =>
            Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.Equal(2, ex.AttemptCount);
        Assert.Equal(ResearchValidationFailureKind.Semantic, ex.Kind);
        Assert.Single(ex.Blockers);
        Assert.DoesNotContain(Source, ex.Message);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(2, judge.InvocationCount);
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("null")]
    [InlineData("{}")]
    [InlineData("{\"accepted\":true,\"blockingIssues\":null}")]
    [InlineData("{\"accepted\":false,\"blockingIssues\":[]}")]
    [InlineData("{\"accepted\":true,\"accepted\":false,\"blockingIssues\":[]}")]
    [InlineData("{\"accepted\":true,\"blockingIssues\":[],\"extra\":1}")]
    public async Task InvalidJudgeResponse_StopsWithoutRepair(string verdict)
    {
        var generator = new FakeChatClient(Generation());
        var judge = new FakeChatClient(verdict);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.IsNotType<ResearchValidationException>(ex);
        Assert.Equal(1, generator.InvocationCount);
    }

    [Theory]
    [InlineData("Invented quote", "epistemicAccuracy")]
    [InlineData(Source, "unknownCategory")]
    [InlineData("", "epistemicAccuracy")]
    public async Task InvalidDiagnosis_IsNotRepairEvidence(string quote, string category)
    {
        var generator = new FakeChatClient(Generation());
        await Assert.ThrowsAsync<InvalidOperationException>(() => Create(generator,
            new FakeChatClient(Rejected(quote, category))).ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.Equal(1, generator.InvocationCount);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task StructuralFailure_UsesOnlyOneRepair(bool recover)
    {
        var generator = new FakeChatClient("not json", recover ? Generation() : "null");
        var judge = new FakeChatClient(Accepted);
        if (recover)
            await Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input);
        else
        {
            var ex = await Assert.ThrowsAsync<ResearchValidationException>(() =>
                Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input));
            Assert.Equal(ResearchValidationFailureKind.Structural, ex.Kind);
        }
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(recover ? 1 : 0, judge.InvocationCount);
    }

    [Fact]
    public async Task CancellationDuringJudge_DoesNotRepair()
    {
        using var cts = new CancellationTokenSource();
        var generator = new FakeChatClient(Generation());
        var judge = new FakeChatClient(Accepted) { OnGetResponse = token => { cts.Cancel(); token.ThrowIfCancellationRequested(); } };
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input, cts.Token));
        Assert.Equal(1, generator.InvocationCount);
    }

    [Theory]
    [InlineData("### Facts\nA proposal.")]
    [InlineData(Content + "\n### Facts\nDuplicate")]
    public async Task InvalidHeadings_AreRepairedBeforeJudge(string content)
    {
        var generator = new FakeChatClient(Generation(content), Generation());
        var judge = new FakeChatClient(Accepted);
        var result = await Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input);
        Assert.Equal(Content, result.Content);
        Assert.Equal(2, generator.InvocationCount);
        Assert.Equal(1, judge.InvocationCount);
    }

    [Fact]
    public async Task TooManyBlockers_IsTechnicalFailure()
    {
        using var parsed = JsonDocument.Parse(Rejected());
        var issue = parsed.RootElement.GetProperty("blockingIssues")[0].Clone();
        var response = JsonSerializer.Serialize(new { accepted = false, blockingIssues = Enumerable.Repeat(issue, 6) });
        var generator = new FakeChatClient(Generation());
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Create(generator, new FakeChatClient(response)).ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.Equal(1, generator.InvocationCount);
    }

    [Fact]
    public async Task JudgeTransportFailure_IsNotSemanticRejection()
    {
        var generator = new FakeChatClient(Generation());
        var judge = new FakeChatClient(Accepted) { OnGetResponse = _ => throw new HttpRequestException("Test failure") };
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            Create(generator, judge).ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.Equal(1, generator.InvocationCount);
    }

    [Fact]
    public async Task ConcurrentRuns_DoNotShareRepairState()
    {
        var generator = new FakeChatClient(Generation(), Generation(), Generation(), Generation());
        var judge = new FakeChatClient(Rejected(), Accepted, Accepted, Accepted);
        var agent = Create(generator, judge);
        var results = await Task.WhenAll(
            agent.ExecuteAsync(new(Guid.NewGuid()), Input),
            agent.ExecuteAsync(new(Guid.NewGuid()), Input));
        Assert.All(results, result => Assert.Equal(Content, result.Content));
        Assert.Equal(3, generator.InvocationCount);
        Assert.Equal(3, judge.InvocationCount);
        Assert.Equal(1, generator.Requests.Count(request => request.Any(m => m.Text?.Contains("RESEARCH REPAIR", StringComparison.Ordinal) == true)));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RealWorkflow_OnlyHandsAcceptedResearchToSynthesis(bool accepted)
    {
        var generator = new FakeChatClient(Generation(), Generation());
        var judge = accepted ? new FakeChatClient(Accepted) : new FakeChatClient(Rejected(), Rejected());
        var synthesis = new CountingAgent<SynthesisInput, SynthesisResult>(new("synthesis"));
        var narrative = new CountingAgent<NarrativeInput, NarrativeResult>(new("narrative"));
        var compose = new CountingAgent<ComposeInput, ComposeResult>(new("compose"));
        var workflow = new StoryWorkflow(new CountingAgent<StoryInput, IReadOnlyList<Idea>>([Idea]),
            Create(generator, judge), synthesis, narrative, compose);
        if (accepted) await workflow.RunAsync(Input.Input);
        else
        {
            var ex = await Record.ExceptionAsync(() => workflow.RunAsync(Input.Input));
            Assert.NotNull(ex);
            Assert.IsType<ResearchValidationException>(ex);
            Assert.Contains("Research validation failed after 2 attempts", ex.ToString());
        }
        Assert.Equal(accepted ? 1 : 0, synthesis.Calls);
        Assert.Equal(synthesis.Calls, narrative.Calls);
        Assert.Equal(synthesis.Calls, compose.Calls);
        if (accepted) Assert.Equal(SynthesisResearchFilter.ExtractFactsAndUnknowns(Content), synthesis.Input!.Research.Content);
    }

    private sealed class CountingAgent<TInput, TOutput>(TOutput result) : IAgent<TInput, TOutput>
    {
        public int Calls { get; private set; }
        public TInput? Input { get; private set; }
        public Task<TOutput> ExecuteAsync(AgentExecutionContext executionContext, TInput input, CancellationToken cancellationToken = default)
        {
            Calls++;
            Input = input;
            return Task.FromResult(result);
        }
    }
}
