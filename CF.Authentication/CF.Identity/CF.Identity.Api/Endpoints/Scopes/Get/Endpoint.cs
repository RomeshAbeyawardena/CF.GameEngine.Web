using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Shared.Abstractions.Records;
using IDFCR.Shared.Http.Extensions;
using MediatR;

namespace CF.Identity.Api.Endpoints.Scopes.Get;

public record GetScopesRequest(string? Key = null, IEnumerable<string>? Keys = null) : MappableBase<IScopeFilter>
{
    protected override IScopeFilter Source => new FindScopesQuery
    {
        Key = Key,
        Keys = Keys,
        UserId = UserId,
        ClientId = ClientId
    };

    public Guid? UserId { get; init; }
    public Guid? ClientId { get; set; }

    public override void Map(IScopeFilter source)
    {
        throw new NotSupportedException();
    }

    public FindScopesQuery ToQuery()
    {
        return Map<FindScopesQuery>();
    }
}

public static class Endpoint
{
    public static async Task<IResult> GetScopePagedAsync([AsParameters] GetScopesRequest request, IAuthenticatedUserContext userContext,
       IMediator mediator, IHttpContextAccessor contextAccessor, CancellationToken cancellationToken)
    {
        if (!userContext.Client?.IsSystem ?? false)
        {
            request.ClientId = userContext.Client?.Id;
        }

        var query = request.ToQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.NegotiateResult(contextAccessor, Endpoints.BaseUrl);
    }
}
