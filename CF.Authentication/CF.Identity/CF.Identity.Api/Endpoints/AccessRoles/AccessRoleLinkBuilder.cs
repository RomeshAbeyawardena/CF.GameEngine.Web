using CF.Identity.Api.Features.AccessRoles;
using IDFCR.Shared.Http.Links;

namespace CF.Identity.Api.Endpoints.AccessRoles;

public class AccessRoleLinkBuilder : DeferredLinkBuilder<AccessRoleDto>
{
    public AccessRoleLinkBuilder(ILinkKeyDirective linkKeyDirective) : base(linkKeyDirective)
    {
        base.AddDeferredSelfLink(Endpoints.GetAccessRole, expressions: x => x.Id);
        base.AddDeferredLink(Clients.Endpoints.GetClient, rel: "Client",
            expressionResolver: c => c.AddOrUpdate(x => x.ClientId!, "id"),
            expressions: x => x.ClientId);
    }
}
