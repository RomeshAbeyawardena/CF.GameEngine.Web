using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public class UserRoles : RoleRegistrarBase
{
    public const string UserRead = "user:api:read";
    public const string UserWrite = "user:api:write";
    public UserRoles()
    {
        TryRegisterRole(UserRead, b => b
            .AddDisplayName(Resources.UserReadRoleName)
            .AddDescription(Resources.UserReadRoleDescription));

        TryRegisterRole(UserWrite, b => b
            .AddDisplayName(Resources.UserWriteRoleName)
            .AddDescription(Resources.UserWriteRoleDescription));
    }
}
