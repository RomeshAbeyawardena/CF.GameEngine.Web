using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.AccessRoles;

public interface IAccessRole : IAccessRoleDetail
{   
}

public interface IAccessRoleSummary : IMappable<IAccessRole>, IIdentifer
{
    Guid ClientId { get; }
    string Key { get; }
    string? DisplayName { get; }
}

public interface IAccessRoleDetail : IAccessRoleSummary
{
    string? Description { get; }
}