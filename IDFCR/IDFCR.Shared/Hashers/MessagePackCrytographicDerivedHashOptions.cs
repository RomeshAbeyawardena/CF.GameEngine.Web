using MessagePack;

namespace IDFCR.Shared.Hashers;

public record MessagePackCrytographicDerivedHashOptions(
    MessagePackSerializerOptions MessagePackSerializerOptions,
    string HashAlgorithmName
);
