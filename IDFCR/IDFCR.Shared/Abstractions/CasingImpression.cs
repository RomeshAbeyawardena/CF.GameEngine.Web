using System.Text;

namespace IDFCR.Shared.Abstractions;

public static class CasingImpression
{
    public static string Calculate(string value)
    {
        List<int> impressions = [];
        
        for(var i=0;i<value.Length;i++)
        {
            char c = value[i];
            if (char.IsLower(c))
            {
                impressions.Add(i);
            }
        }

        return string.Join('-', impressions);
    }

    public static string Restore(string value, string casingImpression)
    {
        if (string.IsNullOrEmpty(casingImpression))
        {
            return value;
        }
        var indices = new HashSet<int>(casingImpression.Split('-').Select(int.Parse));
        var newValue = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
        {
            char c = value[i];
            newValue.Append(indices.Contains(i) ? char.ToLowerInvariant(c) : c);
        }
        
        return newValue.ToString();
    }
}
