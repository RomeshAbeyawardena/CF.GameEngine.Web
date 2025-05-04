using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementQuery(Guid? ParentId, string? ExternalReference, string? Key, string? NameContains, int? PageSize, int? PageIndex, bool NoTracking = true, string? SortField = null, string? SortOrder = null) : PagedQuery(PageSize, PageIndex),
    IRequest<IUnitPagedResult<ElementResponse>>,
    IElementPagedFilter;
