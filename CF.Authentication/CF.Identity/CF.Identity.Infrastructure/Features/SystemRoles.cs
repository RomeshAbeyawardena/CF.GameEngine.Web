using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features;

public class SystemRoles : RoleRegistrarBase
{
    public const string GlobalRead = "api:read";
    public const string GlobalWrite = "api:write";

    public SystemRoles()
    {
        base.RegisterRoles(GlobalRead, GlobalWrite);
    }
}
