using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Repositories;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;

namespace CF.Identity.Infrastructure.SqlServer.Transforms;

public static class UserTransformer
{
    /// <summary>
    /// Resolves or creates a <see cref="DbCommonName"/> for the given lookup value,
    /// assigning either a tracked reference or an existing ID accordingly.
    /// </summary>
    /// <param name="lookupName">The raw name to resolve.</param>
    /// <param name="setEntityRef">Callback to assign the newly created tracked entity.</param>
    /// <param name="setIdRef">Callback to assign the ID of an existing tracked entity.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    private static async Task<(bool, DbCommonName?)> ResolveNameAsync(ICommonNameRepository commonNameRepository, string name, CancellationToken cancellationToken)
    {
        var normalised = name.Trim();
        
        var foundName = (await commonNameRepository.GetByNameRawAsync(normalised, true, cancellationToken)).GetResultOrDefault();

        if (foundName is null)
        {
            var newName = new DbCommonName
            {
                Value = normalised,
            };

            var id = (await commonNameRepository.UpsertAsync(newName, cancellationToken)).GetResultOrDefault();

            return (true, id);
        }

        return (false, foundName.Map<DbCommonName>());
    }

    private static async Task SetCommonNameAsync(ICommonNameRepository commonNameRepository, 
        string? lookupName, Action<Guid> setIdRef, Action<DbCommonName> setCommonName, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(lookupName))
        {
            var (isNew, name) = await ResolveNameAsync(commonNameRepository, lookupName, cancellationToken);
            if (name is null)
                throw new EntityNotFoundException(typeof(DbCommonName), lookupName);

            if (isNew)
            {
                setCommonName(name);
            }
            else
            {
                setIdRef(name.Id);
            }
        }
    }


    public static async Task<DbUser> Transform(IUser user, ICommonNameRepository  commonNameRepository,
        CancellationToken cancellationToken, DbUser? dbUser = null)
    {
        bool isDbUserProvided = dbUser is not null;
        dbUser ??= user.Map<DbUser>();

        await SetCommonNameAsync(commonNameRepository, user.Firstname, id => dbUser.FirstCommonNameId = id, model => dbUser.FirstCommonName = model, cancellationToken);
        await SetCommonNameAsync(commonNameRepository, user.Middlename, id => dbUser.MiddleCommonNameId = id, model => dbUser.MiddleCommonName = model, cancellationToken);
        await SetCommonNameAsync(commonNameRepository, user.Lastname, id => dbUser.LastCommonNameId = id, model => dbUser.LastCommonName = model, cancellationToken);

        if(isDbUserProvided)
        {
            dbUser.Map(user);
            dbUser.RowVersion = user.RowVersion;
        }

        return dbUser;
    }
}
