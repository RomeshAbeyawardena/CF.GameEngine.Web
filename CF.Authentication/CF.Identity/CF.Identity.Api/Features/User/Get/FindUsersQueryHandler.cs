using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public class FindUsersQueryHandler(IUserRepository userRepository) : IUnitRequestCollectionHandler<FindUsersQuery, UserDto>
{
    public async Task<IUnitResultCollection<UserDto>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
    {
        var results = await userRepository.FindUsersAsync(request, cancellationToken);
        return results.Convert(x => x.Map<UserDto>());
    }
}