using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Exceptions;
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
    private static async Task<(bool, DbCommonName?)> ResolveNameAsync(CFIdentityDbContext context, string name, CancellationToken cancellationToken)
    {
        var normalised = name.Trim();
        var upper = normalised.ToUpperInvariant();

        var foundName = await context.CommonNames
            .FirstOrDefaultAsync(x => x.Value == upper, cancellationToken);

        if (foundName is null)
        {
            var newName = new DbCommonName
            {
                Value = upper,
                ValueNormalised = normalised
            };

            context.CommonNames.Add(newName);

            return (true, newName);
        }

        return (false, foundName);
    }

    private static async Task SetCommonNameAsync(CFIdentityDbContext context, string? lookupName, 
        Action<DbCommonName> setEntityRef, Action<Guid> setIdRef, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(lookupName))
        {
            var (isNew, name) = await ResolveNameAsync(context, lookupName, cancellationToken);
            if (name is null)
                throw new EntityNotFoundException(typeof(DbCommonName), lookupName);

            if (isNew)
                setEntityRef(name);
            else
                setIdRef(name.Id);
        }
    }


    public static async Task<DbUser> Transform(IUser user, CFIdentityDbContext context, IProtectionInfo protectionInfo,
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

        await SetCommonNameAsync(context, user.Firstname, name => dbUser.FirstCommonName = name, id => dbUser.FirstCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(context, user.Middlename, name => dbUser.MiddleCommonName = name, id => dbUser.MiddleCommonNameId = id, cancellationToken);
        await SetCommonNameAsync(context, user.Lastname, name => dbUser.LastCommonName = name, id => dbUser.LastCommonNameId = id, cancellationToken);

        if(isDbUserProvided)
        {
            dbUser.Map(user);
            dbUser.RowVersion = user.RowVersion;
        }

        return dbUser;
    }
}
