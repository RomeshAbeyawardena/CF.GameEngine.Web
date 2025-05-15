using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions;

public interface IPIIRowVersion
{
    public string RowVersion { get; set; }
}

public abstract class PIIProtectionProviderBase
{
    protected internal static byte[] KeyBuilder(int length, char separator, Encoding encoding, params string[] values)
    {
        if(length < values.Length)
        {
            throw new ArgumentException("Length must be greater than or equal to the sum of the lengths of the values.");
        }

        var totalCharactersPerValue = length / values.Length;

        var builder = new StringBuilder();
        
        foreach (var value in values)
        {
            builder.Append(separator);
            if(value.Length > totalCharactersPerValue - 1)
            {
               builder.Append(value.AsSpan(0, totalCharactersPerValue - 1));
            }
            else if (value.Length < totalCharactersPerValue - 1)
            {
                var additionalLengthNeeded = totalCharactersPerValue - value.Length - 1;
                var id = Guid.NewGuid().ToString();
                builder.Append($"{value}{id.AsSpan(0, additionalLengthNeeded)}");
            }
            else {
                builder.Append(value);
            }
        }

        return encoding.GetBytes(builder.ToString());
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