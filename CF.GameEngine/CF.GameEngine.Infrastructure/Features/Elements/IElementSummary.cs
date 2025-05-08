using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.Elements;

public interface IElementSummary : IMappable<IElement>, IIdentifer
{
    string Key { get; }
    string Name { get; }
    int? SortOrder { get; }
    Guid ElementTypeId { get; }
    Guid? ParentElementId { get; }
}
