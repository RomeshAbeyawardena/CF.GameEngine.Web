using MessagePack;

namespace StateManagent.Web.Infrastructure.Hashers;

public record MessagePackCrytographicDerivedHashOptions(
    MessagePackSerializerOptions MessagePackSerializerOptions,
    string HashAlgorithmName
);
