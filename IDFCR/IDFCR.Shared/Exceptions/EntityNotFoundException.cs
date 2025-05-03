namespace IDFCR.Shared.Exceptions;

public class EntityNotFoundException(string entityType,
    object id, Exception? innerException = null)
    : NullReferenceException($"Unable to find entity of {entityType} with id '{id}'", innerException), IExposableException
{
    public EntityNotFoundException(Type entityType, object id, Exception? innerException = null)
        : this(entityType.Name, id, innerException)
    {
        
    }

    string IExposableException.Message => $"Unable to find entity of {entityType}";
    string? IExposableException.Details => $"Id: '{id}' in '{entityType}'";
}