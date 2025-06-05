using CF.Identity.Api.Features.Scopes;
using IDFCR.Shared.Http.Links;

namespace CF.Identity.Api.Endpoints.Scopes;

public class ScopeLinkBuilder : DeferredLinkBuilder<ScopeDto>
{
    public ScopeLinkBuilder(ILinkKeyDirective linkKeyDirective) : base(linkKeyDirective)
    {
        AddDeferredSelfLink(Endpoints.GetScope, expressions: x => x.Id);
    }
}
