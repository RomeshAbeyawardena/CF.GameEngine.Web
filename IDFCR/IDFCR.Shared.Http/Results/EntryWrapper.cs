using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Links;
using System.Collections;
using System.Collections.ObjectModel;

namespace IDFCR.Shared.Http.Results;

public record EntryWrapper<T>(T Entry) : IReadOnlyDictionary<string, object?>
    where T : notnull
{
    public void AddLink(string key, ILink link)
    {
        if (!(dictionary.TryGetValue("_links", out var value) && value is Dictionary<string, ILink> links))
        {
            links = [];
            dictionary.Add("_links", links);
        }

        links.Add(key, link);
    }

    public IReadOnlyDictionary<string, ILink>? Links
    {
        get
        {
            if (dictionary.TryGetValue("_links", out var value) && value is IDictionary<string, ILink> links)
            {
                return links.Count > 0 ?  new ReadOnlyDictionary<string, ILink>(links) : null;
            }

            return null;
        }
    }

    private readonly IDictionary<string, object?> dictionary = Entry.AsDictionary();
    object? IReadOnlyDictionary<string, object?>.this[string key] => dictionary[key];

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => dictionary.Keys;
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => dictionary.Values;
    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count => dictionary.Count;

    bool IReadOnlyDictionary<string, object?>.ContainsKey(string key)
    {
        return dictionary.ContainsKey(key);
    }

    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, out object? value)
    {
        return dictionary.TryGetValue(key, out value);
    }
}
