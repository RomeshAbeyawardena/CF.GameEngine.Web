using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Http.Links;

internal record LinkPlaceholderBag : ILinkPlaceholderBag
{
    private readonly Dictionary<string, string> _placeholderBag = [];

    protected IDictionary<string, string> PlaceholderBag => _placeholderBag;

    public string this[string key] => _placeholderBag[key];

    public IEnumerable<string> Keys => _placeholderBag.Keys;
    public IEnumerable<string> Values => _placeholderBag.Values;
    public int Count => _placeholderBag.Count;

    public bool ContainsKey(string key)
    {
        return _placeholderBag.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _placeholderBag.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        return _placeholderBag.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
