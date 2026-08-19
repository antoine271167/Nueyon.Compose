using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Validation;

/// <summary>
///     Validates that ideas conform to structural requirements.
/// </summary>
public sealed class IdeaValidator : IIdeaValidator
{
    /// <summary>
    ///     Determines whether the provided list of ideas is valid.
    ///     An idea list is valid only when:
    ///     1. the list is not null;
    ///     2. the list contains at least one idea;
    ///     3. every idea has a non-empty Title;
    ///     4. every idea has a non-empty Description;
    ///     5. every idea has a non-empty Audience;
    ///     6. every idea has a non-empty Rationale.
    /// </summary>
    /// <param name="ideas">The list of ideas to validate.</param>
    /// <returns>True if the ideas are valid; otherwise false.</returns>
    public bool IsValid(IReadOnlyList<Idea> ideas)
    {
        // Null or empty list is invalid
        if (ideas.Count == 0)
        {
            return false;
        }

        // Check that each idea has all required non-empty fields
        foreach (var idea in ideas)
        {
            if (string.IsNullOrWhiteSpace(idea.Title) ||
                string.IsNullOrWhiteSpace(idea.Description) ||
                string.IsNullOrWhiteSpace(idea.Audience) ||
                string.IsNullOrWhiteSpace(idea.Rationale))
            {
                return false;
            }
        }

        return true;
    }
}