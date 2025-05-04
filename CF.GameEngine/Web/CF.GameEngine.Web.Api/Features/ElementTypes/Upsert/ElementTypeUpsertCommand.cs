using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;

public record ElementTypeUpsertCommand(ElementTypeDto ElementType) : IUnitRequest<Guid>;
