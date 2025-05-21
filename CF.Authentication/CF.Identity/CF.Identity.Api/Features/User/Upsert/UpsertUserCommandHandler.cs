using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Upsert;

public class UpsertUserCommandHandler(IUserRepository userRepository) : IUnitRequestHandler<UpsertUserCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
    { 
        return await userRepository.UpsertAsync(request.User.Map<Infrastructure.Features.Users.UserDto>(), cancellationToken);
    }
}
