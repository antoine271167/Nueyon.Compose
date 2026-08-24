using Microsoft.Agents.AI.Workflows;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;
using Xunit;

namespace Nueyon.Compose.Application.Tests.Workflows;

public sealed class IdeaSelectionExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_WithMultipleIdeas_SelectsFirstIdea()
    {
        // Arrange
        var firstIdea = CreateIdea("First");
        var secondIdea = CreateIdea("Second");

        var executor =
            IdeaSelectionExecutorFactory.CreateIdeaSelectionExecutor();

        var workflow = new WorkflowBuilder(executor).Build();

        // Act
        var run = await InProcessExecution.RunAsync(
            workflow,
            new[] { firstIdea, secondIdea });

        // Assert
        var completedEvent = Assert.Single(
            run.OutgoingEvents
                .OfType<ExecutorCompletedEvent>());

        var selectedIdea =
            Assert.IsType<SelectedIdea>(completedEvent.Data);

        Assert.Same(firstIdea, selectedIdea.Idea);
    }

    private static Idea CreateIdea(string title) =>
        new()
        {
            Title = title,
            Description = $"{title} description",
            Audience = "Test audience",
            Rationale = "Test rationale"
        };
}