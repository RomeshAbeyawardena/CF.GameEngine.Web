using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public record UpsertElementCommand(ElementDto Element) : IUnitRequest<Guid>;
