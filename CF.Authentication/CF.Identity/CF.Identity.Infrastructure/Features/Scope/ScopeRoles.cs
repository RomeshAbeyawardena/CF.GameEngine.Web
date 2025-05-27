using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Scope;

public class ScopeRoles : RoleRegistrarBase
{
    public const string ScopeRead = "scope:api:read";
    public const string ScopeWrite = "scope:api:write";
    public ScopeRoles()
    {
        TryRegisterRole(ScopeRead, RoleCategory.Read, b => b
            .AddDisplayName(Resources.ScopeReadRoleName)
            .AddDescription(Resources.ScopeReadRoleDescription));

        TryRegisterRole(ScopeWrite, RoleCategory.Write, b => b
            .AddDisplayName(Resources.ScopeWriteRoleName)
            .AddDescription(Resources.ScopeWriteRoleDescription));
    }
}
