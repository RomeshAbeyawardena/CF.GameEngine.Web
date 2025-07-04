﻿using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Extensions;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

public class UserFilter(CFIdentityDbContext context, IUserPIIProtection userPIIProtection) : InjectableFilterBase<IUserFilter, DbUser>()
{
    private string? UsernameHmac;

    public override async Task<ExpressionStarter<DbUser>> ApplyFilterAsync(ExpressionStarter<DbUser> query, IUserFilter filter, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            var foundClient = await context.Clients.FindAsync([filter.ClientId], cancellationToken) ?? throw new NullReferenceException("Client not found");
            userPIIProtection.Client = foundClient;
            UsernameHmac = userPIIProtection.HashWithHmac(filter.Username);
        }

        return await base.ApplyFilterAsync(query, filter, cancellationToken);
    }

    public override ExpressionStarter<DbUser> ApplyFilter(ExpressionStarter<DbUser> query, IUserFilter filter)
    {
        query = query.And(x => x.ClientId == filter.ClientId);

        if (!string.IsNullOrWhiteSpace(UsernameHmac))
        {
            query = query.And(x => x.EmailAddressHmac == UsernameHmac || x.UsernameHmac == UsernameHmac);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            var nameMatch = PredicateBuilder.New<DbUser>(true);
            nameMatch = nameMatch.Or(x => x.FirstCommonName.Value.Contains(filter.NameContains))
                .Or(x => x.LastCommonName.Value.Contains(filter.NameContains))
                .Or(ExpressionExtensions.OrNullContains<DbUser>(u => u.MiddleCommonName.Value, filter.NameContains));

            query = query.And(nameMatch);
        }

        if (filter.IsSystem.HasValue)
        {
            query = query.And(x => x.IsSystem == filter.IsSystem);
        }

        return query;
    }
}
