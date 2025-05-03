using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IEditableElementType : IElementTypeDetails, IIdentifer
{
    
}

public interface IElementTypeSummary : IMappable<IElementType>, IIdentifer
{
    int? SortOrder { get; }
    string Key { get; }
    string Name { get; }
}

public interface IElementTypeDetails : IElementTypeSummary
{
    string? ExternalReference { get; }
    string? Description { get; }
}
