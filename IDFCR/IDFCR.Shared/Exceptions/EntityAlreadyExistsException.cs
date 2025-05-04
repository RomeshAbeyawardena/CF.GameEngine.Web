namespace IDFCR.Shared.Exceptions;

public class EntityAlreadyExistsException(string entityType,
    object id, Exception? innerException = null) 
    : EntityExceptionBase(entityType, "An entity of {entity-type} already exists with '{id}'", innerException), IExposableException
{
    public EntityAlreadyExistsException(Type entityType, object id, Exception? innerException = null)
        : this(entityType.Name, id, innerException)
    {

    }

    string IExposableException.Message => FormatMessage("An entity of {entity-type} already exists");
    string? IExposableException.Details => $"Id: '{id}' in '{EntityType}'";
}
