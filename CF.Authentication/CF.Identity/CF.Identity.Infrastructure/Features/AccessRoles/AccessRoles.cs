using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public class Roles : RoleRegistrarBase
{
    public const string RoleRead = "api:role:read";
    public const string RoleWrite = "api:role:write";
    public Roles()
    {
        TryRegisterRole(RoleRead, RoleCategory.Read, b => b
            .AddDisplayName(Resources.AccessRoleReadRoleName)
            .AddDescription(Resources.AccessRoleReadRoleDescription));

        TryRegisterRole(RoleWrite, RoleCategory.Write, b => b
            .AddDisplayName(Resources.AccessRoleWriteRoleName)
            .AddDescription(Resources.AccessRoleWriteRoleDescription));
    }
}
