using MessagePack;
using System.Security.Cryptography;

namespace StateManagent.Web.Infrastructure.Hashers;

public class MessagePackCrytographicDerivedHash(MessagePackCrytographicDerivedHashOptions options) : IUniqueContentHasher
{
    private static HashAlgorithm GetHashAlgorithm(string hashAlgorithmName)
    {
        return hashAlgorithmName switch
        {
            HashAlgorithmName.SHA256 => SHA256.Create(),
            HashAlgorithmName.SHA384 => SHA384.Create(),
            HashAlgorithmName.SHA512 => SHA512.Create(),
            _ => throw new NotSupportedException($"Hash algorithm {hashAlgorithmName} is not supported.")
        };
    }

    public string CreateHash<T>(T value)
    {
        var serialisedData = MessagePackSerializer.Serialize(value, options.MessagePackSerializerOptions);
        using var hashAlgorithm = GetHashAlgorithm(options.HashAlgorithmName);

        return Convert.ToBase64String(hashAlgorithm.ComputeHash(serialisedData));
    }
}
