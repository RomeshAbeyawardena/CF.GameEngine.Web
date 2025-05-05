namespace IDFCR.Shared.Exceptions;

public class EntityHashMismatchException(string entityType, Exception? innerException = null, string? details = null)
    : EntityExceptionBase(entityType, "The entity type '{entity-type}' has a hash mismatch", innerException), IExposableException
{
    public EntityHashMismatchException(Type entityType, Exception? innerException = null, string? details = null)
        : this(entityType.FullName ?? entityType.Name, innerException, details)
    {
        
    }

    string IExposableException.Message => FormatMessage("The entity type '{entity-type}' has a hash mismatch.");
    string? IExposableException.Details => FormatMessage(details);
}
