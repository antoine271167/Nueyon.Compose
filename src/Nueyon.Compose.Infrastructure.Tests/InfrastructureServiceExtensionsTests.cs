using Microsoft.Extensions.DependencyInjection;
using Nueyon.Compose.Application.Agents;
using Nueyon.Compose.Application.Agents.Idea;
using Nueyon.Compose.Application.Agents.Narrative;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
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
        services.AddLogging();
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
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var agent = provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>();
        Assert.NotNull(agent);
        Assert.IsType<IdeaAgent>(agent);
    }

    [Fact]
    public void AddInfrastructure_RegistersIdeaValidationLoopEvaluator()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var evaluator = provider.GetRequiredService<IdeaValidationLoopEvaluator>();
        Assert.NotNull(evaluator);
    }

    [Fact]
    public void AddInfrastructure_WithMissingApiKey_ThrowsWhenResolvingAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act & Assert
        var exception =
            Assert.Throws<InvalidOperationException>(
                provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>);
        Assert.Contains("API key", exception.Message);
    }

    [Fact]
    public void AddInfrastructure_WithMissingModel_ThrowsWhenResolvingAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act & Assert
        var exception =
            Assert.Throws<InvalidOperationException>(
                provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>);
        Assert.Contains("model", exception.Message);
    }

    [Fact]
    public void AddInfrastructure_WithoutIdeaWorkflowOptions_DefaultsToLegacyWithMandatoryJudge()
    {
        var provider = BuildProvider(null);

        var evaluator = provider.GetRequiredService<IdeaValidationLoopEvaluator>();
        Assert.Equal(IdeaWorkflowMode.Legacy, evaluator.Mode);
        Assert.Equal(3, evaluator.EffectiveMaxAttempts);
        Assert.IsType<IdeaAgent>(provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>());
    }

    [Theory]
    [InlineData("Legacy", IdeaWorkflowMode.Legacy, 3)]
    [InlineData("Simplified", IdeaWorkflowMode.Simplified, 2)]
    [InlineData("simplified", IdeaWorkflowMode.Simplified, 2)]
    public void AddInfrastructure_SelectsImmutableModeAndBudgetPerProvider(string value, IdeaWorkflowMode mode,
        int attempts)
    {
        var provider = BuildProvider(value);

        var evaluator = provider.GetRequiredService<IdeaValidationLoopEvaluator>();
        Assert.Equal(mode, evaluator.Mode);
        Assert.Equal(attempts, evaluator.EffectiveMaxAttempts);
        Assert.IsType<IdeaAgent>(provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>());

        // A second provider with another mode is unaffected by the first (no global mutable mode).
        var other = BuildProvider(mode == IdeaWorkflowMode.Legacy ? "Simplified" : "Legacy");
        Assert.NotEqual(mode, other.GetRequiredService<IdeaValidationLoopEvaluator>().Mode);
        Assert.Equal(mode, evaluator.Mode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Strict")]
    [InlineData("1")]
    public void AddInfrastructure_InvalidIdeaMode_FailsWhenResolvingAgent(string value)
    {
        var provider = BuildProvider(value);

        Assert.Throws<InvalidOperationException>(provider.GetRequiredService<IdeaValidationLoopEvaluator>);
        Assert.Throws<InvalidOperationException>(provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>);
    }

    [Fact]
    public void IdeaWorkflowOptions_ValidateRejectsNullAndUnknownModes()
    {
        Assert.Equal(IdeaWorkflowMode.Legacy, new IdeaWorkflowOptions().ResolveMode());
        Assert.Throws<InvalidOperationException>(() => new IdeaWorkflowOptions { Mode = null }.Validate());
        Assert.Throws<InvalidOperationException>(() => new IdeaWorkflowOptions { Mode = "" }.Validate());
        Assert.Throws<InvalidOperationException>(() => new IdeaWorkflowOptions { Mode = "Other" }.Validate());
    }

    private static ServiceProvider BuildProvider(string? mode)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });
        if (mode is not null)
        {
            services.Configure<IdeaWorkflowOptions>(opt => opt.Mode = mode);
        }

        services.AddInfrastructure();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void AddInfrastructure_ThrowsWhenServicesIsNull()
    {
        // Act & Assert
        var exception =
            Assert.Throws<ArgumentNullException>(() => InfrastructureServiceExtensions.AddInfrastructure(null!));
        Assert.Equal("services", exception.ParamName);
    }

    [Fact]
    public void AddInfrastructure_AgentIsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act
        var agent1 = provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>();
        var agent2 = provider.GetRequiredService<IAgent<StoryInput, IReadOnlyList<Idea>>>();

        // Assert
        Assert.Same(agent1, agent2);
    }

    [Fact]
    public void AddInfrastructure_RegistersResearchAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var agent = provider.GetRequiredService<IAgent<ResearchInput, ResearchResult>>();
        Assert.NotNull(agent);
        Assert.IsType<ResearchAgent>(agent);
    }

    [Fact]
    public void AddInfrastructure_RegistersSynthesizerAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var agent = provider.GetRequiredService<IAgent<SynthesisInput, SynthesisResult>>();
        Assert.NotNull(agent);
        Assert.IsType<SynthesizerAgent>(agent);
    }

    [Fact]
    public void AddInfrastructure_ResearchAgentIsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act
        var agent1 = provider.GetRequiredService<IAgent<ResearchInput, ResearchResult>>();
        var agent2 = provider.GetRequiredService<IAgent<ResearchInput, ResearchResult>>();

        // Assert
        Assert.Same(agent1, agent2);
    }

    [Fact]
    public void AddInfrastructure_SynthesizerAgentIsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act
        var agent1 = provider.GetRequiredService<IAgent<SynthesisInput, SynthesisResult>>();
        var agent2 = provider.GetRequiredService<IAgent<SynthesisInput, SynthesisResult>>();

        // Assert
        Assert.Same(agent1, agent2);
    }

    [Fact]
    public void AddInfrastructure_RegistersNarrativeAgent()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        // Act
        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Assert
        var agent = provider.GetRequiredService<IAgent<NarrativeInput, NarrativeResult>>();
        Assert.NotNull(agent);
        Assert.IsType<NarrativeAgent>(agent);
    }

    [Fact]
    public void AddInfrastructure_NarrativeAgentIsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<OpenAiOptions>(opt =>
        {
            opt.ApiKey = "test-key";
            opt.Model = "gpt-4o-mini";
        });

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        // Act
        var agent1 = provider.GetRequiredService<IAgent<NarrativeInput, NarrativeResult>>();
        var agent2 = provider.GetRequiredService<IAgent<NarrativeInput, NarrativeResult>>();

        // Assert
        Assert.Same(agent1, agent2);
    }
}