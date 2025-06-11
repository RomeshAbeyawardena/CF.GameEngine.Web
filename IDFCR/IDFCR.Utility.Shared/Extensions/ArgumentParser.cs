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
    }
    public static IDictionary<string, string> GetArguments(IEnumerable<string> arguments)
    {
        var hash = new HashSet<string>(arguments);
        var argumentState = new ArgumentState();
        var dictionary = new Dictionary<string, string>();
        foreach (var v in hash)
        {
            Console.Write(v);
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
                    dictionary.Add(argumentState.CurrentParameter, v.Trim());
                    argumentState.Reset();
                }
                else
                {
                    dictionary.Add(argumentState.CurrentParameter, bool.TrueString);
                    argumentState.CurrentParameter = v.TrimStart('-');
                }
            }
        }

        if (argumentState.IsInParameterValue && argumentState.CurrentParameter?.Length > 0)
        {
            dictionary.Add(argumentState.CurrentParameter, bool.TrueString);
        }

        return dictionary;
    }
}
