using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Repositories;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
    private static async Task<(bool, Guid)> ResolveNameAsync(ICommonNameRepository commonNameRepository, string name, CancellationToken cancellationToken)
    {
        var normalised = name.Trim();
        var upper = normalised.ToUpperInvariant();

        var foundName = (await commonNameRepository.GetByNameAsync(upper, cancellationToken)).GetResultOrDefault();

        if (foundName is null)
        {
            var newName = new CommonNameDto
            {
                Value = upper,
                ValueNormalised = normalised
            };

            var id = (await commonNameRepository.UpsertAsync(newName, cancellationToken)).GetResultOrDefault();

            return (true, id);
        }

        return (false, foundName.Id);
    }

    private static async Task SetCommonNameAsync(ICommonNameRepository commonNameRepository, 
        string? lookupName, Action<Guid> setIdRef, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(lookupName))
        {
            var (isNew, name) = await ResolveNameAsync(commonNameRepository, lookupName, cancellationToken);
            if (name is null)
                throw new EntityNotFoundException(typeof(DbCommonName), lookupName);

            setIdRef(name.Id);
        }
    }


    public static async Task<DbUser> Transform(IUser user, ICommonNameRepository  commonNameRepository, IProtectionInfo protectionInfo,
        CancellationToken cancellationToken, DbUser? dbUser = null)
    {
        bool isDbUserProvided = dbUser is not null;
        dbUser ??= user.Map<DbUser>();

        var userHmac = protectionInfo.UserHmac;
        var userCasingImpressions = protectionInfo.CasingImpressions;

        dbUser.EmailAddressHmac = userHmac.EmailAddressHmac;
        dbUser.EmailAddressCI = userCasingImpressions.EmailAddressCI;
        dbUser.UsernameHmac = userHmac.UsernameHmac;
        dbUser.UsernameCI = userCasingImpressions.UsernameCI;
        dbUser.PreferredUsernameHmac = userHmac.PreferredUsernameHmac;
        dbUser.PreferredUsernameCI = userCasingImpressions.PreferredUsernameCI;
        dbUser.PrimaryTelephoneNumberHmac = userHmac.PrimaryTelephoneNumberHmac;

        await SetCommonNameAsync(commonNameRepository, user.Firstname, name => dbUser.FirstCommonName = name.ValueNormalised, id => dbUser.FirstCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(commonNameRepository, user.Middlename, name => dbUser.MiddleCommonName = name.ValueNormalised, id => dbUser.MiddleCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(commonNameRepository, user.Lastname, name => dbUser.LastCommonName = name.ValueNormalised, id => dbUser.LastCommonNameId = id, cancellationToken);

        if(isDbUserProvided)
        {
            dbUser.Map(user);
            dbUser.RowVersion = user.RowVersion;
        }

        return dbUser;
    }
}
