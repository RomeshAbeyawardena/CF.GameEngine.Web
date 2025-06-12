using Microsoft.Extensions.Primitives;

namespace CF.Identity.Integration;

public class Section
{
    public string? Name { get; set; }
    public Dictionary<string, StringValues> Content { get; } = [];
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
