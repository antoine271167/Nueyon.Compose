using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Host.Console;
using Nueyon.Compose.Infrastructure;
using Nueyon.Compose.Infrastructure.Options;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true, true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(true)
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

// Add console abstraction
services.AddSingleton<IConsole, SystemConsole>();

// Add workflow services
services.AddSingleton<IStoryWorkflow>(provider =>
{
    var agent = provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>();
    var executor = IdeaExecutorFactory.CreateIdeaExecutor(agent);
    return new StoryWorkflow(executor);
});

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
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        // ReSharper disable once AccessToDisposedClosure
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