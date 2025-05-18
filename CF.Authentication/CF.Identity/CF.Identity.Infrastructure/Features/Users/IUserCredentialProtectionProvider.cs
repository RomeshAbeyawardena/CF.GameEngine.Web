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
    void Protect(UserDto user, IClient client, out IUserHmac userHmac, bool regenerativeIv = false);
    void Unprotect(UserDto user, IClient client);
    string HashUsingHmac(UserDto user, IClient client, Func<UserDto, string> target);
    string HashUsingHmac(IClient client, string value);
}

public class UserCredentialProtectionProvider(IConfiguration configuration, Encoding encoding) : PIIProtectionProviderBase, IUserCredentialProtectionProvider
{
    private byte[] GetKey(UserDto user, IClient client)
    {
        var ourValue = configuration.GetValue<string>("Encryption:Key") ?? throw new InvalidOperationException("Encryption key not found in configuration.");

        var metaData = user.Metadata;

        if (!string.IsNullOrWhiteSpace(metaData))
        {
            metaData = encoding.GetString(Convert.FromBase64String(metaData));
        }

        var keyData = GenerateKey(32, '|', Encoding.UTF8, metaData, client.Reference, ourValue);

        //if all spaces are populated sufficiently with key data, this metadata will be an empty string and won't need persisting to the database
        if (!string.IsNullOrWhiteSpace(keyData.Item1))
        {
            user.Metadata = Convert.ToBase64String(encoding.GetBytes(keyData.Item1));
        }

        return keyData.Item2;
    }

    public string Hash(string secret, IUser user)
    {
        var salt = Encoding.UTF8.GetBytes(user.ClientId.ToString());
        var derived = new Rfc2898DeriveBytes(secret, salt, 100_000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(derived.GetBytes(32));
    }

    public string HashUsingHmac(IClient client, string value)
    {
        var key = configuration.GetValue<string>("Encryption:Key") 
            ?? throw new InvalidOperationException("Encryption key not found in configuration.");

        var reference = client.Reference
            ?? throw new InvalidOperationException("Encryption key not found in configuration.");

        return Convert.ToBase64String(HMACSHA512.HashData(encoding.GetBytes($"{key}|{reference}"), encoding.GetBytes(value)));
    }

    public string HashUsingHmac(UserDto user, IClient client, Func<UserDto, string> target)
    {
        return HashUsingHmac(client, target(user));
    }

    public void Protect(UserDto user, IClient client, out IUserHmac userHmac, bool regenerativeIv = false)
    {
        using Aes? aes = Aes.Create();

        aes.Key = GetKey(user, client);
        if (regenerativeIv || string.IsNullOrWhiteSpace(user.RowVersion))
        {
            aes.GenerateIV();
            user.RowVersion = $"{Convert.ToBase64String(aes.IV)}";
        }
        else
        {
            aes.IV = Convert.FromBase64String(user.RowVersion);
        }

        userHmac = new UserHmac
        {
            EmailAddressHmac = HashUsingHmac(user, client, x => x.EmailAddress),
            PreferredUsernameHmac = HashUsingHmac(user, client, x => x.Username),
            UsernameHmac = HashUsingHmac(user, client, x => x.Username)
        };

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

        aes.Key = GetKey(user, client);
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