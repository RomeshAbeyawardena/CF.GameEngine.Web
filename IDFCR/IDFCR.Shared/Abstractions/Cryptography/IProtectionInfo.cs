namespace IDFCR.Shared.Abstractions.Cryptography;

public interface IProtectionInfo
{
    string Hmac { get; }
    string CasingImpressions { get; }
}
