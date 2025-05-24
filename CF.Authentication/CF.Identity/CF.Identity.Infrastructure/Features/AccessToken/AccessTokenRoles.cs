using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessToken;

public class AccessTokenRoles : RoleRegistrarBase
{
    public const string AccessTokenRead = "access-token:api:read";
    public const string AccessTokenWrite = "access-token:api:write";

    public AccessTokenRoles()
    {
        RegisterRoles(AccessTokenRead, AccessTokenWrite);
    }
}
