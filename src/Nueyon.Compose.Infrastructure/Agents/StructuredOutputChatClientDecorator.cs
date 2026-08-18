using System.Text.Json;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Nueyon.Compose.Infrastructure.Agents;

/// <summary>
/// Decorator for IChatClient that injects structured output format configuration for IdeaResponse.
/// This ensures that all chat completions requests include the IdeaResponse JSON schema format,
/// constraining the OpenAI model output to match the expected structure.
/// </summary>
internal sealed class StructuredOutputChatClientDecorator : IChatClient
{
    private readonly IChatClient _innerClient;

    public StructuredOutputChatClientDecorator(IChatClient innerClient)
    {
        _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
    }

    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<Microsoft.Extensions.AI.ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options = ConfigureStructuredOutput(options);
        return await _innerClient.GetResponseAsync(messages, options, cancellationToken);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<Microsoft.Extensions.AI.ChatMessage> messages,
        ChatOptions? options = null,
        System.Threading.CancellationToken cancellationToken = default)
    {
        options = ConfigureStructuredOutput(options);
        await foreach (var update in _innerClient.GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return _innerClient.GetService(serviceType, serviceKey);
    }

    public void Dispose()
    {
        _innerClient.Dispose();
    }

    /// <summary>
    /// Configures the ChatOptions to include structured output format for IdeaResponse.
    /// This method checks if structured output was requested via AdditionalProperties and
    /// converts it to the appropriate OpenAI ChatClient configuration.
    /// </summary>
    private static ChatOptions ConfigureStructuredOutput(ChatOptions? options)
    {
        options ??= new ChatOptions();

        // Check if structured output was requested
        if (options.AdditionalProperties != null &&
            options.AdditionalProperties.TryGetValue("StructuredOutputType", out var typeObj) &&
            typeObj is Type structuredType &&
            structuredType.Name == "IdeaResponse")
        {
            // Get the schema if available
            if (options.AdditionalProperties.TryGetValue("StructuredOutputSchema", out var schemaObj) &&
                schemaObj is string schemaJson)
            {
                // Create OpenAI's ChatResponseFormat with the schema
                var responseFormat = OpenAI.Chat.ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "IdeaResponse",
                    jsonSchema: System.BinaryData.FromString(schemaJson),
                    jsonSchemaFormatDescription: "Structured response containing an array of content ideas",
                    jsonSchemaIsStrict: true);

                // Store it in AdditionalProperties so the OpenAI adapter can use it
                options.AdditionalProperties["OpenAIChatResponseFormat"] = responseFormat;
            }
        }

        return options;
    }
}
