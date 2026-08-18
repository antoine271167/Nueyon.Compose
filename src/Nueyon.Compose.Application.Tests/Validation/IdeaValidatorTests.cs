using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Validation;

public sealed class IdeaValidatorTests
{
    private readonly IdeaValidator _validator = new();

    /// <summary>
    ///     Test: A list containing one fully populated Idea returns true.
    /// </summary>
    [Fact]
    public void IsValid_WithSingleValidIdea_ReturnsTrue()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "Test Title",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    ///     Test: A list containing multiple fully populated Ideas returns true.
    /// </summary>
    [Fact]
    public void IsValid_WithMultipleValidIdeas_ReturnsTrue()
    {
        // Arrange
        var idea1 = new Idea
        {
            Title = "Title 1",
            Description = "Description 1",
            Audience = "Audience 1",
            Rationale = "Rationale 1"
        };
        var idea2 = new Idea
        {
            Title = "Title 2",
            Description = "Description 2",
            Audience = "Audience 2",
            Rationale = "Rationale 2"
        };
        var ideas = new[] { idea1, idea2 };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    ///     Test: A null list returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithNullList_ReturnsFalse()
    {
        // Act
        var result = _validator.IsValid(null!);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: An empty list returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithEmptyList_ReturnsFalse()
    {
        // Arrange
        var ideas = Array.Empty<Idea>();

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea missing Title returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithMissingTitle_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea having null Title returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithNullTitle_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = null!,
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea having whitespace-only Title returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithWhitespaceTitle_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "   ",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea missing Description returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithMissingDescription_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "Test Title",
            Description = "",
            Audience = "Test Audience",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea missing Audience returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithMissingAudience_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "Test Title",
            Description = "Test Description",
            Audience = "",
            Rationale = "Test Rationale"
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list with an idea missing Rationale returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithMissingRationale_ReturnsFalse()
    {
        // Arrange
        var idea = new Idea
        {
            Title = "Test Title",
            Description = "Test Description",
            Audience = "Test Audience",
            Rationale = ""
        };
        var ideas = new[] { idea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    ///     Test: A list where the first idea is valid but the second is missing a field returns false.
    /// </summary>
    [Fact]
    public void IsValid_WithOneValidAndOneInvalidIdea_ReturnsFalse()
    {
        // Arrange
        var validIdea = new Idea
        {
            Title = "Title 1",
            Description = "Description 1",
            Audience = "Audience 1",
            Rationale = "Rationale 1"
        };
        var invalidIdea = new Idea
        {
            Title = "Title 2",
            Description = "Description 2",
            Audience = "",
            Rationale = "Rationale 2"
        };
        var ideas = new[] { validIdea, invalidIdea };

        // Act
        var result = _validator.IsValid(ideas);

        // Assert
        Assert.False(result);
    }
}