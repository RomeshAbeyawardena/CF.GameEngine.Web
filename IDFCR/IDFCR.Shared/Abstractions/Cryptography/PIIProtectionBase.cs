using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IDFCR.Shared.Abstractions.Cryptography;

public abstract class PIIProtectionBase<T>(Encoding encoding) : PIIProtectionProviderBase(encoding), IPIIProtection<T>
{
    private readonly StateBag stateBag = new();
    private readonly Dictionary<string, PIIProtectionFactory<T>> protectionFactories = [];
    private readonly Dictionary<string, IProtectionInfo> protectionDataStore = [];
    private readonly Dictionary<string, Expression<Func<T, string?>>> protectionInfoHmacBackingStore = [];
    private readonly Dictionary<string, Expression<Func<T, string?>>> protectionInfoCiBackingStore = [];
    private readonly Dictionary<string, Func<T, string, string>> hashingValueStore = [];

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

    protected abstract string GetKey(T entity);
    protected abstract string GetHmacKey();

    protected PIIProtectionBase<T> For(Expression<Func<T, string?>> member, Func<PIIProtectionProviderBase, string?, T, IProtectionInfo> protect,
    Action<PIIProtectionProviderBase, string?, T, IProtectionInfo> unprotect)
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

    protected void SetMemberValue(T instance, Expression<Func<T, string?>> expr, string value)
    {
        var visitor = new LinkExpressionVisitor();
        visitor.Visit(expr);
        var member = visitor.Member;
        if (member is PropertyInfo pi)
        {
            pi.SetValue(instance, value);
        }
    }

    protected void ProtectSymmetric(Expression<Func<T, string?>> member)
    {
        For(member,
            (provider, value, context) =>
            {
                var info = GetProtectionInfo(context, value);
                var encrypted = Encrypt(value?.ToUpperInvariant(), UseAlgorithm(SymmetricAlgorithmName.Aes, context))!;
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

    protected void ProtectHashed(Expression<Func<T, string?>> member, Func<T, string> saltGeneration, HashAlgorithmName algorithm, int length = 64)
    {
        var visitor = new LinkExpressionVisitor();
        visitor.Visit(member);

        hashingValueStore.TryAdd(visitor.MemberName!,
            (context, value) => Hash(algorithm, value, saltGeneration(context), length));

        For(member,
            (provider, value, context) =>
            {
                var info = GetProtectionInfo(context, value);
                if (value is not null)
                {
                    var hash = Hash(algorithm, value, saltGeneration(context), length);
                    SetMemberValue(context, member, hash);
                }
                return info;
            },
            (_, _, _, _) => { });
    }

    protected void ProtectHashed(Expression<Func<T, string?>> member, string salt, HashAlgorithmName algorithm, int length = 64)
    {
        ProtectHashed(member, (x) => salt, algorithm, length);
    }

    protected void MapProtectionInfoTo(Expression<Func<T, string?>> member, BackingStore backingStore, Expression<Func<T, string?>> target)
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

    protected IProtectionInfo GetProtectionInfo(T context, string? value)
    {
        var hmac = HashWithHMAC(GetHmacKey(), value?.ToUpperInvariant());
        var caseImpressions = string.IsNullOrEmpty(value) ? string.Empty : CasingImpression.Calculate(value);

        return new DefaultProtectionInfo(hmac, caseImpressions);
    }

    protected string GenerateKey(T entity, int length, char separator, params string[] values)
    {
        var metaData = MetaDataMember?.Compile()(entity);

        if (!string.IsNullOrWhiteSpace(metaData))
        {
            metaData = Encoding.GetString(Convert.FromBase64String(metaData));
        }

        var keyData = GenerateKey(length, separator, metaData, values);

        //if all spaces are populated sufficiently with key data, this metadata will be an empty string and won't need persisting to the database
        if (!string.IsNullOrWhiteSpace(keyData.Item1))
        {
            SetMemberValue(entity, MetaDataMember!, Convert.ToBase64String(Encoding.GetBytes(keyData.Item1)));
        }

        return Convert.ToBase64String(keyData.Item2);
    }

    protected string GetMemberValue(T instance, Expression<Func<T, string?>> expr)
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
        var derived = new Rfc2898DeriveBytes(Encoding.GetBytes(secret),
            Encoding.GetBytes(salt), 100_000, algorithmName);
        return Convert.ToBase64String(derived.GetBytes(length));
    }

    public string HashWithHMAC(string key, string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        return Convert.ToBase64String(
            HMACSHA512.HashData(Encoding.GetBytes(key), Encoding.GetBytes(data)));
    }

    public IReadOnlyDictionary<string, IProtectionInfo> Protect(T entry)
    {
        protectionDataStore.Clear();
        foreach (var (key, value) in protectionFactories)
        {
            var pD = value.Protect(this, entry);
            protectionDataStore.Add(key, pD);

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

        return protectionDataStore;
    }

    public void Unprotect(T entry, IReadOnlyDictionary<string, IProtectionInfo>? protectionData = null)
    {
        var strippedProtectionData = ExtractProtectionInfo(entry);
        protectionData ??= new Dictionary<string, IProtectionInfo>();

        foreach (var (key, value) in protectionFactories)
        {
            // Try protection data from caller first; fallback to internal copy.
            // This guards against DI scope misalignment (e.g., transient reuse).
            if (!protectionData.TryGetValue(key, out var protectionInfo)
                && !strippedProtectionData.TryGetValue(key, out protectionInfo)
                && !this.protectionDataStore.TryGetValue(key, out protectionInfo))
            {
                throw new KeyNotFoundException($"Protection data for key '{key}' not found.");
            }

            value.Unprotect(this, entry, protectionInfo ?? throw new NullReferenceException());
        }
    }

    public virtual TItem? Get<TItem>(string key)
    {
        return stateBag.Get<TItem>(key);
    }

    public virtual object? Get(string key)
    {
        return stateBag.Get(key);
    }

    public virtual void Set(string key, object? value)
    {
        stateBag.Set(key, value);
    }

    public bool VerifyHashUsing(T hashedEntry, Expression<Func<T, string?>> member, string valueToTest)
    {
        var visitor = new LinkExpressionVisitor();
        visitor.Visit(member);
        var memberName = visitor.MemberName ?? throw new NullReferenceException("Member not found");
        if (!hashingValueStore.TryGetValue(memberName, out var hashFunc))
        {
            throw new KeyNotFoundException($"Hashing function for member '{memberName}' not found.");
        }
        var value = GetMemberValue(hashedEntry, member);
        var hashedValue = hashFunc(hashedEntry, valueToTest);
        return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(value),
            Convert.FromBase64String(hashedValue));
    }

    public bool VerifyHmacUsing(T hashedEntry, Expression<Func<T, string?>> member, string valueToTest)
    {
        var expressionVisitor = new LinkExpressionVisitor();
        expressionVisitor.Visit(member);
        var memberName = expressionVisitor.MemberName ?? throw new NullReferenceException("Member not found");
        if (!protectionInfoHmacBackingStore.TryGetValue(memberName, out var hmacExpr))
        {
            throw new KeyNotFoundException($"HMAC backing store for member '{memberName}' not found.");
        }

        var hmacValue = GetMemberValue(hashedEntry, hmacExpr);
        var hmac = HashWithHMAC(GetHmacKey(), valueToTest.ToUpperInvariant());

        return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(hmacValue),
            Convert.FromBase64String(hmac));
    }
}