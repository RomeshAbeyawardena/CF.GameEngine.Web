using Konscious.Security.Cryptography;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace IDFCR.Shared.Abstractions.Cryptography;

public interface IPIIProtection<T> : IStateBag
{
    string Hash(HashAlgorithmName algorithmName, string secret, string salt, int length, int? iterations = null);
    IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry);
    void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo>? protectionData = null);
    string HashWithArgon2(ArgonVariation argonVariation, byte[] password, string salt, int length, Action<Argon2>? configure = null);
    string HashWithHmac(string key, string data);
    string HashWithHmac(string data);
    bool VerifyHashUsing(T hashedEntry, Expression<Func<T, string?>> member, string valueToTest);
    bool VerifyHmacUsing(T hashedEntry, Expression<Func<T, string?>> member, string valueToTest);
}
