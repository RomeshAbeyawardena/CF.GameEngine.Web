using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Post;

public record PostElementTypeCommand(ElementTypeDto ElementType) : IRequest<IUnitResult<Guid>>;
