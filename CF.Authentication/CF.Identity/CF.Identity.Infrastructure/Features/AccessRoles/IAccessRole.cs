using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public interface IAccessRole : IMappable<IAccessRole>, IIdentifer
{
    //Non-optional, these will belong to a given a client and no-one else
    Guid ClientId { get; }
    string Key { get; }
    string? DisplayName { get; }
    string? Description { get; }
}
