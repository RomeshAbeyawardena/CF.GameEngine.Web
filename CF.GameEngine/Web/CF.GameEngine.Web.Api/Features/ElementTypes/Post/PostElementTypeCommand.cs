using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Post;

public record PostElementTypeCommand(ElementTypeDto ElementType) : IUnitRequest<Guid>;
