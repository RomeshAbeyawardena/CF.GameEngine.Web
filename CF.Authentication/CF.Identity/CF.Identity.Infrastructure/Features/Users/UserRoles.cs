using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public class UserRoles : RoleRegistrarBase
{
    public const string UserRead = "user:api:read";
    public const string UserWrite = "user:api:write";
    public UserRoles()
    {
        RegisterRoles(UserRead, UserWrite);
    }
}
