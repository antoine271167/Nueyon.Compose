using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Harness;
using Nueyon.Compose.Application.Validation;
using Nueyon.Compose.Domain;
using Nueyon.Compose.Infrastructure.Options;

namespace Nueyon.Compose.Infrastructure.Tests;

public sealed class InfrastructureServiceExtensionsTests
{
    [Fact]
    public void AddInfrastructure_RegistersIdeaValidator()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var validator = provider.GetRequiredService<IIdeaValidator>();
        Assert.NotNull(validator);
        Assert.IsType<IdeaValidator>(validator);
    }

    [Fact]
    public void AddInfrastructure_RegistersIdeaAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var agent = provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>();
        Assert.NotNull(agent);
        Assert.IsType<IdeaAgent>(agent);
    }

    [Fact]
    public void AddInfrastructure_RegistersIdeaHarness()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var harness = provider.GetRequiredService<IAgentHarness<ChatInput, IReadOnlyList<Idea>>>();
        Assert.NotNull(harness);
        Assert.IsType<IdeaHarness>(harness);
    }

    [Fact]
    public void AddInfrastructure_WithMissingApiKey_ThrowsWhenResolvingAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>());
        Assert.Contains("API key", exception.Message);
    }

    [Fact]
    public void AddInfrastructure_WithMissingModel_ThrowsWhenResolvingAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>());
        Assert.Contains("model", exception.Message);
    }

    [Fact]
    public void AddInfrastructure_ThrowsWhenServicesIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => InfrastructureServiceExtensions.AddInfrastructure(null!));
        Assert.Equal("services", exception.ParamName);
    }

    [Fact]
    public void AddInfrastructure_AgentAndHarnessAreSingletons()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act
        var agent1 = provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>();
        var agent2 = provider.GetRequiredService<IAgent<ChatInput, IReadOnlyList<Idea>>>();
        var harness1 = provider.GetRequiredService<IAgentHarness<ChatInput, IReadOnlyList<Idea>>>();
        var harness2 = provider.GetRequiredService<IAgentHarness<ChatInput, IReadOnlyList<Idea>>>();

        // Assert
        Assert.Same(agent1, agent2);
        Assert.Same(harness1, harness2);
    }
}
