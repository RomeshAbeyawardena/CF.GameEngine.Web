using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCredentialProtectionProvider
{
    string Hash(string secret, IUser user);
    bool Verify(string secret, string hash, IUser user);
    void Protect(UserDto user, IClient client, bool regenerativeIv = false);
    void Unprotect(UserDto user, IClient client);
}

public class UserCredentialProtectionProvider(IConfiguration configuration) : PIIProtectionProviderBase, IUserCredentialProtectionProvider
{
    private byte[] GetKey(IClient client)
    {
        //something we know
        var ourValue = configuration.GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");
        if (ourValue.Length > 15)
        {
            ourValue = ourValue.Substring(0, 15);
        }

        //something they know
        string theirValue = client.Reference;
        if (client.Reference.Length > 15)
        {
            theirValue = client.Reference.Substring(0, 15);
        }

        var key = $"%{ourValue}|{theirValue}";

        if (key.Length < 32)
        {
            var additionalLengthNeeded = 32 - key.Length;
            var id = Guid.NewGuid().ToString();
            key += id.Substring(0, additionalLengthNeeded);
        }

        //%| to make it a bit more fuzzy
        return Encoding.UTF8.GetBytes(key);
    }

    public string Hash(string secret, IUser user)
    {
        var salt = Encoding.UTF8.GetBytes(user.ClientId.ToString());
        var derived = new Rfc2898DeriveBytes(secret, salt, 100_000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(derived.GetBytes(32));
    }

    public void Protect(UserDto user, IClient client, bool regenerativeIv = false)
    {
        using Aes? aes = Aes.Create();

        aes.Key = GetKey(client);
        if (regenerativeIv || string.IsNullOrWhiteSpace(user.RowVersion))
        {
            aes.GenerateIV();
            user.RowVersion = $"{Convert.ToBase64String(aes.IV)}";
        }
        else
        {
            aes.IV = Convert.FromBase64String(user.RowVersion);
        }

        user.EmailAddress = Encrypt(user.EmailAddress, aes)!;
        user.Username = Encrypt(user.Username, aes)!;
        if (!string.IsNullOrWhiteSpace(user.PreferredUsername))
        {
            user.PreferredUsername = Encrypt(user.PreferredUsername, aes);
        }
        user.HashedPassword = Hash(user.HashedPassword, user);
    }

    public void Unprotect(UserDto user, IClient client)
    {
        if (string.IsNullOrWhiteSpace(user.RowVersion))
        {
            throw new InvalidOperationException("Unable to decrypt");
        }

        using Aes? aes = Aes.Create();

        aes.Key = GetKey(client);
        aes.IV = Convert.FromBase64String(user.RowVersion);

        user.EmailAddress = Decrypt(user.EmailAddress, aes)!;
        user.Username = Decrypt(user.Username, aes)!;
        if (!string.IsNullOrWhiteSpace(user.PreferredUsername))
        {
            user.PreferredUsername = Decrypt(user.PreferredUsername, aes);
        }
    }

    public bool Verify(string secret, string hash, IUser user)
    {
        var hashed = Hash(secret, user);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(hashed),
            Convert.FromBase64String(hash));
    }
}