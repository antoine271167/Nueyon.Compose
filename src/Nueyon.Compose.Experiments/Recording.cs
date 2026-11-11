using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Nueyon.Compose.Application.Agents.Idea;

namespace Nueyon.Compose.Experiments;

public sealed record CellState(string CaseId, string Condition, int Replicate)
{
    public string Key => $"{CaseId}|{Condition}|{Replicate}";
    public string DirectoryPath(string root) => Path.Combine(root, "runs", CaseId, Replicate.ToString(), Condition);
    public string StartedPath(string root) => Path.Combine(DirectoryPath(root), "started.json");
    public string FinalPath(string root) => Path.Combine(DirectoryPath(root), "final.json");
}

public sealed record Reservation(int Ordinal, string Cell, DateTimeOffset Utc);

public sealed class CallLedger
{
    public const int Limit = 112;
    private readonly string _path;
    private readonly List<Reservation> _reservations = [];
    private readonly object _sync = new();

    public CallLedger(string path)
    {
        _path = path;
        if (!File.Exists(path)) return;
        foreach (var line in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) throw new InvalidDataException("Blank ledger entry.");
            var entry = JsonSerializer.Deserialize<Reservation>(line, ExperimentJson.Options)
                        ?? throw new InvalidDataException("Invalid ledger entry.");
            if (entry.Ordinal != _reservations.Count + 1 || string.IsNullOrWhiteSpace(entry.Cell))
                throw new InvalidDataException("Invalid ledger sequence.");
            _reservations.Add(entry);
        }
        if (_reservations.Count > Limit) throw new InvalidDataException("Ledger exceeds budget.");
    }

    public int Count(CellState cell) => _reservations.Count(r => r.Cell == cell.Key);

    public Reservation Reserve(CellState cell)
    {
        lock (_sync)
        {
            var cap = cell.Condition switch { "A" => 6, "B" or "C" => 1, _ => throw new ArgumentException("Invalid condition.") };
            if (_reservations.Count >= Limit || Count(cell) >= cap)
                throw new InvalidOperationException("Call budget exhausted.");
            var entry = new Reservation(_reservations.Count + 1, cell.Key, DateTimeOffset.UtcNow);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entry, new JsonSerializerOptions(ExperimentJson.Options) { WriteIndented = false }) + "\n");
            using var stream = new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough);
            stream.Write(bytes);
            stream.Flush(true);
            _reservations.Add(entry);
            return entry;
        }
    }
}

public sealed class RecordingChatClient(IChatClient inner, CallLedger ledger, CellState cell, string root) : IChatClient
{
    public long TotalTokens { get; private set; }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var sent = messages.ToArray();
        var actualOptions = options?.Clone() ?? new ChatOptions();
        actualOptions.MaxOutputTokens = 6000;
        actualOptions.Temperature = null;
        var reservation = ledger.Reserve(cell);
        var dir = Path.Combine(cell.DirectoryPath(root), $"call-{reservation.Ordinal:D3}");
        Directory.CreateDirectory(dir);
        var recordedOptions = actualOptions.Clone();
        if (actualOptions.AdditionalProperties is not null)
        {
            recordedOptions.AdditionalProperties = new AdditionalPropertiesDictionary();
            foreach (var (key, value) in actualOptions.AdditionalProperties)
                if (key != IdeaValidationLoopEvaluator.ObserverKey)
                    recordedOptions.AdditionalProperties[key] = value;
        }
        DurableFile.Write(Path.Combine(dir, "request.json"),
            JsonSerializer.Serialize(new { messages = sent, options = recordedOptions }, ExperimentJson.Options));
        var started = DateTimeOffset.UtcNow;
        var clock = Stopwatch.StartNew();
        try
        {
            var response = await inner.GetResponseAsync(sent, actualOptions, cancellationToken);
            clock.Stop();
            DurableFile.Write(Path.Combine(dir, "response.txt"), response.Text);
            DurableFile.Write(Path.Combine(dir, "metadata.json"), JsonSerializer.Serialize(new
            {
                reservation.Ordinal, startedUtc = started, completedUtc = DateTimeOffset.UtcNow,
                durationMs = clock.ElapsedMilliseconds, response.ResponseId, response.ModelId,
                response.Usage, finishReason = response.FinishReason?.ToString()
            }, ExperimentJson.Options));
            TotalTokens += response.Usage?.TotalTokenCount ?? 0;
            return response;
        }
        catch (Exception ex)
        {
            clock.Stop();
            DurableFile.Write(Path.Combine(dir, "error.json"), JsonSerializer.Serialize(new
            {
                reservation.Ordinal, startedUtc = started, completedUtc = DateTimeOffset.UtcNow,
                durationMs = clock.ElapsedMilliseconds, type = ex.GetType().FullName, error = ex.Message
            }, ExperimentJson.Options));
            throw;
        }
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,
        ChatOptions? options = null, CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Streaming is not supported by the experiment recorder.");
    public object? GetService(Type serviceType, object? serviceKey = null) => inner.GetService(serviceType, serviceKey);
    public void Dispose() { }
}

public static class DurableFile
{
    public static void Write(string path, string content)
    {
        using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough);
        var bytes = Encoding.UTF8.GetBytes(content);
        stream.Write(bytes);
        stream.Flush(true);
    }

    public static void WriteFinal(string path, string content)
    {
        var temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        Write(temp, content);
        File.Move(temp, path);
    }
}
