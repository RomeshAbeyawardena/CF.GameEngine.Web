using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public record UpsertElementCommand(ElementDto Element) : IUnitRequest<Guid>;
