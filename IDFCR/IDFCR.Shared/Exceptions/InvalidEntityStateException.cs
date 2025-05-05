namespace IDFCR.Shared.Exceptions;

public class InvalidEntityStateException(string entityType, string? message, Exception? innerException = null) 
    : EntityExceptionBase(entityType, string.IsNullOrWhiteSpace(message) ? "Invalid state for entity of '{entity-type}'" : message, innerException), IExposableException
{
    string IExposableException.Message => base.Message;
    string? IExposableException.Details => string.Empty;
}
