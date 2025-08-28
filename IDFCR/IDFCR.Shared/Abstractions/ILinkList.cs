namespace IDFCR.Shared.Abstractions;

public interface ILinkList<T> : IList<ILink<T>>
    where T : IIdentifer, INamed
{
    ILink<T>? this[string key] { get; }
    bool TryAdd(T item, Func<T, string> getDisplayName);
    bool TryRemove(string key);
    bool Contains(string key);
    int IndexOf(string key);
    ILink<T>? Find(string key);
}
