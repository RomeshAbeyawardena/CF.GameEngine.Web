using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal class CommonNamePIIProtection : PIIProtectionBase<DbCommonName>, ICommonNamePIIProtection
{
    protected override string GetKey(DbCommonName entity)
    {
        return GenerateKey(entity, 32, '|', ApplicationKnownValue, entity.Id.ToString("N"));
    }

    protected override string GetHmacKey()
    {
        return ApplicationKnownValue;
    }

    public CommonNamePIIProtection(IConfiguration configuration, Encoding encoding) : base(configuration, encoding)
    {
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);
        
        ProtectSymmetric(x => x.Value);
        MapProtectionInfoTo(x => x.Value, BackingStore.CasingImpression, x => x.ValueCI);
        MapProtectionInfoTo(x => x.Value, BackingStore.Hmac, x => x.ValueHmac);
    }
}
