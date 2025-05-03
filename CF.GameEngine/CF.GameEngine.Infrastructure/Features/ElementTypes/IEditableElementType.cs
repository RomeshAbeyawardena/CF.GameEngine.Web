using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IEditableElementType : IMappable<IElementType>, IIdentifer
{
    string ExternalReference { get; }
    string Key { get; }
    string Name { get; }
    string? Description { get; }
}
