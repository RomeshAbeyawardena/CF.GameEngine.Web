using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features;

public class SystemRoles : RoleRegistrarBase
{
    public const string GlobalRead = "api:read";
    public const string GlobalWrite = "api:write";

    public SystemRoles()
    {
        TryRegisterRole(GlobalRead, b => 
            b.AddDisplayName(Resources.GlobalReadRoleName)
            .AddDescription(Resources.GlobalReadRoleDescription)
            .Privileged());

        TryRegisterRole(GlobalWrite, b =>
            b.AddDisplayName(Resources.GlobalWriteRoleName)
            .AddDescription(Resources.GlobalWriteRoleDescription)
            .Privileged());
    }
}
