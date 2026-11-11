using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;

#pragma warning disable MAAI001

namespace Nueyon.Compose.Experiments;

public sealed record CaseManifest(string Id, string Split, string Source);
public sealed record FrozenMetadata(Dictionary<string, string> Files, string Head, int Budget);

public static class FreezeVerifier
{
    public static void Verify(string root)
    {
        var path = Path.Combine(root, "verification", "frozen.json");
        var options = new JsonSerializerOptions(ExperimentJson.Options) { UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip };
        var freeze = JsonSerializer.Deserialize<FrozenMetadata>(File.ReadAllText(path), options)
                     ?? throw new InvalidDataException("Invalid freeze.");
        if (freeze.Budget != CallLedger.Limit || freeze.Files is not { Count: > 0 } || string.IsNullOrWhiteSpace(freeze.Head))
            throw new InvalidDataException("Invalid freeze metadata.");
        foreach (var (relative, expected) in freeze.Files)
        {
            if (Path.IsPathRooted(relative)) throw new InvalidDataException("Frozen paths must be relative.");
            var actual = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.GetFullPath(relative, root)))).ToLowerInvariant();
            if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException($"Frozen file changed: {relative}");
        }
        var start = new ProcessStartInfo("git") { WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
        start.ArgumentList.Add("rev-parse"); start.ArgumentList.Add("HEAD");
        using var git = Process.Start(start) ?? throw new InvalidOperationException("Could not start git.");
        var head = git.StandardOutput.ReadToEnd().Trim();
        var error = git.StandardError.ReadToEnd();
        git.WaitForExit();
        if (git.ExitCode != 0 || !string.Equals(head, freeze.Head, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException($"Git HEAD differs from freeze: {error}");
    }

    public static void VerifyBaseline(string root)
    {
        var assembly = typeof(IdeaAgent).Assembly;
        var prefix = "Nueyon.Compose.Application.Prompts.Idea.";
        // The experiment baseline is the Legacy set; the Simplified prompts live in a subdirectory.
        var resources = assembly.GetManifestResourceNames().Where(n => n.StartsWith(prefix, StringComparison.Ordinal) && !n.StartsWith(prefix + "Simplified.", StringComparison.Ordinal) && n.EndsWith(".prompt.txt", StringComparison.Ordinal)).ToArray();
        if (resources.Length == 0) throw new InvalidDataException("No embedded Idea prompts.");
        var dir = Path.Combine(root, "prompts", "baseline", "Idea");
        var files = Directory.GetFiles(dir, "*.prompt.txt");
        if (files.Length != resources.Length) throw new InvalidDataException("Baseline prompt set differs from embedded prompts.");
        foreach (var resource in resources)
        {
            using var stream = assembly.GetManifestResourceStream(resource) ?? throw new InvalidDataException("Missing embedded prompt.");
            using var buffer = new MemoryStream(); stream.CopyTo(buffer);
            if (!buffer.ToArray().AsSpan().SequenceEqual(File.ReadAllBytes(Path.Combine(dir, resource[prefix.Length..]))))
                throw new InvalidDataException($"Baseline prompt differs: {resource}");
        }
    }
}

public sealed class ExperimentRunner(string root, IChatClient client, CallLedger ledger)
{
    private readonly string _root = Path.GetFullPath(root);

    public async Task<string> RunAsync(CellState cell, bool verifyFreeze = true, CancellationToken cancellationToken = default)
    {
        if (!Regex.IsMatch(cell.CaseId, @"\A[A-Za-z0-9][A-Za-z0-9_-]*\z") ||
            cell.Condition is not ("A" or "B" or "C") || cell.Replicate < 1)
            throw new ArgumentException("Invalid cell identifier.");
        if (verifyFreeze) { FreezeVerifier.Verify(_root); FreezeVerifier.VerifyBaseline(_root); }
        var cases = JsonSerializer.Deserialize<List<CaseManifest>>(File.ReadAllText(Path.Combine(_root, "cases.json")), ExperimentJson.Options)
                    ?? throw new InvalidDataException("Invalid cases.");
        var selected = cases.Single(c => c.Id == cell.CaseId);
        var sourcePath = Path.GetFullPath(selected.Source, _root);
        if (Path.IsPathRooted(selected.Source) || !sourcePath.StartsWith(_root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Source must be inside experiment root.");
        var source = File.ReadAllText(sourcePath);
        var dir = cell.DirectoryPath(_root);
        if (File.Exists(cell.FinalPath(_root))) return "skipped";
        if (File.Exists(cell.StartedPath(_root))) throw new InvalidOperationException("Started cell cannot be retried automatically.");
        Directory.CreateDirectory(dir);
        var started = DateTimeOffset.UtcNow;
        DurableFile.Write(cell.StartedPath(_root), JsonSerializer.Serialize(new { cell, startedUtc = started }, ExperimentJson.Options));
        var clock = Stopwatch.StartNew();
        using var recorder = new RecordingChatClient(client, ledger, cell, _root);
        object? ideas = null, grounding = null, review = null, revised = null;
        string? error = null;
        var status = "success";
        Exception? failure = null;
        try
        {
            switch (cell.Condition)
            {
                case "A":
                    var attempts = new List<IdeaValidationAttempt>();
                    Action<IdeaValidationAttempt> observer = attempt =>
                    {
                        attempts.Add(attempt);
                        var line = JsonSerializer.Serialize(attempt, new JsonSerializerOptions(ExperimentJson.Options) { WriteIndented = false }) + "\n";
                        using var stream = new FileStream(Path.Combine(dir, "attempts.jsonl"), FileMode.Append, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough);
                        var bytes = System.Text.Encoding.UTF8.GetBytes(line); stream.Write(bytes); stream.Flush(true);
                    };
                    var judge = new IdeaSemanticJudge(recorder.AsAIAgent(IdeaSemanticJudge.GetSystemInstructions(), "OpenAIAgent"));
                    var evaluator = new IdeaValidationLoopEvaluator(new IdeaValidator(), judge);
                    var generator = recorder.AsAIAgent(IdeaAgent.GetSystemInstructions(), "OpenAIAgent");
                    var agent = new IdeaAgent(evaluator.Wrap(generator), NullLogger<IdeaAgent>.Instance);
                    var context = new AgentExecutionContext(Guid.NewGuid()) { AdditionalProperties = new Dictionary<string, object?> { [IdeaValidationLoopEvaluator.ObserverKey] = observer } };
                    ideas = await agent.ExecuteAsync(context, new StoryInput(source), cancellationToken);
                    break;
                case "B":
                    var draftText = await RunPrompt(recorder, "evidence", source, null, false, cancellationToken);
                    DurableFile.Write(Path.Combine(dir, "draft.json"), draftText);
                    var draft = ExperimentValidator.ParseDraft(draftText, source);
                    ideas = draft.Ideas; grounding = draft.Grounding;
                    break;
                case "C":
                {
                    var b = new CellState(cell.CaseId, "B", cell.Replicate);
                    if (!File.Exists(b.FinalPath(_root)) || !File.Exists(Path.Combine(b.DirectoryPath(_root), "draft.json")))
                    {
                        status = "blocked"; error = "A successful B draft is required."; break;
                    }
                    using var bFinal = JsonDocument.Parse(File.ReadAllText(b.FinalPath(_root)));
                    if (bFinal.RootElement.GetProperty("status").GetString() != "success")
                    {
                        status = "blocked"; error = "A successful B draft is required."; break;
                    }
                    var original = File.ReadAllText(Path.Combine(b.DirectoryPath(_root), "draft.json"));
                    try { ExperimentValidator.ParseDraft(original, source); }
                    catch (Exception ex) when (ex is JsonException or InvalidDataException)
                    {
                        status = "blocked"; error = $"B draft is invalid: {ex.Message}"; break;
                    }
                    DurableFile.Write(Path.Combine(dir, "draft.json"), original);
                    var reviewText = await RunPrompt(recorder, "review", source, original, true, cancellationToken);
                    DurableFile.Write(Path.Combine(dir, "reviewed.json"), reviewText);
                    var result = ExperimentValidator.ParseReview(reviewText, source);
                    ideas = result.Revised.Ideas; grounding = result.Revised.Grounding;
                    review = result.Review; revised = result.Revised;
                    break;
                }
            }
        }
        catch (Exception ex) { status = "failed"; error = ex.ToString(); failure = ex; }
        clock.Stop();
        DurableFile.WriteFinal(cell.FinalPath(_root), JsonSerializer.Serialize(new
        {
            status, ideas, grounding, review, revised, error, callCount = ledger.Count(cell),
            totalTokensUsed = recorder.TotalTokens, durationMs = clock.ElapsedMilliseconds,
            startedUtc = started, completedUtc = DateTimeOffset.UtcNow
        }, ExperimentJson.Options));
        if (failure is not null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
        return status;
    }

    private async Task<string> RunPrompt(IChatClient client, string name, string source, string? draft,
        bool review, CancellationToken cancellationToken)
    {
        var system = File.ReadAllText(Path.Combine(_root, "prompts", name + ".system.txt"));
        var template = File.ReadAllText(Path.Combine(_root, "prompts", name + ".user.txt"));
        var replacements = new Dictionary<string, string> { ["SourceContent"] = source };
        if (draft is not null) replacements["Draft"] = draft;
        var seen = new HashSet<string>();
        var user = Regex.Replace(template, @"\{\{([A-Za-z]+)\}\}", match =>
        {
            var key = match.Groups[1].Value;
            if (!replacements.TryGetValue(key, out var value)) throw new InvalidDataException($"Unknown prompt token: {key}");
            seen.Add(key); return value;
        });
        if (!seen.SetEquals(replacements.Keys)) throw new InvalidDataException("Missing prompt token.");
        var schema = ExperimentValidator.Schema(review);
        var options = new ChatClientAgentRunOptions(new ChatOptions { ResponseFormat = ChatResponseFormat.ForJsonSchema(schema, review ? "ReviewResult" : "ExperimentIdeaResult") });
        var agent = client.AsAIAgent(system, "OpenAIAgent");
        return (await agent.RunAsync(user, null, options, cancellationToken)).Text;
    }
}
