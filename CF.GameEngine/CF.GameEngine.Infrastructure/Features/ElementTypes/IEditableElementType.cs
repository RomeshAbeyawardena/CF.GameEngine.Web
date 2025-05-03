using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IEditableElementType : IMappable<IElementType>, 
    IElementTypeDetails, IIdentifer
{
    
}

public interface IElementTypeSummary : IMappable<IElementType>, IIdentifer
{
    string Key { get; }
    string Name { get; }
}

public interface IElementTypeDetails : IElementTypeSummary
{
    string? ExternalReference { get; }
    string? Description { get; }
}
