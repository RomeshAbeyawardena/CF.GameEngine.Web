using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Clients;

public interface IClient : IEditableClient
{
}

public interface IEditableClient : IClientDetails
{

}

public interface IClientSummary : IMappable<IClient>, IIdentifer
{
    string Reference { get; }
    string Name { get; }
    string? DisplayName { get; } 
    bool IsSystem { get; }
}

public interface IClientDetails : IClientSummary, IValidity
{
    /// <summary>
    /// When returned from the server this will be hashed and meaningless
    /// </summary>
    string? SecretHash { get; }
    DateTimeOffset? SuspendedTimestampUtc { get; }
}