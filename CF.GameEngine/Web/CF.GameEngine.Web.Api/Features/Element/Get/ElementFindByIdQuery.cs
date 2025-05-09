using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementFindByIdQuery (Guid ElementId) : IUnitRequest<ElementResponseDetail>;