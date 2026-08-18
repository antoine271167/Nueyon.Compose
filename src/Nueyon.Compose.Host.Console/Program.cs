using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Flow;
using Nueyon.Compose.Host.Console;
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

// Add console application
services.AddSingleton<ConsoleApplication>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

try
{
    // Validate OpenAI configuration
    var openAiOptions = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
    openAiOptions.Validate();

    // Get and run the console application
    var consoleApplication = serviceProvider.GetRequiredService<ConsoleApplication>();

    // Create a cancellation token source to handle Ctrl+C
    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        cts.Cancel();
    };

    // Run the application
    var exitCode = await consoleApplication.RunAsync(cts.Token);
    Environment.Exit(exitCode);
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
