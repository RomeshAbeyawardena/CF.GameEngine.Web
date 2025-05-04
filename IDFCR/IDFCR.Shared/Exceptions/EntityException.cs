using IDFCR.Shared.Builders;
using IDFCR.Shared.Extensions;

namespace IDFCR.Shared.Exceptions;

public abstract class EntityExceptionBase(string entityType, string message, Exception? innerException) 
    : Exception(string.Empty, innerException)
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
            message = message.Replace(k, value);
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

    protected string FormatMessage(string sourceMessage, IReadOnlyDictionary<string, string>? values = null)
    {
        return FormatMessage(sourceMessage, EntityType, values);
    }

    protected EntityExceptionBase(Type entityType, string message, Exception innerException)
        : this(entityType.Name, message, innerException)
    {
    }

    private readonly string _message = FormatMessage(message, entityType);
    public string EntityType { get; } = entityType.Replace("dto", string.Empty, StringComparison.InvariantCultureIgnoreCase);

    public override string Message => _message;
}
