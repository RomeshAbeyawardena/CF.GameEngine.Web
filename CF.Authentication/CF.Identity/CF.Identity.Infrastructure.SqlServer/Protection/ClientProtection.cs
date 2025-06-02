using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;

using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class ClientProtection : PIIProtectionBase<DbClient>, IClientProtection
{
    protected override string GetKey(DbClient client)
    {
        return $"{base.ApplicationKnownValue}-{client.Reference}";
    }

    public bool VerifySecret(IClient client, string secret)
    {
        return VerifyHashUsing(client.Map<DbClient>(), x => x.SecretHash, secret);
    }

    public string HashSecret(string secret)
    {
        return GetHashUsing(null, x => x.SecretHash, secret, out _);
    }

    public ClientProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        ProtectArgonHashed(x => x.SecretHash, x => GetKey(x), ArgonVariation.Argon2id);
    }
}
