using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class UserPIIProtection : PIIProtectionBase<DbUser>, IUserPIIProtection
{
    private readonly ICommonNamePIIProtection _commonNamePIIProtection;
    private string GetHash(DbUser user)
    {
        var secondPart = Client.SecretHash ?? user.Id.ToString();
        return $"{ApplicationKnownValue}:{secondPart}";
    }

    protected override string GetKey(DbUser entity)
    {
        return GenerateKey(entity, 32, '|', ApplicationKnownValue, Client.SecretHash ?? throw new NullReferenceException());
    }

    protected override string GetHmacKey()
    {
        return $"{ApplicationKnownValue}:{Client.SecretHash}";
    }

    protected override void OnUnprotect(DbUser user)
    {
        if (user.FirstCommonName is not null)
        {
            _commonNamePIIProtection.Unprotect(user.FirstCommonName);
        }

        if (user.MiddleCommonName is not null)
        {
            _commonNamePIIProtection.Unprotect(user.MiddleCommonName);
        }

        if (user.LastCommonName is not null)
        {
            _commonNamePIIProtection.Unprotect(user.LastCommonName);
        }
    }

    public UserPIIProtection(ICommonNamePIIProtection commonNamePIIProtection, IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        _commonNamePIIProtection = commonNamePIIProtection;
        SetMetaData(x => x.Metadata);
        SetRowVersion(x => x.RowVersion);
        
        ProtectSymmetric(x => x.EmailAddress);
        MapProtectionInfoTo(x => x.EmailAddress, BackingStore.CasingImpression, x => x.EmailAddressCI);
        MapProtectionInfoTo(x => x.EmailAddress, BackingStore.Hmac, x => x.EmailAddressHmac);
        ProtectSymmetric(x => x.Username);
        MapProtectionInfoTo(x => x.Username, BackingStore.CasingImpression, x => x.UsernameCI);
        MapProtectionInfoTo(x => x.Username, BackingStore.Hmac, x => x.UsernameHmac);
        ProtectSymmetric(x => x.PreferredUsername);
        MapProtectionInfoTo(x => x.PreferredUsername, BackingStore.CasingImpression, x => x.PreferredUsernameCI);
        MapProtectionInfoTo(x => x.PreferredUsername, BackingStore.Hmac, x => x.PreferredUsernameHmac);
        ProtectSymmetric(x => x.PrimaryTelephoneNumber);
        MapProtectionInfoTo(x => x.PrimaryTelephoneNumber, BackingStore.Hmac, x => x.PrimaryTelephoneNumberHmac);
#pragma warning disable IDE0200 //Client part not known during instantiation
        ProtectArgonHashed(x => x.HashedPassword, x => GetHash(x), ArgonVariation.Argon2id);
#pragma warning restore IDE0200
    }

    private const string ClientKey = "client";
    public IClient Client { 
        get => Get<IClient>(ClientKey) 
            ?? throw new NullReferenceException("Client dependency not found");
        set => Set(ClientKey, value);
    }
}
