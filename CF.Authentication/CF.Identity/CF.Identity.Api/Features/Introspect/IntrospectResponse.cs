namespace CF.Identity.Api.Features.Introspect;

public record IntrospectResponse(bool Active, string ClientId, string Scope, string Exp, string Aud, string Iss) : IntrospectBaseResponse(Active);
