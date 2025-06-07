using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions.Roles;

namespace CF.Identity.Infrastructure.Features;

public class SystemRoles : RoleRegistrarBase
{
    public const string GlobalRead = "api:read";
    public const string GlobalWrite = "api:write";

    public SystemRoles()
    {
        TryRegisterRole(GlobalRead, b =>
            b.SetCategory(IDFCR.Shared.Abstractions.RoleCategory.Read)
            .AddDisplayName(Resources.GlobalReadRoleName)
            .AddDescription(Resources.GlobalReadRoleDescription)
            .Privileged());

        TryRegisterRole(GlobalWrite, b =>
            b.SetCategory(IDFCR.Shared.Abstractions.RoleCategory.Write)
            .AddDisplayName(Resources.GlobalWriteRoleName)
            .AddDescription(Resources.GlobalWriteRoleDescription)
            .Privileged());
    }
}
