using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class ClientProtection : PIIProtectionBase<DbClient>, IClientProtection
{
    protected override string GetKey(DbClient client)
    {
        return $"{client.Reference}-{client.Id}";
    }

    protected override string GetHmacKey()
    {
        //not in use, but required by base class
        return string.Empty;
    }

    public ClientProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        ProtectArgonHashed(x => x.SecretHash, x => GetKey(x), ArgonVariation.Argon2id);
    }
}
