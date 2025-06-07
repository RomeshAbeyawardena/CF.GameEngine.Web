using Microsoft.Extensions.Configuration;
using System.Text;
using PIIProtection = IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.SqlServer.Protection;

internal abstract class PIIProtectionBase<T>(IConfiguration configuration, Encoding encoding) : PIIProtection.PIIProtectionBase<T>(encoding)
{
    protected string ApplicationKnownValue => configuration
        .GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");
}
