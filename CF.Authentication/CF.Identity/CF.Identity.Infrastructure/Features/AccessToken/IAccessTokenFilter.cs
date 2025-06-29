﻿using IDFCR.Shared.Abstractions.Filters;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public interface IAccessTokenFilter : IFilter<IAccessTokenFilter>, IValidityFilter
{
    bool ShowAll { get; }
    string? ReferenceToken { get; }
    Guid? UserId { get; }
    Guid? ClientId { get; }
    string? Type { get; }
}
