using IDFCR.Shared.Builders;
using IDFCR.Shared.Extensions;

namespace IDFCR.Shared.Exceptions;

public abstract class EntityExceptionBase : Exception
{
    public static IReadOnlyDictionary<string, string> ConfigureKeyValues(Action<IDictionaryBuilder<string, string>> action)
    {
        var builder = new DictionaryBuilder<string, string>();
        action(builder);
        return builder.Build();
    }

    private static string Format(string sourceMessage, IReadOnlyDictionary<string, string> values)
    {
        var message = sourceMessage;
        foreach(var (key, value) in values)
        {
            var k = key.ToKebabCase();
            message = message.Replace($"{{{k}}}", value);
        }
        return message;
    }

    protected static string FormatMessage(string sourceMessage, string entityType, IReadOnlyDictionary<string, string>? values = null)
    {
        var vals = values == null 
            ? []
            : new Dictionary<string, string>(values);

        vals.TryAdd("entity-type", entityType);

        return Format(sourceMessage, vals);
    }


    private readonly string _message;
    protected EntityExceptionBase(string entityType, string message, Exception? innerException, params string[] affixesToRemove) : base(null, innerException)
    {
        EntityType = entityType.ReplaceAll(string.Empty, StringComparison.InvariantCultureIgnoreCase, [.. affixesToRemove.Prepend("dto")]);
        _message = FormatMessage(message, EntityType);
    }

    protected string FormatMessage(string sourceMessage, IReadOnlyDictionary<string, string>? values = null)
    {
        return FormatMessage(sourceMessage, EntityType, values);
    }

    protected string FormatMessage(string sourceMessage, Action<IDictionaryBuilder<string, string>> action)
    {
        return FormatMessage(sourceMessage, EntityType, ConfigureKeyValues(action));
    }

    protected EntityExceptionBase(Type entityType, string message, Exception innerException, params string[] affixesToRemove)
        : this(entityType.Name, message, innerException, affixesToRemove)
    {
    }

    public string EntityType { get; }

    public override string Message => _message;
}
