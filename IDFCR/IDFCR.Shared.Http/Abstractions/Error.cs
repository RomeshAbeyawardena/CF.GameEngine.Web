using IDFCR.Shared.Exceptions;

namespace IDFCR.Shared.Http.Abstractions;

public record Error(string Message, string? Details) : IError
{
    public Error(IExposableException exception)
        : this(exception.Message, exception.Details)
    {
    }
}
