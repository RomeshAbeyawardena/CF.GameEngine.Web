using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeFindQuery(Guid ElementTypeId) : IRequest<IUnitResult<ElementTypeResponseDetail>>;