using Nueyon.Compose.Application.Agents.Synthesis;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
///     Tests for the deterministic extraction of Facts and Unknowns from Research content
///     performed at the Synthesis boundary.
/// </summary>
public sealed class SynthesisResearchFilterTests
{
    [Fact]
    public void ExtractFactsAndUnknowns_WithAllSections_IncludesOnlyFactsAndUnknowns()
    {
        // Arrange
        const string research =
            """
            ### Facts

            Fact one. Fact two.

            ### Interpretations

            Interpretation one.

            ### Unknowns

            Unknown one.

            ### Development sequence

            Sequence one.

            ### Editorial relevance

            Facts 1 and 2 are relevant to the Selected Idea.
            """;

        // Act
        var result = SynthesisResearchFilter.ExtractFactsAndUnknowns(research);

        // Assert
        Assert.Contains("Fact one. Fact two.", result, StringComparison.Ordinal);
        Assert.Contains("Unknown one.", result, StringComparison.Ordinal);
        Assert.DoesNotContain("Interpretation one.", result, StringComparison.Ordinal);
        Assert.DoesNotContain("Sequence one.", result, StringComparison.Ordinal);
        Assert.DoesNotContain("Facts 1 and 2 are relevant", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ExtractFactsAndUnknowns_WithAllSections_PreservesSectionHeadings()
    {
        // Arrange
        const string research =
            """
            ### Facts

            Fact one.

            ### Interpretations

            Interpretation one.

            ### Unknowns

            Unknown one.

            ### Development sequence

            Sequence one.

            ### Editorial relevance

            Relevance one.
            """;

        // Act
        var result = SynthesisResearchFilter.ExtractFactsAndUnknowns(research);

        // Assert
        Assert.Contains("### Facts", result, StringComparison.Ordinal);
        Assert.Contains("### Unknowns", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ExtractFactsAndUnknowns_WithOnlyFacts_PreservesFactsAndOmitsUnknowns()
    {
        // Arrange
        const string research =
            """
            ### Facts

            Fact one. Fact two.
            """;

        // Act
        var result = SynthesisResearchFilter.ExtractFactsAndUnknowns(research);

        // Assert
        Assert.Contains("### Facts", result, StringComparison.Ordinal);
        Assert.Contains("Fact one. Fact two.", result, StringComparison.Ordinal);
        Assert.DoesNotContain("### Unknowns", result, StringComparison.Ordinal);
    }

    [Fact]
    public void ExtractFactsAndUnknowns_WithMultilineContent_PreservesTextExactly()
    {
        // Arrange
        const string research =
            """
            ### Facts

            Fact one.
            Fact two spans
            multiple lines.

            - Fact three as a bullet.

            ### Unknowns

            Unknown one.
            Unknown two spans
            multiple lines.
            """;

        // Act
        var result = SynthesisResearchFilter.ExtractFactsAndUnknowns(research);

        // Assert
        var expectedFacts = string.Join(
            Environment.NewLine,
            "Fact one.",
            "Fact two spans",
            "multiple lines.",
            string.Empty,
            "- Fact three as a bullet.");
        var expectedUnknowns = string.Join(
            Environment.NewLine,
            "Unknown one.",
            "Unknown two spans",
            "multiple lines.");

        Assert.Contains("### Facts", result, StringComparison.Ordinal);
        Assert.Contains("### Unknowns", result, StringComparison.Ordinal);
        Assert.Contains(expectedFacts, result, StringComparison.Ordinal);
        Assert.Contains(expectedUnknowns, result, StringComparison.Ordinal);
    }
}