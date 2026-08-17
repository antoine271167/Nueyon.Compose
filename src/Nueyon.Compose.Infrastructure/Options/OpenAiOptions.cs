namespace Nueyon.Compose.Infrastructure.Options;

/// <summary>
/// Strongly typed configuration for OpenAI integration.
/// </summary>
public sealed class OpenAiOptions
{
    /// <summary>
    /// The OpenAI API key required to authenticate with the OpenAI API.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The OpenAI model name (e.g., "gpt-4o-mini").
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Validates that the required OpenAI configuration is present.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when API key or model is missing.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API key is required. Configure it via appsettings.json, environment variables, or user secrets.");
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            throw new InvalidOperationException(
                "OpenAI model name is required. Configure it via appsettings.json, environment variables, or user secrets.");
        }
    }
}
