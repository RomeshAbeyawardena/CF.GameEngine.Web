using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace IDFCR.Utility.Shared.Extensions;

public static class ArgumentParser
{
    private class ArgumentState
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
    public static IDictionary<string, StringValues> GetArguments(IEnumerable<string> arguments)
    {
        var argumentState = new ArgumentState();
        var dictionary = new Dictionary<string, StringValues>();
        foreach (var v in arguments)
        {

            if (!argumentState.IsInParameterValue && v.StartsWith('-'))
            {
                argumentState.IsInParameterValue = true;
                argumentState.CurrentParameter = v.TrimStart('-');
                continue;
            }

            if (argumentState.IsInParameterValue && argumentState.CurrentParameter?.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(v) && !v.StartsWith('-'))
                {
                    ArgumentState.Append(dictionary, argumentState.CurrentParameter, v);
                    argumentState.Reset();
                }
                else
                {
                    ArgumentState.Append(dictionary, argumentState.CurrentParameter, bool.TrueString);
                    argumentState.CurrentParameter = v.TrimStart('-');
                }
            }
        }

        if (argumentState.IsInParameterValue && argumentState.CurrentParameter?.Length > 0)
        {
            ArgumentState.Append(dictionary, argumentState.CurrentParameter, bool.TrueString);
        }

        return dictionary;
    }
}
