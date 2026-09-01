using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Agents.Research;

public sealed record ResearchInput(
    ChatInput Input,
    SelectedIdea SelectedIdea);