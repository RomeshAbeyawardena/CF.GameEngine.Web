using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace IDFCR.Shared.Abstractions;

public interface IStateBagItem
{
    Type Type { get; }
    string Key { get; }
    object? Value { get; }
}

public interface IStateBag
{
    T? Get<T>(string key);
    object? Get(string key);
    void Set(string key, object? value);
}

public record StateBagItem(Type Type, string Key, object? Value) : IStateBagItem;

public class StateBag : IStateBag
{
    private ConcurrentDictionary<string, IStateBagItem> _state = [];
    public T? Get<T>(string key)
    {
        var value = Get(key);
        if(value is not null && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    public object? Get(string key)
    {
        if(_state.TryGetValue(key, out var value))
        {
            return value.Value;
        }

        return null;
    }

    public void Set(string key, object? value)
    {
        if (value is null)
        {
            throw new NullReferenceException();
        }

        var val = new StateBagItem(value.GetType(), key, value);
        
        _state.AddOrUpdate(key, val, (k, v) => val);
    }
}