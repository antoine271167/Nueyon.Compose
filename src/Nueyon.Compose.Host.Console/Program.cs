using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Application.Harness;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Infrastructure;
using Nueyon.Compose.Infrastructure.Options;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .Build();

// Set up dependency injection
var services = new ServiceCollection();

// Add configuration
services.Configure<OpenAiOptions>(configuration.GetSection("OpenAI"));

// Add logging
services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddConsole();
    builder.AddConfiguration(configuration.GetSection("Logging"));
});

// Add infrastructure (OpenAI integration)
services.AddInfrastructure();

// Add application services
services.AddSingleton<IFlowEngine, StoryFlowEngine>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

try
{
    // Validate OpenAI configuration
    var openAiOptions = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
    openAiOptions.Validate();

    // Get the flow engine
    var flowEngine = serviceProvider.GetRequiredService<IFlowEngine>();

    // Execute the Compose flow with sample input
    Console.WriteLine("Nueyon.Compose");
    Console.WriteLine("================");
    Console.WriteLine();

    // Create a sample input
    var input = new ChatInput
    {
        Content = "Create a short story about a time traveler discovering an ancient library"
    };

    Console.WriteLine($"Processing: {input.Content}");
    Console.WriteLine();

    // Execute the flow
    var workspace = new StoryWorkspace { Input = input };
    var result = await flowEngine.ExecuteAsync(workspace, CancellationToken.None);

    // Display results
    if (result.Ideas is not null && result.Ideas.Count > 0)
    {
        Console.WriteLine("Generated Ideas:");
        Console.WriteLine("================");
        foreach (var idea in result.Ideas)
        {
            Console.WriteLine($"\nTitle: {idea.Title}");
            Console.WriteLine($"Description: {idea.Description}");
            Console.WriteLine($"Audience: {idea.Audience}");
            Console.WriteLine($"Rationale: {idea.Rationale}");
        }
    }
    else
    {
        Console.WriteLine("No ideas were generated.");
    }
}
catch (InvalidOperationException ex)
{
    Console.Error.WriteLine($"Configuration Error: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    if (ex.InnerException is not null)
    {
        Console.Error.WriteLine($"Details: {ex.InnerException.Message}");
    }
    Environment.Exit(1);
}
finally
{
    await serviceProvider.DisposeAsync();
}
