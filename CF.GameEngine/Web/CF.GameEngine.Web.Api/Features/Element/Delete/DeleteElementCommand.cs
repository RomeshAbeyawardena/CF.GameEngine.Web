using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Delete;

public record DeleteElementCommand(Guid ElementId) : IUnitRequest;
