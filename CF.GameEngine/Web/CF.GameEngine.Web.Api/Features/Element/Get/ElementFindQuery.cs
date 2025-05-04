using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementFindQuery (Guid ElementId) : IUnitRequest<ElementResponseDetail>;