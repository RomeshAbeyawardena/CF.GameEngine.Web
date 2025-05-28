using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using Microsoft.AspNetCore.Authorization;

namespace IDFCR.Http.Authentication.Extensions;

public static class Authorise
{
    public static IAuthorizeData Using<T>(RoleCategory category, params string[] additionalRoles)
        where T : IRoleRegistrar, new()
    {
        var roles = RoleRegistrar.FlattenedRoles<T>(category, additionalRoles);
        return new AuthorizeAttribute(roles);
    }
}
