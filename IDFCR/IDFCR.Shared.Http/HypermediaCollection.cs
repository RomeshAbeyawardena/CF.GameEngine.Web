using System.Collections;

namespace IDFCR.Shared.Http;

public class HypermediaCollection<T> : IHypermediaCollection<T>
{
    private readonly List<IHypermedia<T>> _items = [];

    public IEnumerable<T>? AsRawEnumerable() => _items.Where(s => s is not null).Select(s => s.Value!);

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
