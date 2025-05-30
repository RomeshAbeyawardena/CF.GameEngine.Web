namespace IDFCR.Shared.Abstractions.Cryptography;

public record DefaultProtectionInfo(string Hmac, string CasingImpressions) : IProtectionInfo;
