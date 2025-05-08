namespace IDFCR.Shared.Exceptions;

public class InvalidEntityStateException(string entityType, string? message, Exception? innerException = null) 
    : EntityExceptionBase(entityType, string.IsNullOrWhiteSpace(message) ? "Invalid state for entity of '{entity-type}'" : message, innerException), IExposableException
{
    public InvalidEntityStateException(Type entityType, string? message, Exception? innerException = null) : this(entityType.Name, message, innerException)
    { 
    }

    string IExposableException.Message => base.Message;
    string? IExposableException.Details => string.Empty;
}
