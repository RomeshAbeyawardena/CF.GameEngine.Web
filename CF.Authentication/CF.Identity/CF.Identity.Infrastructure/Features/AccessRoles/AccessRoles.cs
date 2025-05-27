using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public class AccessRoles : RoleRegistrarBase
{
    public const string RoleRead = "api:role:read";
    public const string RoleWrite = "api:role:write";
    public AccessRoles()
    {
        TryRegisterRole(RoleRead, b => b
            .AddDisplayName(Resources.AccessRoleReadRoleName)
            .AddDescription(Resources.AccessRoleReadRoleDescription));

        TryRegisterRole(RoleWrite, b => b
            .AddDisplayName(Resources.AccessRoleWriteRoleName)
            .AddDescription(Resources.AccessRoleWriteRoleDescription));
    }
}
