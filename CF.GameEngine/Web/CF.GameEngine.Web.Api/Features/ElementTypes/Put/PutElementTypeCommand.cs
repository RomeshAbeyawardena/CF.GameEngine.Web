using IDFCR.Shared.Mediatr;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Put;

public record PutElementTypeCommand(ElementTypeDto ElementType) : IUnitRequest<Guid>;
