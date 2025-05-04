namespace IDFCR.Shared.Exceptions;

public abstract class EntityExceptionBase(string entityType, string message, Exception? innerException) 
    : Exception(string.Empty, innerException)
{
    private static string FormatMessage(string sourceMessage, string entityType)
    {
        return sourceMessage.Replace("{entity-type}", entityType.Replace("dto", string.Empty, StringComparison.InvariantCultureIgnoreCase));
    }

    protected string FormatMessage(string sourceMessage)
    {
        return FormatMessage(sourceMessage, EntityType);
    }

    protected EntityExceptionBase(Type entityType, string message, Exception innerException)
        : this(entityType.Name, message, innerException)
    {
    }

    private readonly string _message = FormatMessage(message, entityType);
    public string EntityType { get; } = entityType;

    public override string Message => _message;
}
