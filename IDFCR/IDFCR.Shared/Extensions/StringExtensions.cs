using System.Security.Authentication;
using System.Security.Cryptography;

namespace IDFCR.Shared.Extensions;

public static class StringExtensions
{
    public static string FixedLength(this string value, int length)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new string(' ', length);
        }

        if (value.Length > length && length - 3 > 0)
        {
            return string.Concat(value.AsSpan(0, length - 3).Trim(), "...");
        }

        if (value.Length < length)
        {
            var spacesToAdd = length - value.Length;
            return string.Concat(value, new string(' ', spacesToAdd));
        }

        return value;
    }

    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var result = new List<char>();

        for (int i = 0; i < value.Length; i++)
        {
            char current = value[i];
            char? prev = i > 0 ? value[i - 1] : (char?)null;
            char? next = i < value.Length - 1 ? value[i + 1] : (char?)null;

            if (current == '_' || current == ' ')
            {
                if (result.Count > 0 && result[^1] != '-') result.Add('-');
                continue;
            }

            if (char.IsUpper(current))
            {
                bool isAcronymEnd = next.HasValue && char.IsLower(next.Value);
                bool isWordBreak = prev.HasValue && !char.IsUpper(prev.Value);

                if ((isAcronymEnd || isWordBreak) && result.Count > 0 && result[^1] != '-')
                    result.Add('-');

                result.Add(char.ToLower(current));
            }
            else
            {
                // Insert hyphen between digit & letter or vice versa
                if (prev.HasValue && (
                    (char.IsDigit(prev.Value) && char.IsLetter(current)) ||
                    (char.IsLetter(prev.Value) && char.IsDigit(current))))
                {
                    result.Add('-');
                }

                result.Add(char.ToLower(current));
            }
        }

        return new string([.. result]).Trim('-');
    }

    public static string ReplaceAll(this string value, string newValue, params string[] values)
    {
        return ReplaceAll(value, newValue, default, values);
    }

    public static string ReplaceAll(this string value, string newValue, StringComparison stringComparison, params string[] values)
    {
        if (values == null || values.Length == 0)
            return value;
        foreach (var item in values)
        {
            value = value.Replace(item, newValue, stringComparison);
        }
        return value;
    }

    public static string PrependUrl(this string value, string source)
    {
        return Prepend(value, source, '/');
    }
    public static string Prepend(this string value, string source, char? separator)
    {
        return $"{source}{separator}{value}";
    }
    public static string Hash(this string value, HashAlgorithmType hashAlgorithmType, 
        string? format = null, params object[] args)
    {
        using HashAlgorithm hashAlgorithm = hashAlgorithmType switch
        {
            HashAlgorithmType.None => throw new NotSupportedException("No hashing algorithm is used."),
            HashAlgorithmType.Md5 => MD5.Create(),
            HashAlgorithmType.Sha1 => SHA1.Create(),
            HashAlgorithmType.Sha256 => SHA256.Create(),
            HashAlgorithmType.Sha384 => SHA384.Create(),
            HashAlgorithmType.Sha512 => SHA512.Create(),
            _ => throw new NotSupportedException($"Hash algorithm {hashAlgorithmType} is not supported.")
        };

        var formattedValue = string.IsNullOrWhiteSpace(format) 
            ? value
            : string.Format(format, args.Prepend(value));

        return Convert
            .ToBase64String(hashAlgorithm
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(formattedValue)));
    }
}
