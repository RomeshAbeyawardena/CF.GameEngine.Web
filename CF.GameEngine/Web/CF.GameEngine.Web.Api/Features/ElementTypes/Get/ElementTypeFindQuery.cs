using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Get;

public record ElementTypeFindQuery(Guid ElementTypeId) : IUnitRequest<ElementTypeResponseDetail>;