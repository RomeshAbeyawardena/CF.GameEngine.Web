using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeFindByIdQuery(Guid ElementTypeId) : IUnitRequest<ElementTypeResponseDetail>;