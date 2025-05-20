using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public class FindUserQueryHandler(IUserRepository userRepository) : IUnitRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<IUnitResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.FindUserByIdAsync(request.Id, cancellationToken);
        return result.Convert(x => x.Map<UserDto>());
    }
}
