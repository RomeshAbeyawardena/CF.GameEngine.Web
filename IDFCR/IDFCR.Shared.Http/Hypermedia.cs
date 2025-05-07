using IDFCR.Shared.Http.Links;
using System.Collections;
using System.Text.Json.Serialization;

namespace IDFCR.Shared.Http;

public interface IHypermedia<T>
{
    T Data { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, ILink?>? Links { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, object?>? Meta { get; }
}

public interface IHypermediaCollection<T> : IEnumerable<IHypermedia<T>>
{
    IHypermedia<T> Add(T item);
    bool Remove(IHypermedia<T> items);
    IEnumerable<T> AsRawEnumerable();
}

public class HypermediaCollection<T> : IHypermediaCollection<T>
{
    private readonly List<IHypermedia<T>> _items = [];

    public IEnumerable<T> AsRawEnumerable() => _items.Select(s => s.Data);

    public HypermediaCollection(IEnumerable<T> values)
    {
        _items.AddRange(values.Select(v => new Hypermedia<T>(v)));
    }

    public IHypermedia<T> Add(T item)
    {
        var entry = new Hypermedia<T>(item);
        _items.Add(entry);
        return entry;
    }

    public IEnumerator<IHypermedia<T>> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public bool Remove(IHypermedia<T> item)
    {
        return _items.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public record Hypermedia<T>(T Data) : IHypermedia<T>
{
    IReadOnlyDictionary<string, ILink?>? IHypermedia<T>.Links => _links.Count > 0 ? _links : null;
    IReadOnlyDictionary<string, object?>? IHypermedia<T>.Meta => _meta.Count > 0 ? _meta : null;

    private readonly Dictionary<string, ILink?> _links = [];
    private readonly Dictionary<string, object?> _meta = [];

    internal Dictionary<string, ILink?> Links => _links;
    internal Dictionary<string, object?> Meta => _meta;
}

public static class HypermediaExtensions
{
    public static IHypermedia<T> AddLink<T>(this Hypermedia<T> hypermedia, string rel, ILink link)
    {
        hypermedia.Links[rel] = link;
        return hypermedia;
    }

    public static IHypermedia<T> AddMeta<T>(this Hypermedia<T> hypermedia, string key, object? value)
    {
        hypermedia.Meta[key] = value;
        return hypermedia;
    }
}