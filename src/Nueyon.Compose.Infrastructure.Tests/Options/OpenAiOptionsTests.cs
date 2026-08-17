using Nueyon.Compose.Infrastructure.Options;

namespace Nueyon.Compose.Infrastructure.Tests.Options;

public sealed class OpenAiOptionsTests
{
    [Fact]
    public void Validate_WithValidOptions_DoesNotThrow()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "sk-test-key",
            Model = "gpt-4o-mini"
        };

        // Act & Assert
        options.Validate();
    }

    [Fact]
    public void Validate_WithMissingApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "",
            Model = "gpt-4o-mini"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("API key", exception.Message);
    }

    [Fact]
    public void Validate_WithNullApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = null!,
            Model = "gpt-4o-mini"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("API key", exception.Message);
    }

    [Fact]
    public void Validate_WithWhitespaceApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "   ",
            Model = "gpt-4o-mini"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("API key", exception.Message);
    }

    [Fact]
    public void Validate_WithMissingModel_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "sk-test-key",
            Model = ""
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("model", exception.Message);
    }

    [Fact]
    public void Validate_WithNullModel_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "sk-test-key",
            Model = null!
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("model", exception.Message);
    }

    [Fact]
    public void Validate_WithWhitespaceModel_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "sk-test-key",
            Model = "   "
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("model", exception.Message);
    }

    [Fact]
    public void Validate_WithBothMissing_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = new OpenAiOptions
        {
            ApiKey = "",
            Model = ""
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        // Should fail on API key first
        Assert.Contains("API key", exception.Message);
    }
}
