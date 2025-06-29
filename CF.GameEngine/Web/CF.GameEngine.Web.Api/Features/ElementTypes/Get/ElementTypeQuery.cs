﻿using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeQuery(string? ExternalReference, string? Key, string? NameContains, 
    int? PageSize, int? PageIndex, bool NoTracking = true, string? SortField = null, string? SortOrder = null) 
    : PagedQuery(PageSize, PageIndex), IUnitPagedRequest<ElementTypeResponse>, IElementTypePagedFilter;