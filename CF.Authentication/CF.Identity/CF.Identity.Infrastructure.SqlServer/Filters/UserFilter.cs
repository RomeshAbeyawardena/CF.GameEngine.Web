using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Extensions;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

public class UserFilter(CFIdentityDbContext context, IUserCredentialProtectionProvider userCredentialProtectionProvider) : InjectableFilterBase<IUserFilter, DbUser>(), IUserFilter
{
    protected override IUserFilter Source => this;
    public Guid ClientId { get; set; }
    public string? Username { get; set; }
    public string? NameContains { get; set; }
    public bool? IsSystem { get; set; }

    private string? UsernameHmac;

    public override async Task<ExpressionStarter<DbUser>> ApplyFilterAsync(ExpressionStarter<DbUser> query, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(Username))
        {
            var foundClient = await context.Clients.FindAsync([ClientId], cancellationToken) ?? throw new NullReferenceException("Client not found");
            UsernameHmac = userCredentialProtectionProvider.HashUsingHmac(foundClient, Username);
        }

        return await base.ApplyFilterAsync(query, cancellationToken);
    }

    public override ExpressionStarter<DbUser> ApplyFilter(ExpressionStarter<DbUser> query, IUserFilter filter)
    {
        query = query.And(x => x.ClientId == filter.ClientId);

        if (!string.IsNullOrWhiteSpace(Username))
        {
            query = query.And(x => x.EmailAddressHmac == UsernameHmac || x.UsernameHmac == UsernameHmac);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            var nameMatch = PredicateBuilder.New<DbUser>(true);
            nameMatch = nameMatch.Or(x => x.FirstCommonName.ValueNormalised.Contains(filter.NameContains))
                .Or(x => x.LastCommonName.ValueNormalised.Contains(filter.NameContains))
                .Or(ExpressionExtensions.OrNullContains<DbUser>(u => u.MiddleCommonName.ValueNormalised, NameContains));

            query = query.And(nameMatch);
        }

        if (filter.IsSystem.HasValue)
        {
            query = query.And(x => x.IsSystem == filter.IsSystem);
        }

        return query;
    }

    public override void Map(IUserFilter source)
    {
        UsernameHmac = source.Username;
        ClientId = source.ClientId;
        NameContains = source.NameContains;
        IsSystem = source.IsSystem;
    }
}
