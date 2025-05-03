using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;

public record ElementTypeUpsertCommand(ElementTypeDto ElementType) : IRequest<IUnitResult<Guid>>;
