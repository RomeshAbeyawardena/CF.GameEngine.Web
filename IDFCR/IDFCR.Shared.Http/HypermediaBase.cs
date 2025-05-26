using IDFCR.Shared.Http.Links;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace IDFCR.Shared.Http;

public abstract record HypermediaBase : IHypermedia
{
    private readonly Dictionary<string, object?> _dictionary = [];
    internal readonly Dictionary<string, ILink?> _links = [];
    internal readonly Dictionary<string, object?> _meta = [];

    protected Dictionary<string, ILink?> LinksDictionary => _links;
    protected Dictionary<string, object?> MetaDictionary => _meta;
    protected Dictionary<string, object?> ObjectDictionary => _dictionary;
    
    object? IReadOnlyDictionary<string, object?>.this[string key] => _dictionary[key];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyDictionary<string, ILink?>? Links => _links.Count > 0 ? _links : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyDictionary<string, object?>? Meta => _meta.Count > 0 ? _meta : null;
    
    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => _dictionary.Keys;
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => _dictionary.Values;

    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count => _dictionary.Count;

    public bool ContainsKey(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        _dictionary["_links"] = LinksDictionary;
        _dictionary["_meta"] = MetaDictionary;

        return _dictionary.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
