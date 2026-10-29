using Nueyon.Compose.Application.Prompts;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Prompts;

public class PromptLoaderTests
{
    public static IEnumerable<object[]> AllPrompts =>
    new[]
    {
        new object[] { "Idea/Idea.system.prompt.txt" },
        new object[] { "Idea/Idea.user.prompt.txt" },
        new object[] { "Research/Research.system.prompt.txt" },
        new object[] { "Research/Research.user.prompt.txt" },
        new object[] { "Synthesis/Synthesis.system.prompt.txt" },
        new object[] { "Synthesis/Synthesis.user.prompt.txt" },
        new object[] { "Narrative/Narrative.system.prompt.txt" },
        new object[] { "Narrative/Narrative.user.prompt.txt" },
        new object[] { "Compose/Compose.system.prompt.txt" },
        new object[] { "Compose/Compose.article.user.prompt.txt" },
        new object[] { "Compose/Compose.linkedin.user.prompt.txt" }
    };

    [Theory]
    [MemberData(nameof(AllPrompts))]
    public void Load_ExistingPrompt_ReturnsNonEmptyText(string path)
    {
        Assert.False(string.IsNullOrWhiteSpace(PromptLoader.Load(path)));
    }

    [Fact]
    public void Load_BackslashSeparatedPath_ReturnsSameTextAsForwardSlash()
    {
        Assert.Equal(
            PromptLoader.Load("Idea/Idea.system.prompt.txt"),
            PromptLoader.Load("Idea\\Idea.system.prompt.txt"));
    }

    [Fact]
    public void Load_MissingPrompt_ThrowsWithPromptPath()
    {
        var ex = Assert.Throws<FileNotFoundException>(() => PromptLoader.Load("Idea/Missing.prompt.txt"));

        Assert.Contains("Idea/Missing.prompt.txt", ex.Message);
    }

    [Fact]
    public void Render_MissingTokenValue_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.txt"));

        Assert.Contains("SourceContent", ex.Message);
    }

    [Fact]
    public void Render_UnusedSuppliedToken_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.txt", ("SourceContent", "x"), ("Other", "y")));
    }

    [Fact]
    public void Render_DuplicateSuppliedToken_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.txt", ("SourceContent", "a"), ("SourceContent", "b")));

        Assert.Contains("SourceContent", ex.Message);
    }

    [Fact]
    public void Render_ValueWithBracesJsonUnicodeAndTokenLikeText_IsInsertedVerbatim()
    {
        const string value = "{\"a\": 1}\n# Nu\u00EByon {{SourceContent}} {x}\r\n**md**";

        var result = PromptLoader.Render("Idea/Idea.user.prompt.txt", ("SourceContent", value));

        Assert.Contains(value, result);
    }

    [Fact]
    public void Render_InsertedValue_IsNotProcessedRecursively()
    {
        var template = PromptLoader.Load("Idea/Idea.user.prompt.txt");

        var result = PromptLoader.Render("Idea/Idea.user.prompt.txt", ("SourceContent", "{{SourceContent}}"));

        Assert.Equal(template, result);
    }

    [Fact]
    public void Render_TokenRepeatedInTemplate_ReplacesEveryOccurrence()
    {
        var result = PromptLoader.Render(
            "Compose/Compose.linkedin.user.prompt.txt",
            ("Format", "FMT-MARKER"),
            ("NarrativeContent", "narrative"));

        Assert.True(result.Split("FMT-MARKER").Length - 1 >= 2);
    }

    [Fact]
    public void Render_AllTokensSupplied_LeavesNoUnresolvedTokens()
    {
        var result = PromptLoader.Render(
            "Compose/Compose.linkedin.user.prompt.txt",
            ("Format", "LinkedInPost"),
            ("NarrativeContent", "narrative"));

        Assert.DoesNotContain("{{", result);
    }
}
