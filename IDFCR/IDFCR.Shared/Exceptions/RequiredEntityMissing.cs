namespace IDFCR.Shared.Exceptions;

public class RequiredEntityMissingException(string entityType, Exception? innerException = null, 
    string? details = null) 
        : EntityExceptionBase(entityType, "The required data for the entity of {entity-type} was not supplied", innerException) ,IExposableException
{
    public RequiredEntityMissingException(Type entityType, Exception? innerException = null, string? details = null)
        : this(entityType.Name, innerException, details)
    {
    }

    string IExposableException.Message => FormatMessage(Message);
    string? IExposableException.Details => FormatMessage(details);
}
