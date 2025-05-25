using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Roles;

public class AccessRoles : RoleRegistrarBase
{
    public const string RoleRead = "api:role:read";
    public const string RoleWrite = "api:role:write";
    public AccessRoles()
    {
        TryRegisterRole(RoleRead, b => b
            .AddDisplayName(Resources.RoleReadRoleName)
            .AddDescription(Resources.RoleReadRoleDescription));

        TryRegisterRole(RoleWrite, b => b
            .AddDisplayName(Resources.RoleWriteRoleName)
            .AddDescription(Resources.RoleWriteRoleDescription));
    }
}
