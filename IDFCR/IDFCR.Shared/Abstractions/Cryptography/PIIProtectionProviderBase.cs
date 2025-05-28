using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions.Cryptography;

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