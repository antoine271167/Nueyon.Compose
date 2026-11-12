using Nueyon.Compose.Application.Prompts;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Prompts;

public class PromptLoaderTests
{
[Fact]
public void DirectoryScope_UsesDiskStrictlyAndRestoresEmbeddedPrompts()
{
    var directory = Path.Combine(Path.GetTempPath(), $"nueyon-prompt-test-{Guid.NewGuid():N}");
    Directory.CreateDirectory(Path.Combine(directory, "Idea"));
    try
    {
        File.WriteAllText(Path.Combine(directory, "Idea", "Idea.system.prompt.md"), "disk prompt");
        var embedded = PromptLoader.Load("Idea/Idea.system.prompt.md");
        using (PromptDirectory.Use(directory))
        {
            Assert.Equal("disk prompt", PromptLoader.Load("Idea/Idea.system.prompt.md"));
            Assert.Throws<FileNotFoundException>(() => PromptLoader.Load("Idea/Idea.user.prompt.md"));
        }
        Assert.Equal(embedded, PromptLoader.Load("Idea/Idea.system.prompt.md"));
    }
    finally { Directory.Delete(directory, true); }
}

    public static IEnumerable<object[]> AllPrompts =>
    [
        ["Idea/Idea.system.prompt.md"],
        ["Idea/Idea.user.prompt.md"],
        ["Idea/IdeaValidation.system.prompt.md"],
        ["Research/Research.system.prompt.md"],
        ["Research/Research.user.prompt.md"],
        ["Synthesis/Synthesis.system.prompt.md"],
        ["Synthesis/Synthesis.user.prompt.md"],
        ["Narrative/Narrative.system.prompt.md"],
        ["Narrative/Narrative.user.prompt.md"],
        ["Compose/Compose.system.prompt.md"],
        ["Compose/Compose.article.user.prompt.md"],
        ["Compose/Compose.linkedin.user.prompt.md"]
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
            PromptLoader.Load("Idea/Idea.system.prompt.md"),
            PromptLoader.Load("Idea\\Idea.system.prompt.md"));
    }

    [Fact]
    public void Load_MissingPrompt_ThrowsWithPromptPath()
    {
        var ex = Assert.Throws<FileNotFoundException>(() => PromptLoader.Load("Idea/Missing.prompt.md"));

        Assert.Contains("Idea/Missing.prompt.md", ex.Message);
    }

    [Fact]
    public void Render_MissingTokenValue_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.md"));

        Assert.Contains("SourceContent", ex.Message);
    }

    [Fact]
    public void Render_UnusedSuppliedToken_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.md", ("SourceContent", "x"), ("Other", "y")));
    }

    [Fact]
    public void Render_DuplicateSuppliedToken_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            PromptLoader.Render("Idea/Idea.user.prompt.md", ("SourceContent", "a"), ("SourceContent", "b")));

        Assert.Contains("SourceContent", ex.Message);
    }

    [Fact]
    public void Render_ValueWithBracesJsonUnicodeAndTokenLikeText_IsInsertedVerbatim()
    {
        const string value = "{\"a\": 1}\n# Nu\u00EByon {{SourceContent}} {x}\r\n**md**";

        var result = PromptLoader.Render("Idea/Idea.user.prompt.md", ("SourceContent", value));

        Assert.Contains(value, result);
    }

    [Fact]
    public void Render_ValueContainingDifferentTokenLikeText_IsKeptLiterally()
    {
        var result = PromptLoader.Render(
            "Idea/Idea.user.prompt.md",
            ("SourceContent", "before {{UnexpectedToken}} after"));

        Assert.Contains("before {{UnexpectedToken}} after", result);
    }

    [Fact]
    public void Render_ValueContainingOwnTokenName_IsNotReExpanded()
    {
        var template = PromptLoader.Load("Idea/Idea.user.prompt.md");

        var result = PromptLoader.Render("Idea/Idea.user.prompt.md", ("SourceContent", "{{SourceContent}}"));

        Assert.Equal(template, result);
    }

    [Theory]
    [InlineData("input.Synthesis.Content")]
    [InlineData("Source_Content")]
    [InlineData("1Name")]
    [InlineData("Name-With-Hyphen")]
    [InlineData("Name With Space")]
    [InlineData("")]
    public void RenderTemplate_UnsupportedPlaceholderName_ThrowsNamingPromptAndExpression(string name)
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            PromptLoader.RenderTemplate("X/Test.prompt.md", "a {{" + name + "}} b"));

        Assert.Contains("{{" + name + "}}", ex.Message);
        Assert.Contains("X/Test.prompt.md", ex.Message);
    }

    [Fact]
    public void RenderTemplate_JsonAndSingleBraces_AreOrdinaryText()
    {
        var result = PromptLoader.RenderTemplate(
            "X/Test.prompt.md",
            "{\"a\": {\"b\": 1}} {x} {{Name}}",
            ("Name", "v"));

        Assert.Equal("{\"a\": {\"b\": 1}} {x} v", result);
    }

    [Fact]
    public void RenderTemplate_ValueWithUnsupportedPlaceholderText_IsNotRejected()
    {
        var result = PromptLoader.RenderTemplate(
            "X/Test.prompt.md",
            "{{Name}}",
            ("Name", "{{input.Content}} {{A_B}}"));

        Assert.Equal("{{input.Content}} {{A_B}}", result);
    }

    [Theory]
    [InlineData("{{UnexpectedToken}}")]
    [InlineData("{{input.Content}}")]
    [InlineData("{{A_B}}")]
    [InlineData("{{Name-With-Hyphen}}")]
    [InlineData("{{Name}}")]
    public void RenderTemplate_ValueWithTokenLikeText_IsPreservedLiterally(string value)
    {
        var result = PromptLoader.RenderTemplate("X/Test.prompt.md", "a {{Name}} b", ("Name", value));

        Assert.Equal("a " + value + " b", result);
    }

    [Theory]
    [InlineData("{{Name}}", "Name", "v")]
    [InlineData("{{SourceContent}}", "SourceContent", "v")]
    [InlineData("{{Token123}}", "Token123", "v")]
    public void RenderTemplate_ValidPlaceholder_IsReplaced(string template, string name, string value)
    {
        Assert.Equal(value, PromptLoader.RenderTemplate("X/Test.prompt.md", template, (name, value)));
    }

    [Fact]
    public void Render_TokenRepeatedInTemplate_ReplacesEveryOccurrence()
    {
        var result = PromptLoader.Render(
            "Compose/Compose.linkedin.user.prompt.md",
            ("Format", "FMT-MARKER"),
            ("NarrativeContent", "narrative"));

        Assert.True(result.Split("FMT-MARKER").Length - 1 >= 2);
    }

    [Fact]
    public void Render_AllTokensSupplied_LeavesNoUnresolvedTokens()
    {
        var result = PromptLoader.Render(
            "Compose/Compose.linkedin.user.prompt.md",
            ("Format", "LinkedInPost"),
            ("NarrativeContent", "narrative"));

        Assert.DoesNotContain("{{", result);
    }
}
