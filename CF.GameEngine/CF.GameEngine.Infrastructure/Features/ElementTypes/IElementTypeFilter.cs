using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IElementTypePagedFilter : IElementTypeFilter, IPagedQuery;

public interface IElementTypeFilter : IFilter<IElementTypeFilter>
{
    string? ExternalReference { get; }
    string? Key { get; }
    string? NameContains { get; }
    
}
