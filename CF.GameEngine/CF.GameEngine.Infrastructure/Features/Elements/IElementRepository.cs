﻿using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.GameEngine.Infrastructure.Features.Elements;

public interface IElementRepository : IRepository<ElementDto>
{
    Task<IUnitResultCollection<ElementDto>> FindElementsAsync(IElementFilter elementFilter, CancellationToken cancellationToken);
    Task<IUnitResult<ElementDto>> GetElementById(Guid elementId, CancellationToken cancellationToken);
    Task<IUnitPagedResult<ElementDto>> GetPagedAsync(IElementPagedFilter elementFilter, CancellationToken cancellationToken = default);
}
