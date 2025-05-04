using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Get;

public record ElementFindQuery (Guid ElementId) : IRequest<IUnitResult<ElementResponseDetail>>;