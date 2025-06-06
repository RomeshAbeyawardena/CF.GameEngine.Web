using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Http.Links;

public record LinkCollection : ILinkCollection
{
    private readonly IReadOnlyDictionary<string, ILink> _linkDictionary;
    public LinkCollection(IReadOnlyDictionary<string, ILink> linkDictionary)
    {
        _linkDictionary = linkDictionary
            .Where(x => x.Value != Link.Empty)
            .ToDictionary();
    }

    public ILink this[string key] => _linkDictionary[key];

    public IEnumerable<string> Keys => _linkDictionary.Keys;
    public IEnumerable<ILink> Values => _linkDictionary.Values;
    public int Count => _linkDictionary.Count;

    public bool ContainsKey(string key)
    {
        return _linkDictionary.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, ILink>> GetEnumerator()
    {
        return _linkDictionary.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ILink value)
    {
        return _linkDictionary.TryGetValue(key, out value);
    }   

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}