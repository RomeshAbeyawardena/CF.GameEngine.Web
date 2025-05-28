using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions.Cryptography;

public enum SymmetricAlgorithmName
{
    Aes
}

public abstract class PIIProtectionBase<T>(Encoding encoding) : PIIProtectionProviderBase, IPIIProtection<T>
{
    private readonly Dictionary<string, PIIProtectionFactory<T>> protectionFactories = [];
    private readonly Dictionary<string, IProtectionInfo> protectionData = [];

    private static SymmetricAlgorithm GetAlgorithm(SymmetricAlgorithmName algorithmName)
    {
        return algorithmName switch
        {
            SymmetricAlgorithmName.Aes => Aes.Create(),
            _ => throw new NotSupportedException($"Algorithm '{algorithmName}' is not supported.")
        };
    }

    protected Expression<Func<T, string?>>? RowVersionMember { get; private set; }

    protected abstract string GetKey(T entity);

    protected PIIProtectionBase<T> For(Expression<Func<T, string>> member, Func<PIIProtectionProviderBase, string, T, IProtectionInfo> protect,
    Action<PIIProtectionProviderBase, string, T, IProtectionInfo> unprotect)
    {
        var vis = new LinkExpressionVisitor();
        vis.Visit(member);

        protectionFactories.Add(vis.MemberName!,
            new PIIProtectionFactory<T>(member, protect, unprotect));

        return this;
    }

    protected void SetRowVersion(Expression<Func<T, string?>> member)
    {
        RowVersionMember = member;
    }

    protected SymmetricAlgorithm UseAlgorithm(
        SymmetricAlgorithmName algorithmName, T entity, bool regenerateIv = false)
    {
        var algorithm = GetAlgorithm(algorithmName);

        algorithm.Key = encoding.GetBytes(GetKey(entity));
        var rowVersion = RowVersionMember?.Compile()?.Invoke(entity);
        if (regenerateIv || string.IsNullOrWhiteSpace(rowVersion))
        {
            algorithm.GenerateIV();
            var exp = new LinkExpressionVisitor();
            exp.Visit(RowVersionMember);
            if (exp.Member is PropertyInfo prop)
            {
                prop.SetValue(entity, $"{Convert.ToBase64String(algorithm.IV)}");
            }
        }
        else
        {
            algorithm.IV = Convert.FromBase64String(rowVersion);
        }

        return algorithm;
    }

    protected void SetMemberValue(T instance, Expression<Func<T, string>> expr, string value)
    {
        var member = ((MemberExpression)expr.Body).Member;
        if (member is PropertyInfo pi)
        {
            pi.SetValue(instance, value);
        }
    }

    protected void ProtectSymmetric(Expression<Func<T, string>> member)
    {
        For(member,
            (provider, value, context) =>
            {
                var info = GetProtectionInfo(context, value);
                var encrypted = Encrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                SetMemberValue(context, member, encrypted);
                return info;
            },
            (provider, value, context, info) =>
            {
                var decrypted = Decrypt(value, UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
                var restored = CasingImpression.Restore(decrypted, info.CasingImpressions);
                SetMemberValue(context, member, restored);
            });
    }

    protected void ProtectHashed(Expression<Func<T, string>> member, string secret, string salt, HashAlgorithmName algorithm)
    {
        For(member,
            (provider, value, context) =>
            {
                var info = GetProtectionInfo(context, value);
                var hash = Hash(algorithm, secret, salt, 64);
                SetMemberValue(context, member, hash);
                return info;
            },
            (_, _, _, _) => { });
    }


    protected IProtectionInfo GetProtectionInfo(T context, string value)
    {
        var hmac = HashWithHMAC(GetKey(context), value);
        var caseImpressions = CasingImpression.Calculate(value);

        return new DefaultProtectionInfo(hmac, caseImpressions);
    }

    public string Hash(HashAlgorithmName algorithmName, string secret, string salt, int length)
    {
        var derived = new Rfc2898DeriveBytes(encoding.GetBytes(secret), 
            encoding.GetBytes(salt), 100_000, algorithmName);
        return Convert.ToBase64String(derived.GetBytes(length));
    }

    public string HashWithHMAC(string key, string data)
    {
        return Convert.ToBase64String(
            HMACSHA512.HashData(encoding.GetBytes(key), encoding.GetBytes(data)));
    }

    public IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry)
    {
        protectionData.Clear();
        foreach (var (key, value) in protectionFactories)
        {
            protectionData.Add(key, value.Protect(this, entry));
        }

        return protectionData;
    }

    public void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo> protectionData)
    {
        foreach(var (key, value) in protectionFactories)
        {
            // Try protection data from caller first; fallback to internal copy.
            // This guards against DI scope misalignment (e.g., transient reuse).
            if (!protectionData.TryGetValue(key, out var protectionInfo) 
                && !this.protectionData.TryGetValue(key, out protectionInfo))
            {
                throw new KeyNotFoundException($"Protection data for key '{key}' not found.");
            }

            value.Unprotect(this, entry, protectionInfo);
        }
    }
}
