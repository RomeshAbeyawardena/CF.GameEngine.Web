using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record GetUserByIdQuery(Guid Id) : IUnitRequest<UserDto>;

public class FindUserQueryHandler(IUserRepository userRepository) : IUnitRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<IUnitResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.FindUserByIdAsync(request.Id, cancellationToken);
        return result.Convert(x => x.Map<UserDto>());
    }
}

public record FindUsersQuery(Guid ClientId, string? NameContains = null, bool? IsSystem = null, bool NoTracking = true) : IUnitRequestCollection<UserDto>, IUserFilter;

public class FindUsersQueryHandler(IUserRepository userRepository) : IUnitRequestCollectionHandler<FindUsersQuery, UserDto>
{
    public async Task<IUnitResultCollection<UserDto>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
    {
        var results = await userRepository.FindUsersAsync(request, cancellationToken);
        return results.Convert(x => x.Map<UserDto>());
    }
}