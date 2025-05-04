using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementQuery(Guid? ParentElementId, string? ExternalReference, string? Key, string? NameContains, 
    int? PageSize, int? PageIndex, bool NoTracking = true, string? SortField = null, string? SortOrder = null) : PagedQuery(PageSize, PageIndex),
    IUnitPagedRequest<ElementResponse>,
    IElementPagedFilter;
