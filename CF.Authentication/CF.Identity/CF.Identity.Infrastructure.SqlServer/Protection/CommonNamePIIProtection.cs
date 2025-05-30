using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class CommonNamePIIProtection : PIIProtectionBase<DbCommonName>, ICommonNamePIIProtection
{
    private readonly IConfiguration _configuration;
    protected override string GetKey(DbCommonName entity)
    {
        var ourValue = _configuration.GetValue<string>("Encryption:Key") 
            ?? throw new InvalidOperationException("Encryption key not found in configuration.");
        return GenerateKey(entity, 32, '|', ourValue, entity.Id.ToString("N"));
    }

    protected override string GetHmacKey()
    {
        return ApplicationKnownValue;
    }

    public CommonNamePIIProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        _configuration = configuration;
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);
        
        ProtectSymmetric(x => x.Value);
        MapProtectionInfoTo(x => x.Value, BackingStore.CasingImpression, x => x.ValueCI);
        MapProtectionInfoTo(x => x.Value, BackingStore.Hmac, x => x.ValueHmac);
    }
}
