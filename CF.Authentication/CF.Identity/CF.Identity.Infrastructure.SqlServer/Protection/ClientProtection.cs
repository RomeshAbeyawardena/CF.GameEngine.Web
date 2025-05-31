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
        return $"{base.ApplicationKnownValue}-{client.Reference}";
    }

    protected override string GetHmacKey() => string.Empty;

    public ClientProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        ProtectArgonHashed(x => x.SecretHash, x => GetKey(x), ArgonVariation.Argon2id);
    }
}
