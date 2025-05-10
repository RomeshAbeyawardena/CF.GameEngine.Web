using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Models;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

internal class AccessTokenRepository(TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<IAccessToken, DbAccessToken, AccessTokenDto>(timeProvider, context), IAccessTokenRepository
{
}
