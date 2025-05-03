namespace IDFCR.Shared.Exceptions;

/// <summary>
/// Interface for exceptions that can be exposed to the client.
/// </summary>
public interface IExposableException
{
    string Message { get; }
    string? Details { get; }
}
