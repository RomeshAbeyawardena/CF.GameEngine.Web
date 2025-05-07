using CF.GameEngine.Web.Api.Features.Element;
using IDFCR.Shared.Http.Links;
using ElementTypeEndpoint = CF.GameEngine.Web.Api.Endpoints.ElementTypes.Get.Endpoints;
namespace CF.GameEngine.Web.Api.Endpoints.Element.Get;

public class ElementResponseLinkBuilder : DeferredLinkBuilder<ElementResponse>
{
    public ElementResponseLinkBuilder(ILinkKeyDirective linkKeyDirective) : base(linkKeyDirective)
    {
        AddDeferredSelfLink(nameof(Endpoints.FindElementAsync), expressions: [x => x.Id]);
        AddDeferredLink(nameof(ElementTypeEndpoint.FindElementTypeAsync), 
            expressionResolver: c=>c.AddOrUpdate(x => x.ElementTypeId, "id"), 
            expressions: [x => x.ElementTypeId]);
    }
}
