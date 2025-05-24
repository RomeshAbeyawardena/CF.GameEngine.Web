using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Roles;

public interface IRole : IIdentifer
{
    //Non-optional, these will belong to a given a client and no-one else
    Guid ClientId { get; }
    string Key { get; }
    string? DisplayName { get; }
}