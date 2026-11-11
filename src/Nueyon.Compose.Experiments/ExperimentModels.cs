using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Nueyon.Compose.Experiments;

public static class ExperimentJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        AllowDuplicateProperties = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        PropertyNameCaseInsensitive = false
    };
}

public sealed record ExperimentIdea
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Audience { get; init; }
    public required string Rationale { get; init; }
    public required string Evidence { get; init; }
}

public sealed record GroundingRecord
{
    public required string Field { get; init; }
    public required string Claim { get; init; }
    public required string SourceEvidence { get; init; }
    public required string Inference { get; init; }
    public required string Constraints { get; init; }
}

public sealed record ReviewIssue
{
    public required string Field { get; init; }
    public required string Issue { get; init; }
    public required string SourceEvidence { get; init; }
    public required string Action { get; init; }
}

public sealed record ExperimentIdeaResult
{
    public required List<ExperimentIdea> Ideas { get; init; }
    public required List<GroundingRecord> Grounding { get; init; }
}

public sealed record ReviewResult
{
    public required List<ReviewIssue> Review { get; init; }
    public required ExperimentIdeaResult Revised { get; init; }
}

public static class ExperimentValidator
{
    private static readonly string[] Fields = ["title", "description", "audience", "rationale", "evidence"];

    public static ExperimentIdeaResult ParseDraft(string json, string source)
    {
        var result = JsonSerializer.Deserialize<ExperimentIdeaResult>(json, ExperimentJson.Options)
                     ?? throw new InvalidDataException("Null draft.");
        Validate(result, source);
        return result;
    }

    public static ReviewResult ParseReview(string json, string source)
    {
        var result = JsonSerializer.Deserialize<ReviewResult>(json, ExperimentJson.Options)
                     ?? throw new InvalidDataException("Null review.");
        if (result.Review is null || result.Revised is null)
            throw new InvalidDataException("Missing review or revised result.");
        foreach (var item in result.Review)
        {
            if (item is null || !Fields.Contains(item.Field) || !NonBlank(item.Issue) ||
                !Quote(item.SourceEvidence, source) || item.Action is not ("keep" or "revise"))
                throw new InvalidDataException("Invalid review record.");
        }
        Validate(result.Revised, source);
        return result;
    }

    private static void Validate(ExperimentIdeaResult result, string source)
    {
        if (result.Ideas is not { Count: >= 1 and <= 3 } ||
            result.Grounding is not { Count: >= 1 and <= 12 })
            throw new InvalidDataException("Invalid ideas or grounding cardinality.");
        foreach (var idea in result.Ideas)
            if (idea is null || !NonBlank(idea.Title) || !NonBlank(idea.Description) ||
                !NonBlank(idea.Audience) || !NonBlank(idea.Rationale) || !NonBlank(idea.Evidence))
                throw new InvalidDataException("Invalid idea fields.");
        foreach (var item in result.Grounding)
            if (item is null || !Fields.Contains(item.Field) || !NonBlank(item.Claim) ||
                !Quote(item.SourceEvidence, source) || !NonBlank(item.Inference) || !NonBlank(item.Constraints))
                throw new InvalidDataException("Invalid grounding record.");
        if (Fields.Except(result.Grounding.Select(g => g.Field), StringComparer.Ordinal).Any())
            throw new InvalidDataException("All first idea fields require grounding.");
    }

    private static bool NonBlank(string? value) => !string.IsNullOrWhiteSpace(value);
    private static bool Quote(string? value, string source) =>
        NonBlank(value) && source.Contains(value!, StringComparison.Ordinal);

    private static JsonObject Object(params (string Name, JsonNode Schema)[] fields)
    {
        var props = new JsonObject();
        var required = new JsonArray();
        foreach (var (name, schema) in fields) { props.Add(name, schema); required.Add(name); }
        return new JsonObject { ["type"] = "object", ["properties"] = props, ["required"] = required, ["additionalProperties"] = false };
    }

    private static JsonObject Text() => new() { ["type"] = "string", ["minLength"] = 1 };
    private static JsonObject Choice(params string[] values) =>
        new() { ["type"] = "string", ["enum"] = new JsonArray(values.Select(v => (JsonNode?)JsonValue.Create(v)).ToArray()) };
    private static JsonObject Array(JsonNode items, int min, int? max = null)
    {
        var node = new JsonObject { ["type"] = "array", ["items"] = items, ["minItems"] = min };
        if (max is not null) node["maxItems"] = max.Value;
        return node;
    }
    private static JsonObject DraftSchema() => Object(
        ("ideas", Array(Object(("title", Text()), ("description", Text()), ("audience", Text()),
            ("rationale", Text()), ("evidence", Text())), 1, 3)),
        ("grounding", Array(Object(("field", Choice(Fields)), ("claim", Text()), ("sourceEvidence", Text()),
            ("inference", Text()), ("constraints", Text())), 1, 12)));

    public static JsonElement Schema(bool review)
    {
        JsonNode node = review
            ? Object(("review", Array(Object(("field", Choice(Fields)), ("issue", Text()),
                ("sourceEvidence", Text()), ("action", Choice("keep", "revise"))), 0)),
                ("revised", DraftSchema()))
            : DraftSchema();
        return JsonSerializer.SerializeToElement(node);
    }
}
