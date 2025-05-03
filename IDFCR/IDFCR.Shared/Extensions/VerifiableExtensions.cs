using IDFCR.Shared.Abstractions;

namespace IDFCR.Shared.Extensions;

public static class VerifiableExtensions
{
    public static bool IsHashValid(this IVerifiable? source, IVerifiable target)
    {
        if (source is null)
        {
            return false;
        }

        return source.Hash == target.Hash;
    }

    public static void UpdateHash<T>(this T value, Func<T, string> hashGeneration)
        where T : IVerifiable
    {
        if (value is not null)
        {
            value.Hash = hashGeneration(value);
        }
    }
}
