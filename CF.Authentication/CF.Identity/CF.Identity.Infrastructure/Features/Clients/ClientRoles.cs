using CF.Identity.Infrastructure.Properties;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;

namespace CF.Identity.Infrastructure.Features.Clients;

public class ClientRoles : RoleRegistrarBase
{
    public const string ClientRead = "client:api:read";
    public const string ClientWrite = "client:api:write";

    public ClientRoles()
    {
        TryRegisterRole(ClientRead, RoleCategory.Read, b => b
            .AddDisplayName(Resources.ClientReadRoleName)
            .AddDescription(Resources.ClientReadRoleDescription));

        TryRegisterRole(ClientWrite, RoleCategory.Write, b => b
            .AddDisplayName(Resources.ClientWriteRoleName)
            .AddDescription(Resources.ClientWriteRoleDescription));
    }
}
