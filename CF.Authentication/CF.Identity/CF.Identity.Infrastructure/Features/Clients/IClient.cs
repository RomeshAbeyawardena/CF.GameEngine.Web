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
    
}

public interface IClientDetails : IClientSummary, IValidity
{
    DateTimeOffset? SuspendedTimestampUtc { get; }
}