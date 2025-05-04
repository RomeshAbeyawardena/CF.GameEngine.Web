using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Put;

public record PutElementCommand(ElementDto Element) : IUnitRequest<Guid>;
