using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Delete;

public record ElementTypeDeleteCommand(Guid ElementTypeId) : IUnitRequest;
