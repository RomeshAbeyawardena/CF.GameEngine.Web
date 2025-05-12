using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUserQuery(Guid Id) : IUnitRequest<UserDto>;
public record FindUsersQuery(Guid? ClientId, string? NameContains) : IUnitRequestCollection<UserDto>, IUserFilter;
