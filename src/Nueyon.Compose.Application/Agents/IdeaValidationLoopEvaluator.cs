using System.Text.Json;
using Microsoft.Agents.AI;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Nueyon.Compose.Application.Agents;

/// <summary>
/// A Nueyon-specific evaluator for the Microsoft Agent Framework LoopAgent.
/// 
/// This evaluator integrates Idea validation into the loop decision process.
/// After each agent iteration, it parses the agent's text response into Ideas,
/// validates them using IdeaValidator, and decides whether to re-invoke the agent.
/// 
/// The evaluator is stateless and safe for concurrent use.
/// </summary>
public sealed class IdeaValidationLoopEvaluator : LoopEvaluator
{
    private readonly IIdeaValidator _validator;

    /// <summary>
    /// Initializes a new instance of IdeaValidationLoopEvaluator.
    /// </summary>
    /// <param name="validator">The validator to use for inspecting generated ideas.</param>
    /// <exception cref="ArgumentNullException">Thrown when validator is null.</exception>
    public IdeaValidationLoopEvaluator(IIdeaValidator validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <summary>
    /// Evaluates the agent's response and decides whether to re-invoke the agent
    /// based on idea validation.
    /// 
    /// The evaluation logic:
    /// 1. Extract and parse the JSON from the LoopContext.LastResponse.Text
    /// 2. Convert to IReadOnlyList&lt;Idea&gt;
    /// 3. Pass to IdeaValidator.IsValid()
    /// 4. If valid, return LoopEvaluation.Stop() (success - no more attempts needed)
    /// 5. If invalid:
    ///    a. If at the final iteration (Iteration == MaxIterations), throw InvalidOperationException
    ///    b. Otherwise, return LoopEvaluation.Continue() to retry with feedback
    /// 
    /// Note: Parsing happens here for validation, and again in IdeaAgent for the final
    /// output. This duplication is acceptable to maintain clean architecture.
    /// </summary>
    /// <param name="context">The current loop context containing the agent response and state.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A LoopEvaluation indicating whether to continue or stop the loop.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the agent fails to produce valid ideas after the maximum number of attempts.</exception>
    public override async ValueTask<LoopEvaluation> EvaluateAsync(
        LoopContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Extract the text response from the agent's last iteration
        var responseText = context.LastResponse.Text;

        if (string.IsNullOrWhiteSpace(responseText))
        {
            // Empty response is invalid - check if at final attempt
            if (context.Iteration == 3)
            {
                throw new InvalidOperationException("Idea agent failed to produce valid ideas after 3 attempts.");
            }

            // Not at final attempt - ask the agent to try again
            return LoopEvaluation.Continue("The agent provided an empty response. Please generate content ideas.");
        }

        // Parse the JSON response into ideas for validation
        IReadOnlyList<Idea> ideas;
        try
        {
            ideas = ParseIdeasFromJson(responseText);
        }
        catch (InvalidOperationException ex)
        {
            // Parsing failed - check if at final attempt
            if (context.Iteration == 3)
            {
                throw new InvalidOperationException("Idea agent failed to produce valid ideas after 3 attempts.", ex);
            }

            // Not at final attempt - ask the agent to try again with feedback
            return LoopEvaluation.Continue(
                $"Failed to parse the response: {ex.Message} " +
                "Please respond with valid JSON containing an array of ideas with Title, Description, Audience, and Rationale fields.");
        }

        // Validate the ideas using the existing deterministic validator
        var isValid = _validator.IsValid(ideas);

        if (isValid)
        {
            // Ideas are valid - stop the loop and return success
            return LoopEvaluation.Stop();
        }

        // Ideas are invalid - check if at final attempt
        if (context.Iteration == 3)
        {
            throw new InvalidOperationException("Idea agent failed to produce valid ideas after 3 attempts.");
        }

        // Not at final attempt - continue the loop for another attempt with feedback
        var feedback = "The generated ideas do not meet validation requirements. " +
            "Ensure each idea has a non-empty Title, Description, Audience, and Rationale.";

        return LoopEvaluation.Continue(feedback);
    }

    /// <summary>
    /// Parses the JSON response from the model into a list of Idea objects.
    /// 
    /// This mirrors the parsing logic in IdeaAgent. The duplication is intentional
    /// to maintain separation between the validation loop logic and the agent result processing.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <returns>A read-only list of parsed Idea objects.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the JSON is invalid or cannot be deserialized.</exception>
    private static IReadOnlyList<Idea> ParseIdeasFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException("Agent response is empty.");
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var ideas = JsonSerializer.Deserialize<List<Idea>>(json, options)
                ?? throw new InvalidOperationException("Deserialization resulted in null list.");

            return ideas.AsReadOnly();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Failed to parse agent response as JSON: {ex.Message}",
                ex);
        }
    }
}

#pragma warning restore MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.
