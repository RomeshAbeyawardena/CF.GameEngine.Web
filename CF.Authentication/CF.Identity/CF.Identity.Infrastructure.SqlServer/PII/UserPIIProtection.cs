using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.PII;

public interface IUserPIIProtection : IPIIProtection<DbUser>;

internal class UserPIIProtection : PIIProtectionBase<DbUser>, IUserPIIProtection
{
    private readonly IConfiguration _configuration;
    protected override string GetKey(DbUser entity)
    {
        var ourValue = _configuration.GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");

        var client = Get<ClientDto>("client") 
            ?? throw new NullReferenceException("Client dependency not found");

        return GenerateKey(entity, 32, '|', ourValue, client.SecretHash ?? throw new NullReferenceException());
    }

    public UserPIIProtection(IConfiguration configuration, Encoding encoding) : base(encoding)
    {
        _configuration = configuration;
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
        ProtectHashed(x => x.HashedPassword, "PasswordHash", "PasswordHashSalt",
            System.Security.Cryptography.HashAlgorithmName.SHA384);
    }
}
