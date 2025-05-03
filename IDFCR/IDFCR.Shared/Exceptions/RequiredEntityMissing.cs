namespace IDFCR.Shared.Exceptions;

public class RequiredEntityMissingException(string entityType, Exception? innerException = null, 
    string? details = null) 
        : NullReferenceException(
             $"The required data for the entity of {entityType} was not supplied", innerException)
            ,IExposableException
{
    public RequiredEntityMissingException(Type entityType, Exception? innerException = null, string? details = null)
        : this(entityType.Name, innerException, details)
    {
    }

    string IExposableException.Message => Message;
    string? IExposableException.Details => details;
}
