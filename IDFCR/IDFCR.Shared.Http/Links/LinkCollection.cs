using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Http.Links;

public record LinkCollection(IReadOnlyDictionary<string, ILink> linkDictionary) : ILinkCollection
{
    public ILink this[string key] => linkDictionary[key];

    public IEnumerable<string> Keys => linkDictionary.Keys;
    public IEnumerable<ILink> Values => linkDictionary.Values;
    public int Count => linkDictionary.Count;

    public bool ContainsKey(string key)
    {
        return linkDictionary.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, ILink>> GetEnumerator()
    {
        return linkDictionary.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ILink value)
    {
        return linkDictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}