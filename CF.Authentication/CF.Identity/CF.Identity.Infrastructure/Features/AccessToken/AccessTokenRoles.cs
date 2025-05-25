using IDFCR.Shared.Abstractions;
using CF.Identity.Infrastructure.Properties;
namespace CF.Identity.Infrastructure.Features.AccessToken;

public class AccessTokenRoles : RoleRegistrarBase
{
    public const string AccessTokenRead = "access-token:api:read";
    public const string AccessTokenWrite = "access-token:api:write";

    public AccessTokenRoles()
    {
        TryRegisterRole(AccessTokenRead, b =>b
            .AddDisplayName(Resources.AccessTokenReadRoleName)
            .AddDescription(Resources.AccessTokenWriteRoleDescription));

        TryRegisterRole(AccessTokenWrite, b => b
                .AddDisplayName(Resources.AccessTokenWriteRoleName)
               .AddDescription(Resources.AccessTokenWriteRoleDescription));
    }
}
