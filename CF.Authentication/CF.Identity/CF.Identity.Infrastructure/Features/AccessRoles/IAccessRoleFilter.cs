﻿using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Abstractions.Paging;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public interface IAccessRoleFilter : IFilter<IAccessRoleFilter>
{
    Guid? ClientId { get; }
    string? Name { get; }
    string? NameContains { get; }
}

public interface IPagedAccessRoleFilter : IAccessRoleFilter, IPagedQuery, IEntityOrder;
