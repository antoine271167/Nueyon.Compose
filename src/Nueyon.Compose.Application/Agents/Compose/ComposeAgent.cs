using System.Diagnostics;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Compose;

/// <summary>
///     An agent that transforms a narrative into finished content for a specified content format.
/// </summary>
public sealed class ComposeAgent(
    AIAgent agent,
    ILogger<ComposeAgent> logger)
    : IAgent<ComposeInput, ComposeResult>
{
    private readonly AIAgent _agent = agent ?? throw new ArgumentNullException(nameof(agent));
    private readonly ILogger<ComposeAgent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<ComposeResult> ExecuteAsync(
        AgentExecutionContext executionContext,
        ComposeInput input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(executionContext);
        ArgumentNullException.ThrowIfNull(input);

        const string agentName = "ComposeAgent";
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Agent {AgentName} invocation started with ExecutionId {ExecutionId}",
                agentName,
                executionContext.ExecutionId);

            var userMessage = CreateUserMessage(input);

            var options = CreateAgentRunOptions();

            var response = await _agent.RunAsync(
                userMessage,
                null,
                options,
                cancellationToken);

            var responseText = ExtractResponseText(response);
            var composeResult = ParseComposeResultFromJson(responseText);

            stopwatch.Stop();

            _logger.LogInformation(
                "Agent {AgentName} invocation completed in {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            return composeResult;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            stopwatch.Stop();
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Agent {AgentName} invocation failed after {Duration}ms with ExecutionId {ExecutionId}",
                agentName,
                stopwatch.ElapsedMilliseconds,
                executionContext.ExecutionId);

            throw;
        }
    }

    /// <summary>
    ///     Gets the system instructions used to configure this agent's underlying AI agent.
    /// </summary>
    public static string GetSystemInstructions() => ComposeAgentInstructions.GetSystemInstructions();

    private static string CreateUserMessage(ComposeInput input)
    {
        var format = input.Composition.Format;

        return format switch
        {
            ContentFormat.Article => CreateArticleUserMessage(input),
            ContentFormat.LinkedInPost => CreateLinkedInPostUserMessage(input),
            _ => throw new InvalidOperationException(
                $"Unsupported content format: {format}")
        };
    }

    private static string CreateArticleUserMessage(ComposeInput input) =>
        PromptLoader.Render(
            "Compose/Compose.article.user.prompt.txt",
            ("Format", input.Composition.Format.ToString()),
            ("NarrativeContent", input.Narrative.Content));


    private static string CreateLinkedInPostUserMessage(ComposeInput input) =>
        PromptLoader.Render(
            "Compose/Compose.linkedin.user.prompt.txt",
            ("Format", input.Composition.Format.ToString()),
            ("NarrativeContent", input.Narrative.Content));

    private static ChatClientAgentRunOptions CreateAgentRunOptions()
    {
        var responseFormat = ChatResponseFormat.ForJsonSchema<ComposeResult>(
            null,
            nameof(ComposeResult));

        var chatOptions = new ChatOptions
        {
            ResponseFormat = responseFormat
        };

        return new ChatClientAgentRunOptions(chatOptions);
    }

    private static string ExtractResponseText(AgentResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        var text = response.Text;

        return string.IsNullOrWhiteSpace(text)
            ? throw new InvalidOperationException(
                "Agent response does not contain text content.")
            : text;
    }

    private static ComposeResult ParseComposeResultFromJson(string json)
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

            var response = JsonSerializer.Deserialize<ComposeResult>(
                json,
                options);

            if (response is null)
            {
                throw new InvalidOperationException(
                    "Deserialization resulted in null response.");
            }

            if (string.IsNullOrWhiteSpace(response.Content))
            {
                throw new InvalidOperationException(
                    "Compose response content is empty.");
            }

            return response;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Failed to parse agent response as JSON: {ex.Message}",
                ex);
        }
    }
}
