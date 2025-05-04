using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Put;

public record PutElementTypeCommand(ElementTypeDto ElementType) : IUnitRequest<Guid>;
