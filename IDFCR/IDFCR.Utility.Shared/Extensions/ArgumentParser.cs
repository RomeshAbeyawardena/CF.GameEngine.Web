using Microsoft.Extensions.Primitives;
using System.Reflection;

namespace IDFCR.Utility.Shared.Extensions;

public static class ArgumentParser
{
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
