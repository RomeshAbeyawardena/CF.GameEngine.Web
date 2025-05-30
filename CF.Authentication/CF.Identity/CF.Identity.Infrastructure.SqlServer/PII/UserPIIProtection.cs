using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.PII;

public interface IUserPIIProtection : IPIIProtection<DbUser>
{
    DbClient Client { get; set; }
}

internal class UserPIIProtection : PIIProtectionBase<DbUser>, IUserPIIProtection
{
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

    public UserPIIProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
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
        ProtectHashed(x => x.HashedPassword, x => GetHash(x),
            System.Security.Cryptography.HashAlgorithmName.SHA384);
#pragma warning restore IDE0200
    }

    private const string ClientKey = "client";
    public DbClient Client { 
        get => Get<DbClient>(ClientKey) 
            ?? throw new NullReferenceException("Client dependency not found");
        set => Set(ClientKey, value);
    }
}
