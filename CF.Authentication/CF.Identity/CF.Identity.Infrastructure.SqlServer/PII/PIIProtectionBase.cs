using PIIProtection = IDFCR.Shared.Abstractions.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CF.Identity.Infrastructure.SqlServer.PII;

internal abstract class PIIProtectionBase<T>(IConfiguration configuration, Encoding encoding) : PIIProtection.PIIProtectionBase<T>(encoding)
{
    protected string ApplicationKnownValue => configuration
        .GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");
}
