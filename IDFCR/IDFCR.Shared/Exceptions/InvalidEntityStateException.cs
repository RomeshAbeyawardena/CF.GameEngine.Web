namespace IDFCR.Shared.Exceptions;

public class InvalidEntityStateException(string entityType, string? message, Exception? innerException = null) 
    : Exception(string.IsNullOrWhiteSpace(message) ? $"Invalid state for entity of '{entityType}'" : message, innerException), IExposableException
{
    string IExposableException.Message => base.Message;
    string? IExposableException.Details { get; }
}
