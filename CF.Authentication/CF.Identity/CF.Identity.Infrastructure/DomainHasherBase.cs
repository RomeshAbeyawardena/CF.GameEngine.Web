using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.Infrastructure;

public abstract class DomainHasherBase<T>(Encoding? encoding) : IDomainHasher<T>
{
    protected abstract string GenerateSalt(T client);
    protected abstract Func<T, string?> SecretProperty { get; }
    protected virtual string HashFormat => "{0}:{1}";
    protected Encoding Encoding => encoding ?? Encoding.UTF8;

    public string Hash(string secret, T value)
    {
        var salt = GenerateSalt(value);
        var formatted = string.Format(HashFormat, secret, salt);
        var bytes = Encoding.GetBytes(formatted);
        return Convert.ToBase64String(SHA256.HashData(bytes));

    }

    public bool Verify(string secret, T value)
    {
        var hashedSecret = Hash(secret, value);

        var storedHash = SecretProperty(value);

        if (string.IsNullOrWhiteSpace(storedHash))
        {
            //Hash check was not required as its not supported
            return true;
        }

        return CryptographicOperations.FixedTimeEquals(Encoding.GetBytes(storedHash), Encoding.GetBytes(hashedSecret));
    }
}
