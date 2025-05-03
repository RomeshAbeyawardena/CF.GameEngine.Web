using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Delete;

public record ElementTypeDeleteCommand(Guid ElementTypeId) : IRequest<IUnitResult>;
