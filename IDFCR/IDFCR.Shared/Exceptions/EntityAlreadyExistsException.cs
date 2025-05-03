namespace IDFCR.Shared.Exceptions;

public class EntityAlreadyExistsException(string entityType,
    object id, Exception? innerException = null) 
    : InvalidOperationException($"An entity of {entityType} already exists with '{id}'", innerException), IExposableException
{
    public EntityAlreadyExistsException(Type entityType, object id, Exception? innerException = null)
        : this(entityType.Name, id, innerException)
    {

    }

    string IExposableException.Message => $"An entity of {entityType} already exists";
    string? IExposableException.Details => $"Id: '{id}' in '{entityType}'";
}
