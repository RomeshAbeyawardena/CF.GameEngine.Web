using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.GameEngine.Infrastructure.Features.Elements;

public interface IElementPagedFilter : IElementFilter, 
    IPagedQuery, IEntityOrder
{
    
}

public interface IElementFilter : IFilter<IElementFilter>
{
    Guid? ParentElementId { get; }
    string? ExternalReference { get; }
    string? Key { get; }
    string? NameContains { get; }
}
