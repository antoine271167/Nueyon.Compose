using System.Text.Json;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Agents;

/// <summary>
/// Tests for IdeaAgent response parsing with the new IdeaResponse structured output contract.
/// 
/// These tests verify that:
/// - Valid IdeaResponse JSON (with wrapper object) deserializes correctly
/// - Multiple ideas in the ideas array are handled
/// - Empty ideas arrays are handled
/// - Invalid JSON raises clear exceptions
/// - Wrong root shapes (bare arrays) are explicitly rejected
/// </summary>
public sealed class IdeaAgentResponseParsingTests
{
    /// <summary>
    /// Helper to call the private ParseIdeasFromJson method via reflection.
    /// Unwraps TargetInvocationException to expose the actual exception.
    /// </summary>
    private static IReadOnlyList<Idea> ParseJson(string json)
    {
        var method = typeof(IdeaAgent).GetMethod(
            "ParseIdeasFromJson",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

        ArgumentNullException.ThrowIfNull(method, nameof(method));

        try
        {
            var result = method.Invoke(null, new object[] { json });
            return (IReadOnlyList<Idea>)result!;
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            // Unwrap the actual exception that was thrown inside the method
            throw ex.InnerException ?? ex;
        }
    }

    /// <summary>
    /// Test: Valid response with single idea in wrapper object
    /// Expected: Deserializes successfully and returns the idea
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithValidSingleIdea_ReturnsOneIdea()
    {
        // Arrange
        var json = """
        {
          "ideas": [
            {
              "title": "First idea",
              "description": "First description",
              "audience": "Developers",
              "rationale": "Useful because..."
            }
          ]
        }
        """;

        // Act
        var result = ParseJson(json);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("First idea", result[0].Title);
        Assert.Equal("First description", result[0].Description);
        Assert.Equal("Developers", result[0].Audience);
        Assert.Equal("Useful because...", result[0].Rationale);
    }

    /// <summary>
    /// Test: Valid response with multiple ideas
    /// Expected: Deserializes successfully and returns all ideas
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithMultipleIdeas_ReturnsAllIdeas()
    {
        // Arrange
        var json = """
        {
          "ideas": [
            {
              "title": "Idea 1",
              "description": "Description 1",
              "audience": "Audience 1",
              "rationale": "Rationale 1"
            },
            {
              "title": "Idea 2",
              "description": "Description 2",
              "audience": "Audience 2",
              "rationale": "Rationale 2"
            }
          ]
        }
        """;

        // Act
        var result = ParseJson(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Idea 1", result[0].Title);
        Assert.Equal("Idea 2", result[1].Title);
    }

    /// <summary>
    /// Test: Valid response with empty ideas array
    /// Expected: Deserializes successfully and returns empty list
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithEmptyIdeasArray_ReturnsEmptyList()
    {
        // Arrange
        var json = """
        {
          "ideas": []
        }
        """;

        // Act
        var result = ParseJson(json);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    /// Test: Invalid JSON throws clear exception
    /// Expected: InvalidOperationException with descriptive message
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithInvalidJson_ThrowsInvalidOperationException()
    {
        // Arrange
        var json = "not valid json at all";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ParseJson(json));
        Assert.Contains("Failed to parse agent response as JSON", exception.Message);
    }

    /// <summary>
    /// Test: Wrong root shape - bare array instead of wrapper object
    /// Expected: FAILS with clear error, does NOT fall back to array parsing
    /// 
    /// This is a critical test to ensure we don't accidentally accept the old format.
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithBareArrayInsteadOfWrapper_FailsWithClearError()
    {
        // Arrange
        var json = """
        [
          {
            "title": "Wrong shape",
            "description": "This is a bare array",
            "audience": "Should fail",
            "rationale": "Because the contract requires a wrapper"
          }
        ]
        """;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ParseJson(json));
        Assert.Contains("Failed to parse agent response as JSON", exception.Message);
    }

    /// <summary>
    /// Test: Empty response string
    /// Expected: InvalidOperationException with "empty response" message
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithEmptyString_ThrowsWithEmptyResponseMessage()
    {
        // Arrange
        var json = "";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ParseJson(json));
        Assert.Contains("Agent response is empty", exception.Message);
    }

    /// <summary>
    /// Test: Whitespace-only response
    /// Expected: InvalidOperationException with "empty response" message
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithWhitespaceOnly_ThrowsWithEmptyResponseMessage()
    {
        // Arrange
        var json = "   \n\t  ";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ParseJson(json));
        Assert.Contains("Agent response is empty", exception.Message);
    }

    /// <summary>
    /// Test: Wrapper object missing ideas property
    /// Expected: InvalidOperationException during deserialization
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithMissingIdeasProperty_ThrowsInvalidOperationException()
    {
        // Arrange
        var json = """
        {
          "title": "No ideas property"
        }
        """;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ParseJson(json));
        Assert.Contains("Failed to parse agent response as JSON", exception.Message);
    }

    /// <summary>
    /// Test: Case-insensitive property names
    /// Expected: Successfully deserializes with different casing
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithDifferentCasing_DeserializesSuccessfully()
    {
        // Arrange
        var json = """
        {
          "IDEAS": [
            {
              "TITLE": "Title Test",
              "DESCRIPTION": "Desc Test",
              "AUDIENCE": "Audience Test",
              "RATIONALE": "Rationale Test"
            }
          ]
        }
        """;

        // Act
        var result = ParseJson(json);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Title Test", result[0].Title);
    }

    /// <summary>
    /// Test: JSON with extra properties is ignored
    /// Expected: Successfully deserializes, ignoring extra properties
    /// </summary>
    [Fact]
    public void ParseIdeasFromJson_WithExtraProperties_IgnoresExtraAndDeserializes()
    {
        // Arrange
        var json = """
        {
          "ideas": [
            {
              "title": "Valid Idea",
              "description": "Description",
              "audience": "Audience",
              "rationale": "Rationale",
              "extra_field": "This should be ignored",
              "another_extra": 42
            }
          ],
          "extra_root_field": "Also ignored"
        }
        """;

        // Act
        var result = ParseJson(json);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Valid Idea", result[0].Title);
    }
}
