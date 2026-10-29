using Nueyon.Compose.Application.Prompts;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Prompts;

public class PromptLoaderTests
{
    public static IEnumerable<object[]> AllPrompts =>
    [
        ["Idea/Idea.system.prompt.txt"],
        ["Idea/Idea.user.prompt.txt"],
        ["Research/Research.system.prompt.txt"],
        ["Research/Research.user.prompt.txt"],
        ["Synthesis/Synthesis.system.prompt.txt"],
        ["Synthesis/Synthesis.user.prompt.txt"],
        ["Narrative/Narrative.system.prompt.txt"],
        ["Narrative/Narrative.user.prompt.txt"],
        ["Compose/Compose.system.prompt.txt"],
        ["Compose/Compose.article.user.prompt.txt"],
        ["Compose/Compose.linkedin.user.prompt.txt"]
    ];

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
    public void Render_ValueContainingDifferentTokenLikeText_IsKeptLiterally()
    {
        var result = PromptLoader.Render(
            "Idea/Idea.user.prompt.txt",
            ("SourceContent", "before {{UnexpectedToken}} after"));

        Assert.Contains("before {{UnexpectedToken}} after", result);
    }

    [Fact]
    public void Render_ValueContainingOwnTokenName_IsNotReExpanded()
    {
        var template = PromptLoader.Load("Idea/Idea.user.prompt.txt");

        var result = PromptLoader.Render("Idea/Idea.user.prompt.txt", ("SourceContent", "{{SourceContent}}"));

        Assert.Equal(template, result);
    }

    [Theory]
    [InlineData("input.Synthesis.Content")]
    [InlineData("Source_Content")]
    public void RenderTemplate_UnsupportedPlaceholderName_ThrowsNamingPromptAndExpression(string name)
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.RenderTemplate("X/Test.prompt.txt", "a {{" + name + "}} b"));

        Assert.Contains("{{" + name + "}}", ex.Message);
        Assert.Contains("X/Test.prompt.txt", ex.Message);
    }

    [Fact]
    public void RenderTemplate_JsonAndSingleBraces_AreOrdinaryText()
    {
        var result = PromptLoader.RenderTemplate(
            "X/Test.prompt.txt",
            "{\"a\": {\"b\": 1}} {x} {{Name}}",
            ("Name", "v"));

        Assert.Equal("{\"a\": {\"b\": 1}} {x} v", result);
    }

    [Fact]
    public void RenderTemplate_ValueWithUnsupportedPlaceholderText_IsNotRejected()
    {
        var result = PromptLoader.RenderTemplate(
            "X/Test.prompt.txt",
            "{{Name}}",
            ("Name", "{{input.Content}} {{A_B}}"));

        Assert.Equal("{{input.Content}} {{A_B}}", result);
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