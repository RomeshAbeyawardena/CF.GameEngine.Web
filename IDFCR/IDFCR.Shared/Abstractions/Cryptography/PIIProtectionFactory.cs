using System.Linq.Expressions;

namespace IDFCR.Shared.Abstractions.Cryptography;

public class PIIProtectionFactory<T>(Expression<Func<T, string>> member, Func<PIIProtectionProviderBase, string, IProtectionInfo> protect,
    Action<PIIProtectionProviderBase, string, IProtectionInfo> unprotect)
{
    public IProtectionInfo Protect(PIIProtectionProviderBase provider, T instance)
    {
        var value = member.Compile();
        return protect(provider, value(instance));
    }

    public void Unprotect(PIIProtectionProviderBase provider, T instance, IProtectionInfo protectionInfo)
    {
        var value = member.Compile();
        unprotect(provider, value(instance), protectionInfo);
    }
}
