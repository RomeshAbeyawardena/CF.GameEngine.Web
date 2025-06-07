using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Introspect;

public record IntrospectQuery(string Token, Guid ClientId) : IUnitRequest<IntrospectResponse>;
