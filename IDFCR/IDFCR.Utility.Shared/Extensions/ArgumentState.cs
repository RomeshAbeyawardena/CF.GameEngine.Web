using Microsoft.Extensions.Primitives;

namespace IDFCR.Utility.Shared.Extensions;

public class ArgumentState
{
    public string? CurrentParameter { get; set; }
    public bool IsInParameterValue { get; set; }

    public void Reset()
    {
        CurrentParameter = null;
        IsInParameterValue = false;
    }

    public static void Append(IDictionary<string, StringValues> target, string key, string value)
    {
        var trimmedValue = value.Trim();
        if (!target.TryAdd(key, trimmedValue))
        {
            var collection = target[key].ToList();
            collection.Add(trimmedValue);
            target[key] = collection.ToArray();
        }
    }
}
