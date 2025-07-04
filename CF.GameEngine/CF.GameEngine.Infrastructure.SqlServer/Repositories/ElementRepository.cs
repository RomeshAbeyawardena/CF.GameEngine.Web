﻿using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.SqlServer.Filters;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace CF.GameEngine.Infrastructure.SqlServer.Repositories;

internal class ElementRepository(TimeProvider timeProvider, CFGameEngineDbContext context)
    : RepositoryBase<IElement, Element, ElementDto>(timeProvider, context), IElementRepository
{
    public async Task<IUnitResultCollection<ElementDto>> FindElementsAsync(IElementFilter filter, CancellationToken cancellationToken)
    {
        var elementFilter = new ElementFilter(filter);
        var elements = await base.Set<Element>(filter)
            .Where(elementFilter.ApplyFilter(Builder))
            .ToListAsync(cancellationToken);

        return UnitResultCollection.FromResult(elements.Select(x => x.Map<ElementDto>()).ToList(), UnitAction.Get);
    }

    public async Task<IUnitResult<ElementDto>> GetElementById(Guid elementId, CancellationToken cancellationToken)
    {
        var element = await FindAsync(cancellationToken, [elementId]);
        if (element is null)
        {
            return UnitResult.NotFound<ElementDto>(elementId);
        }

        return UnitResult.FromResult(element, UnitAction.Get);
    }

    public Task<IUnitPagedResult<ElementDto>> GetPagedAsync(IElementPagedFilter pagedQuery, CancellationToken cancellationToken = default)
    {
        var query = new ElementFilter(pagedQuery);
        return base.GetPagedAsync(pagedQuery, new EntityOrder(pagedQuery, "SortOrder"),
            base.Set<Element>(pagedQuery).Where(query.ApplyFilter(Builder)),
            cancellationToken);
    }
}
