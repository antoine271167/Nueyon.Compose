namespace Nueyon.Compose.Application.Agents.Synthesis;

/// <summary>
///     Deterministically extracts only the Facts and Unknowns sections from Research content,
///     so that the Synthesizer agent no longer sees Interpretations, Development Sequence, or
///     Editorial Relevance.
/// </summary>
public static class SynthesisResearchFilter
{
    private const string FactsHeading = "### Facts";
    private const string UnknownsHeading = "### Unknowns";

    private static readonly string[] _allHeadings =
    [
        "### Facts",
        "### Interpretations",
        "### Unknowns",
        "### Development sequence",
        "### Editorial relevance"
    ];

    /// <summary>
    ///     Extracts the Facts and Unknowns sections from the supplied Research content,
    ///     preserving their original text exactly. Sections that are not present are omitted.
    /// </summary>
    public static string ExtractFactsAndUnknowns(string researchContent)
    {
        ArgumentNullException.ThrowIfNull(researchContent);

        var facts = ExtractSection(researchContent, FactsHeading);
        var unknowns = ExtractSection(researchContent, UnknownsHeading);

        var sections = new List<string>();

        if (facts is not null)
        {
            sections.Add($"### Facts{Environment.NewLine}{Environment.NewLine}{facts}");
        }

        if (unknowns is not null)
        {
            sections.Add($"### Unknowns{Environment.NewLine}{Environment.NewLine}{unknowns}");
        }

        return string.Join($"{Environment.NewLine}{Environment.NewLine}", sections);
    }

    private static string? ExtractSection(string content, string heading)
    {
        var headingIndex = content.IndexOf(heading, StringComparison.Ordinal);
        if (headingIndex < 0)
        {
            return null;
        }

        var contentStart = headingIndex + heading.Length;

        var nextHeadingIndex = -1;
        foreach (var otherHeading in _allHeadings)
        {
            if (otherHeading == heading)
            {
                continue;
            }

            var index = content.IndexOf(otherHeading, contentStart, StringComparison.Ordinal);
            if (index >= 0 && (nextHeadingIndex < 0 || index < nextHeadingIndex))
            {
                nextHeadingIndex = index;
            }
        }

        var sectionEnd = nextHeadingIndex >= 0 ? nextHeadingIndex : content.Length;

        var section = content[contentStart..sectionEnd];

        return section.Trim('\r', '\n').TrimEnd();
    }
}