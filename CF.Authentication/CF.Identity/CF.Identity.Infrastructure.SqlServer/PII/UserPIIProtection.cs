using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CF.Identity.Infrastructure.SqlServer.PII;

public interface IUserPIIProtection : IPIIProtection<DbUser>;

internal class UserPIIProtection(IConfiguration configuration, Encoding encoding) 
    : PIIProtectionBase<DbUser>(encoding), IUserPIIProtection
{
    protected override string GetKey(DbUser entity)
    {
        var ourValue = configuration.GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");

        var client = Get<ClientDto>("client") 
            ?? throw new NullReferenceException("Client dependency not found");

        return GenerateKey(entity, 32, '|', ourValue, client.SecretHash ?? throw new NullReferenceException());
    }
}
