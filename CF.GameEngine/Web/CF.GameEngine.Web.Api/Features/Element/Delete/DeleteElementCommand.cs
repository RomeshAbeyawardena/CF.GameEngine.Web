using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Delete;

public record DeleteElementCommand(Guid ElementId) : IRequest<IUnitResult>;
