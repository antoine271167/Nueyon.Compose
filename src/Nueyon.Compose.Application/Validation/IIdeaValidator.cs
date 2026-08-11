using Nueyon.Compose.Domain;

namespace Nueyon.Compose.Application.Validation;

public interface IIdeaValidator
{
    bool IsValid(
        IReadOnlyList<Idea> ideas);
}
