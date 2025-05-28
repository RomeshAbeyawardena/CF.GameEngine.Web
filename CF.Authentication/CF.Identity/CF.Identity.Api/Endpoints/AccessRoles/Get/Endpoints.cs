using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions.Paging;
using MediatR;

namespace CF.Identity.Api.Endpoints.AccessRoles.Get;

public record GetRolesRequest : MappablePagedQuery<IPagedAccessRoleFilter>, IPagedAccessRoleFilter
{
    protected override IPagedAccessRoleFilter Source => this;

    public Guid? ClientId { get; set; }
    public string? NameContains { get; set; }
    public bool NoTracking { get; set; }
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }

    public override void Map(IPagedAccessRoleFilter source)
    {
        throw new NotSupportedException();
    }
}

public static class Endpoints
{
    //public static async Task<IResult> GetRoles(
    //    [AsParameters] GetRolesRequest request,
    //    IMediator mediator, CancellationToken cancellationToken)
    //{
    //    var result = await mediator.Send(new FindAccessRoleQuery(), cancellationToken);
    //}
}
