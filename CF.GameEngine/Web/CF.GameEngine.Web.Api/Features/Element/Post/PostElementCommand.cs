using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Post;

public record PostElementCommand(ElementDto Element) : IUnitRequest<Guid>;
