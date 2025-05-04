using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Delete;

public record ElementTypeDeleteCommand(Guid ElementTypeId) : IUnitRequest;
