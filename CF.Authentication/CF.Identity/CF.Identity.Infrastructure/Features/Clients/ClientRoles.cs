using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Clients;

public class ClientRoles : RoleRegistrarBase
{
    public const string ClientRead = "client:api:read";
    public const string ClientWrite = "client:api:write";

    public ClientRoles()
    {
        RegisterRoles(ClientRead, ClientWrite);
    }
}
