using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class AccessTokenProtection : PIIProtectionBase<DbAccessToken>, IAccessTokenSpaProtection
{
    protected override string GetKey(DbAccessToken entity)
    {
        return $"{ApplicationKnownValue}-{Client.Reference}";
    }

    public string GetHashedAccessToken(string accessToken)
    {
        return GetHashUsing(null!, x => x.ReferenceToken, accessToken, out _);
    }

    public bool VerifyAccessToken(string accessToken)
    {
        return VerifyHashUsing(null!, x => x.ReferenceToken, accessToken);
    }

    public AccessTokenProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        ProtectArgonHashed(x => x.ReferenceToken, x => GetKey(x), ArgonVariation.Argon2id);
        ProtectArgonHashed(x => x.RefreshToken, x => GetKey(x), ArgonVariation.Argon2id);
    }

    private const string ClientIdKey = nameof(ClientIdKey);

    public IClient Client { 
        get => Get<IClient>(ClientIdKey) ?? throw new NullReferenceException(); 
        set => Set(ClientIdKey, value); 
    }
}
