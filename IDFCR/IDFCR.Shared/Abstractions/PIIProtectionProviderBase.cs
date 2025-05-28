using System.ComponentModel;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions;

public interface IPIIProtection<T>
{
    string Hash(HashAlgorithmName algorithmName, string secret, string salt, int length);
    IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry);
    void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo> protectionData);
    string HashWithHMAC(string key, string data);
}


public interface IProtectionInfo
{
    string Hmac { get; }
    string CasingImpressions { get; }
}

public record DefaultProtectionInfo(string Hmac, string CasingImpressions) : IProtectionInfo;

public class PIIProtectionFactory<T>(Expression<Func<T, string>> member, Func<PIIProtectionProviderBase, string, IProtectionInfo> protect,
    Action<PIIProtectionProviderBase, string, IProtectionInfo> unprotect)
{
    public IProtectionInfo Protect(PIIProtectionProviderBase provider, T instance)
    {
        var value = member.Compile();
        return protect(provider, value(instance));
    }

    public void Unprotect(PIIProtectionProviderBase provider, T instance, IProtectionInfo protectionInfo)
    {
        var value = member.Compile();
        unprotect(provider, value(instance), protectionInfo);
    }
}

public abstract class PIIProtectionBase<T>(Encoding encoding) : PIIProtectionProviderBase, IPIIProtection<T>
{
    private readonly Dictionary<string, PIIProtectionFactory<T>> protectionFactories = [];
    private readonly Dictionary<string, IProtectionInfo> protectionData = [];

    protected PIIProtectionBase<T> For(Expression<Func<T, string>> member, Func<PIIProtectionProviderBase, string, IProtectionInfo> protect,
    Action<PIIProtectionProviderBase, string, IProtectionInfo> unprotect)
    {
        var vis = new LinkExpressionVisitor();
        vis.Visit(member);

        protectionFactories.Add(vis.MemberName!,
            new PIIProtectionFactory<T>(member, protect, unprotect));

        return this;
    }

    public string Hash(HashAlgorithmName algorithmName, string secret, string salt, int length)
    {
        var derived = new Rfc2898DeriveBytes(encoding.GetBytes(secret), 
            encoding.GetBytes(salt), 100_000, algorithmName);
        return Convert.ToBase64String(derived.GetBytes(length));
    }

    public string HashWithHMAC(string key, string data)
    {
        return Convert.ToBase64String(
            HMACSHA512.HashData(encoding.GetBytes(key), encoding.GetBytes(data)));
    }

    public IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry)
    {
        protectionData.Clear();
        foreach (var (key, value) in protectionFactories)
        {
            protectionData.Add(key, value.Protect(this, entry));
        }

        return protectionData;
    }

    public void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo> protectionData)
    {
        foreach(var (key, value) in protectionFactories)
        {
            // Try protection data from caller first; fallback to internal copy.
            // This guards against DI scope misalignment (e.g., transient reuse).
            if (!protectionData.TryGetValue(key, out var protectionInfo) 
                && !this.protectionData.TryGetValue(key, out protectionInfo))
            {
                throw new KeyNotFoundException($"Protection data for key '{key}' not found.");
            }

            value.Unprotect(this, entry, protectionInfo);
        }
    }
}


public abstract class PIIProtectionProviderBase
{
    private static string[] GetSpacerParts(string parts, char separator)
    {
        return parts.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }

    protected internal static (string?, byte[]) GenerateKey(int length, char separator, Encoding encoding, string? spacers, params string[] values)
    {
        if (length < values.Length)
        {
            throw new ArgumentException("Length must be greater than or equal to the sum of the lengths of the values.");
        }
        List<string> spacerParts = [];
        if (!string.IsNullOrWhiteSpace(spacers))
        {
            spacerParts.AddRange(GetSpacerParts(spacers, separator));
        }

        var totalCharactersPerValue = length / values.Length;

        var builder = new StringBuilder();

        var index = 0;
        bool isAdditionalSpacer = false;
        foreach (var value in values)
        {
            builder.Append(separator);
            if (value.Length > totalCharactersPerValue - 1)
            {
                builder.Append(value.AsSpan(0, totalCharactersPerValue - 1));
            }
            else if (value.Length < totalCharactersPerValue - 1)
            {
                var additionalLengthNeeded = totalCharactersPerValue - value.Length - 1;

                var spacer = spacerParts.ElementAtOrDefault(index++);

                if (string.IsNullOrEmpty(spacer))
                {
                    var id = Guid.NewGuid().ToString();
                    spacer = id.AsSpan(0, additionalLengthNeeded).ToString();
                    isAdditionalSpacer = true;
                }

                if (spacer.Length < additionalLengthNeeded)
                {
                    throw new FormatException("Spacer is not long enough to fill the gap.");
                }

                if (isAdditionalSpacer)
                {
                    spacerParts.Add(spacer);
                }

                builder.Append($"{value}{spacer}");
            }
            else
            {
                builder.Append(value);
            }
        }

        return (string.Join(separator, spacerParts), encoding.GetBytes(builder.ToString()));
    }

    protected static string? Encrypt(string? value, SymmetricAlgorithm symmetricAlgorithm)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            using var encryptor = symmetricAlgorithm.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }

        return value;
    }

    protected static string? Decrypt(string? value, SymmetricAlgorithm symmetricAlgorithm)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            using var decryptor = symmetricAlgorithm.CreateDecryptor();
            var encryptedBytes = Convert.FromBase64String(value);
            var plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
        return value;
    }
}