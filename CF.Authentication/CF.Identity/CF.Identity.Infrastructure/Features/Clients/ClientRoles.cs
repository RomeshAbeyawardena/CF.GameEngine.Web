using IDFCR.Shared.Abstractions;
using CF.Identity.Infrastructure.Properties;

namespace CF.Identity.Infrastructure.Features.Clients;

public class ClientRoles : RoleRegistrarBase
{
    public const string ClientRead = "client:api:read";
    public const string ClientWrite = "client:api:write";

    public ClientRoles()
    {
        TryRegisterRole(ClientRead, b => b
            .AddDisplayName(Resources.ClientReadRoleName)
            .AddDescription(Resources.ClientReadRoleDescription));

        TryRegisterRole(ClientWrite, b => b
            .AddDisplayName(Resources.ClientWriteRoleName)
            .AddDescription(Resources.ClientWriteRoleDescription));
    }
}
