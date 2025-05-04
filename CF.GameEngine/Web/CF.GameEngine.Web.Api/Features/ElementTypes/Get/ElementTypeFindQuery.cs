using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeFindQuery(Guid ElementTypeId) : IUnitRequest<ElementTypeResponseDetail>;