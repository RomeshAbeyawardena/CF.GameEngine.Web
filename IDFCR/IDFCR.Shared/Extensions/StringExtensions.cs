using System.Security.Authentication;
using System.Security.Cryptography;

namespace IDFCR.Shared.Extensions;

public static class StringExtensions
{
    public static string PrependUrl(this string value, string source)
    {
        return Prepend(value, source, '/');
    }
    public static string Prepend(this string value, string source, char? separator)
    {
        return $"{source}{separator}{value}";
    }
    public static string Hash(this string value, HashAlgorithmType hashAlgorithmType, 
        string? format = null, params object[] args)
    {
        using HashAlgorithm hashAlgorithm = hashAlgorithmType switch
        {
            HashAlgorithmType.None => throw new NotSupportedException("No hashing algorithm is used."),
            HashAlgorithmType.Md5 => MD5.Create(),
            HashAlgorithmType.Sha1 => SHA1.Create(),
            HashAlgorithmType.Sha256 => SHA256.Create(),
            HashAlgorithmType.Sha384 => SHA384.Create(),
            HashAlgorithmType.Sha512 => SHA512.Create(),
            _ => throw new NotSupportedException($"Hash algorithm {hashAlgorithmType} is not supported.")
        };

        var formattedValue = string.IsNullOrWhiteSpace(format) 
            ? value
            : string.Format(format, args.Prepend(value));

        return Convert
            .ToBase64String(hashAlgorithm
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(formattedValue)));
    }
}
