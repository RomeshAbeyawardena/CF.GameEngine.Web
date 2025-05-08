using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementQuery(Guid? ParentElementId = null, string? ExternalReference = null, string? Key = null, string? NameContains = null, 
    int? PageSize = null, int? PageIndex = null, bool NoTracking = true, string? SortField = null, string? SortOrder = null) : PagedQuery(PageSize, PageIndex),
    IUnitPagedRequest<ElementResponse>,
    IElementPagedFilter;
