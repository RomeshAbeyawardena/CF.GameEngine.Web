using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Http.Results;

public class CaseInsensitiveComparer<TKey> : IEqualityComparer<TKey>
    where TKey : notnull
{
    public bool Equals(TKey? x, TKey? y)
    {
        return string.Equals(x?.ToString(), y?.ToString(), StringComparison.OrdinalIgnoreCase);
    }
    public int GetHashCode([DisallowNull] TKey obj)
    {
        return obj.ToString()?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
    }
}
