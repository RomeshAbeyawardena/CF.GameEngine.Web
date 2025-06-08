using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUserByIdQuery(Guid Id, bool Bypass = false) : RoleRequirementBase(() => RoleRegistrar.List<UserRoles>(RoleCategory.Read)), IUnitRequest<UserDto>;
