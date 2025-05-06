using CF.GameEngine.Web.Api.Features.ElementTypes;
using IDFCR.Shared.Http.Links;

namespace CF.GameEngine.Web.Api.Endpoints.ElementTypes.Get;

public class ElementTypeResponseLinkBuilder : DeferredLinkBuilder<ElementTypeResponse>
{
    public ElementTypeResponseLinkBuilder(ILinkKeyDirective linkKeyDirective) : base(linkKeyDirective)
    {
        AddDeferredSelfLink(nameof(Endpoints.FindElementTypeAsync), expressions: [x => x.Id]);
    }
}
