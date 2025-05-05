namespace IDFCR.Shared.Exceptions;

public class EntityNotFoundException(string entityType,
    object id, Exception? innerException = null)
    : EntityExceptionBase(entityType, $"Unable to find entity of {{entity-type}} with id '{id}'", innerException), IExposableException
{
    public EntityNotFoundException(Type entityType, object id, Exception? innerException = null)
        : this(entityType.Name, id, innerException)
    {
        
    }

    string IExposableException.Message => FormatMessage("Unable to find entity of {entity-type}");
    string? IExposableException.Details => FormatMessage("Id: '{id}' in '{entity-type}'", b => b.AddOrUpdate("id", id.ToString() ?? string.Empty));
}