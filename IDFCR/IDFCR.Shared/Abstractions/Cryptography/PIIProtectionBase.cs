using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions.Cryptography;

public enum SymmetricAlgorithmName
{
    Aes
}

public enum BackingStore
{
    Hmac,
    CasingImpression
}

public abstract class PIIProtectionBase<T>(Encoding encoding) : PIIProtectionProviderBase(encoding), IPIIProtection<T>
{
    private readonly Dictionary<string, PIIProtectionFactory<T>> protectionFactories = [];
    private readonly Dictionary<string, IProtectionInfo> protectionData = [];
    private readonly Dictionary<string, Expression<Func<T, string>>> protectionInfoHmacBackingStore = [];
    private readonly Dictionary<string, Expression<Func<T, string>>> protectionInfoCiBackingStore = [];

    private static SymmetricAlgorithm GetAlgorithm(SymmetricAlgorithmName algorithmName)
    {
        return algorithmName switch
        {
            SymmetricAlgorithmName.Aes => Aes.Create(),
            _ => throw new NotSupportedException($"Algorithm '{algorithmName}' is not supported.")
        };
    }

    protected Expression<Func<T, string?>>? RowVersionMember { get; private set; }
    protected Expression<Func<T, string?>>? MetaDataMember { get; private set; }
    protected Encoding Encoding => encoding;
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
    protected void SetMetaData(Expression<Func<T, string?>> member)
    {
        MetaDataMember = member;
    }
    protected SymmetricAlgorithm UseAlgorithm(
        SymmetricAlgorithmName algorithmName, T entity, bool regenerateIv = false)
    {
        var algorithm = GetAlgorithm(algorithmName);

        algorithm.Key = Convert.FromBase64String(GetKey(entity));
        var rowVersion = RowVersionMember?.Compile()?.Invoke(entity);
        if (regenerateIv || string.IsNullOrWhiteSpace(rowVersion))
        {
            algorithm.GenerateIV();
            SetMemberValue(entity, RowVersionMember!, $"{Convert.ToBase64String(algorithm.IV)}");
        }
        else
        {
            algorithm.IV = Convert.FromBase64String(rowVersion);
        }

        return algorithm;
    }

    protected void SetMemberValue(T instance, Expression<Func<T, string>> expr, string value)
    {
        var visitor = new LinkExpressionVisitor();
        visitor.Visit(expr);
        var member = visitor.Member;
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

    protected void ProtectHashed(Expression<Func<T, string>> member, string secret, string salt, HashAlgorithmName algorithm, int length = 64)
    {
        For(member,
            (provider, value, context) =>
            {
                var info = GetProtectionInfo(context, value);
                var hash = Hash(algorithm, secret, salt, length);
                SetMemberValue(context, member, hash);
                return info;
            },
            (_, _, _, _) => { });
    }

    protected void MapProtectionInfoTo(Expression<Func<T, string>> member, BackingStore backingStore, Expression<Func<T, string>> target)
    {
        var linkVisitor = new LinkExpressionVisitor();
        linkVisitor.Visit(member);
        switch (backingStore)
        {
            case BackingStore.Hmac:
                protectionInfoHmacBackingStore.Add(linkVisitor.MemberName!, target);
                break;
            case BackingStore.CasingImpression:
                protectionInfoCiBackingStore.Add(linkVisitor.MemberName!, target);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(backingStore), backingStore, null);
        }
    }

    protected IProtectionInfo GetProtectionInfo(T context, string value)
    {
        var hmac = HashWithHMAC(GetKey(context), value);
        var caseImpressions = CasingImpression.Calculate(value);

        return new DefaultProtectionInfo(hmac, caseImpressions);
    }

    protected string GenerateKey(T entity, int length, char separator, Encoding encoding, params string[] values)
    {
        var metaData = MetaDataMember?.Compile()(entity);

        if (!string.IsNullOrWhiteSpace(metaData))
        {
            metaData = encoding.GetString(Convert.FromBase64String(metaData));
        }

        var keyData = GenerateKey(length, separator, metaData, values);

        //if all spaces are populated sufficiently with key data, this metadata will be an empty string and won't need persisting to the database
        if (!string.IsNullOrWhiteSpace(keyData.Item1))
        {
            SetMemberValue(entity, MetaDataMember!, Convert.ToBase64String(encoding.GetBytes(keyData.Item1)));
        }

        return Convert.ToBase64String(keyData.Item2);
    }

    protected string GetMemberValue(T instance, Expression<Func<T, string>> expr)
    {
        var visitor = new LinkExpressionVisitor();
        visitor.Visit(expr);
        var member = visitor.Member;
        if (member is PropertyInfo pi)
        {
            return pi.GetValue(instance) as string ?? string.Empty;
        }

        return string.Empty;
    }


    public IReadOnlyDictionary<string, IProtectionInfo> ExtractProtectionInfo(T entity)
    {
        var result = new Dictionary<string, IProtectionInfo>();

        foreach (var key in protectionFactories.Keys)
        {
            var hmac = protectionInfoHmacBackingStore.TryGetValue(key, out var hmacExpr)
                ? GetMemberValue(entity, hmacExpr)
                : string.Empty;

            var ci = protectionInfoCiBackingStore.TryGetValue(key, out var ciExpr)
                ? GetMemberValue(entity, ciExpr)
                : string.Empty;

            result[key] = new DefaultProtectionInfo(hmac, ci);
        }

        return result;
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
            var pD = value.Protect(this, entry);
            protectionData.Add(key, pD);

            var backingStore = protectionInfoHmacBackingStore.GetValueOrDefault(key);
            if (backingStore is not null)
            {
                SetMemberValue(entry, backingStore, pD.Hmac);
            }

            backingStore = protectionInfoCiBackingStore.GetValueOrDefault(key);
            if (backingStore is not null)
            {
                SetMemberValue(entry, backingStore, pD.CasingImpressions);
            }
        }

        return protectionData;
    }

    public void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo> protectionData)
    {
        var strippedProtectionData = ExtractProtectionInfo(entry);

        foreach(var (key, value) in protectionFactories)
        {
            // Try protection data from caller first; fallback to internal copy.
            // This guards against DI scope misalignment (e.g., transient reuse).
            if (!protectionData.TryGetValue(key, out var protectionInfo) 
                && !strippedProtectionData.TryGetValue(key, out protectionInfo)
                && !this.protectionData.TryGetValue(key, out protectionInfo))
            {
                throw new KeyNotFoundException($"Protection data for key '{key}' not found.");
            }

            value.Unprotect(this, entry, protectionInfo ?? throw new NullReferenceException());
        }
    }
}
