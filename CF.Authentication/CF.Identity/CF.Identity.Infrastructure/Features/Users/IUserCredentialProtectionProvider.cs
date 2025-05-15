using CF.Identity.Infrastructure.Features.Clients;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserCredentialProtectionProvider
{
    string Hash(string secret, IUser user);
    bool Verify(string secret, string hash, IUser user);
    void Protect(UserDto user, IClient client);
    void Unprotect(UserDto user, IClient? client = null);
}

public class UserCredentialProtectionProvider(IConfiguration configuration) : IUserCredentialProtectionProvider
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

    private static string? Encrypt(string? value, Aes aes)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }

        return value;
    }

    public string Hash(string secret, IUser user)
    {
        throw new NotImplementedException();
    }

    public void Protect(UserDto user, IClient client)
    {
        using Aes? aes = Aes.Create();

        aes.Key = GetKey(client);
        if (string.IsNullOrWhiteSpace(user.RowVersion))
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
        user.PreferredUsername = Encrypt(user.PreferredUsername, aes);
    }

    public void Unprotect(UserDto user, IClient? client = null)
    {
        throw new NotImplementedException();
    }

    public bool Verify(string secret, string hash, IUser user)
    {
        throw new NotImplementedException();
    }
}