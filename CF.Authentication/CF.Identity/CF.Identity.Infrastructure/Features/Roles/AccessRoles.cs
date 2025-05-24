using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Roles;

public class AccessRoles : RoleRegistrarBase
{
    public const string RoleRead = "api:role:read";
    public const string RoleWrite = "api:role:write";
    public AccessRoles()
    {
        RegisterRoles(RoleRead, RoleWrite);
    }
}
