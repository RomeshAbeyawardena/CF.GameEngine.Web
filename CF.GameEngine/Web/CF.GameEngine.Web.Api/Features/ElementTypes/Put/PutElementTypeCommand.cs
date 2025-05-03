using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Put;

public record PutElementTypeCommand(ElementTypeDto ElementType) : IRequest<IUnitResult<Guid>>;
