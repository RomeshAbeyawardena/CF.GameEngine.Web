using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public record UpsertElementCommand(ElementDto Element) : IRequest<IUnitResult<Guid>>;
