using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.Elements;

public interface IElement : IEditableElement
{
}

public interface IEditableElement : IElementDetails
{
}

public interface IElementSummary : IMappable<IElement>, IIdentifer
{
    string Key { get; }
    string Name { get; }
    int? SortOrder { get; }
    Guid ElementTypeId { get; }
    Guid ParentElementId { get; }
}

public interface IElementDetails : IElementSummary
{
    string? ExternalReference { get; }
    string? Description { get; }
}