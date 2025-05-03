namespace IDFCR.Shared.Exceptions;

public class EntityHashMismatchException(string entityTypeName, Exception? innerException = null, string? details = null)
    : InvalidOperationException($"The entity type '{entityTypeName}' has a hash mismatch", innerException), IExposableException
{
    public EntityHashMismatchException(Type entityType, Exception? innerException = null, string? details = null)
        : this(entityType.FullName ?? entityType.Name, innerException, details)
    {
        
    }

    string IExposableException.Message => $"The entity type '{entityTypeName}' has a hash mismatch.";
    string? IExposableException.Details => details;
}
