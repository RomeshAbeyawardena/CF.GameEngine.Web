using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.Models;

internal class CommonNamePIIProtection : PIIProtectionBase<DbCommonName>
{
    private readonly IConfiguration _configuration;
    protected override string GetKey(DbCommonName entity)
    {
        var ourValue = _configuration.GetValue<string>("Encryption:Key") 
            ?? throw new InvalidOperationException("Encryption key not found in configuration.");
        return GenerateKey(entity, 32, '|', Encoding, ourValue);
    }

    public CommonNamePIIProtection(IConfiguration configuration, Encoding encoding) : base(encoding)
    {
        _configuration = configuration;
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);
        
        ProtectSymmetric(x => x.ValueNormalised);
        ProtectSymmetric(x => x.Value);
    }
}
