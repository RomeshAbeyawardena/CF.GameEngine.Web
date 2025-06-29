﻿using IDFCR.Shared.Abstractions.Results;

namespace IDFCR.Shared.Http.Mediatr.Scopes
{
    public interface IScopedStateWriter
    {
        Task<IUnitResult> WriteAsync(string key, object value, CancellationToken cancellationToken);
    }
}
