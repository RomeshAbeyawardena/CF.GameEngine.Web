using System.Text;

namespace IDFCR.Shared.Abstractions;

public static class CasingImpression
{
    private const int MaxCasedLength = 256;
    private const string EncodingScheme = "Base64ByteIndex";

    public static string Calculate(string value)
    {
        if (value.Length > 256)
        {
            throw new NotSupportedException($"Casing impression only supports strings up to {MaxCasedLength} characters using {EncodingScheme} encoding.");
        }

        List<byte> impressions = [];
        
        for(var i=0;i<value.Length;i++)
        {
            char c = value[i];
            if (char.IsLower(c))
            {
                impressions.Add(Convert.ToByte(i));
            }
        }

        return Convert.ToBase64String(impressions.ToArray());
    }

    public static string Restore(string value, string casingImpression)
    {
        if (string.IsNullOrEmpty(casingImpression))
        {
            return value;
        }

        var indices = new HashSet<byte>(Convert.FromBase64String(casingImpression));

        var newValue = new StringBuilder();

        for (byte i = 0; i < value.Length; i++)
        {
            char c = value[i];
            newValue.Append(indices.Contains(i) ? char.ToLowerInvariant(c) : c);
        }
        
        return newValue.ToString();
    }
}
