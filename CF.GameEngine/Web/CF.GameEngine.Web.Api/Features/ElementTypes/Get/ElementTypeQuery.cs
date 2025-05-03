using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeQuery(IElementTypeFilter ElementTypeFilter, int? PageSize, int? PageIndex) : PagedQuery(PageIndex, PageSize);