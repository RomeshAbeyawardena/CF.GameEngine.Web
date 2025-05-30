using System.Linq.Expressions;
using System.Security.Cryptography;

namespace IDFCR.Shared.Abstractions.Cryptography;

public interface IPIIProtection<T> : IStateBag
{
    string Hash(HashAlgorithmName algorithmName, string secret, string salt, int length);
    IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry);
    void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo>? protectionData = null);
    string HashWithHMAC(string key, string data);
    bool VerifyHashUsing(T hashedEntry, Expression<Func<T, string?>> member, string valueToTest);
}
