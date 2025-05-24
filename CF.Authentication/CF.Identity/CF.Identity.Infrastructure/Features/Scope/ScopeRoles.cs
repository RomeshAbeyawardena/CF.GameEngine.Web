using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Scope;

public class ScopeRoles : RoleRegistrarBase
{
    public const string ScopeRead = "scope:api:read";
    public const string ScopeWrite = "scope:api:write";
    public ScopeRoles()
    {
        RegisterRoles(ScopeRead, ScopeWrite);
    }
}
