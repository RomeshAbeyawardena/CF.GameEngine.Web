using System.Collections;

namespace IDFCR.Shared.Abstractions;

public class LinkList<T> : ILinkList<T>
    where T : IIdentifer, INamed
{
    private readonly List<ILink<T>> links = [];

    public LinkList(IEnumerable<T> items, Func<T, string> getDisplayName)
    {
        foreach (var item in items) TryAdd(item, getDisplayName);
    }

    public ILink<T>? this[string key] => Find(key);

    ILink<T> IList<ILink<T>>.this[int index] { get => links[index]; set => links[index] = value; }

    public int Count { get; }
    public bool IsReadOnly { get; }

    public bool Contains(string key)
    {
        return Find(key) is not null;
    }

    public ILink<T>? Find(string key)
    {
        return links.Find(x => x.Model != null && x.Model.Key.Contains(key));
    }

    public IEnumerator<ILink<T>> GetEnumerator()
    {
        return links.GetEnumerator();
    }

    public int IndexOf(string key)
    {
        var foundItem = Find(key);

        if (foundItem is null)
        {
            return -1;
        }

        return links.IndexOf(foundItem);
    }

    public bool TryAdd(T item, Func<T, string> getDisplayName)
    {
        var foundItem = Find(item.Key);

        if (foundItem is not null)
        {
            return false;
        }

        var displayName = getDisplayName(item);

        links.Add(new Link<T>(item.Id, displayName, item));

        return true;
    }

    public bool TryRemove(string key)
    {
        var foundItem = Find(key);

        if (foundItem is null)
        {
            return false;
        }

        return links.Remove(foundItem);
    }

    void ICollection<ILink<T>>.Add(ILink<T> item)
    {
        links.Add(item);
    }

    void ICollection<ILink<T>>.Clear()
    {
        links.Clear();
    }

    bool ICollection<ILink<T>>.Contains(ILink<T> item)
    {
        return links.Contains(item);
    }

    void ICollection<ILink<T>>.CopyTo(ILink<T>[] array, int arrayIndex)
    {
        links.CopyTo(array, arrayIndex);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    int IList<ILink<T>>.IndexOf(ILink<T> item)
    {
        return links.IndexOf(item);
    }

    void IList<ILink<T>>.Insert(int index, ILink<T> item)
    {
        links.Insert(index, item);
    }

    bool ICollection<ILink<T>>.Remove(ILink<T> item)
    {
        return links.Remove(item);
    }

    void IList<ILink<T>>.RemoveAt(int index)
    {
        links.RemoveAt(index);
    }
}