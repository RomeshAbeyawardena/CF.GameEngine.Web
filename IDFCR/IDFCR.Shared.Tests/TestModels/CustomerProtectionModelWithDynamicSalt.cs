using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests.TestModels;

internal class CustomerProtectionModelWithDynamicSalt : PIIProtectionBase<Customer>
{
    protected override string GetKey(Customer entity)
    {
        //using something that should not change or collide
        return GenerateKey(entity, 32, ',', entity.Id.ToString("X").Substring(0, 15), entity.ClientId.ToString("X"));
    }

    public CustomerProtectionModelWithDynamicSalt(Encoding encoding) : base(encoding)
    {
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);

        ProtectSymmetric(x => x.Name);
        MapProtectionInfoTo(x => x.Name, BackingStore.Hmac, x => x.NameHmac);
        MapProtectionInfoTo(x => x.Name, BackingStore.CasingImpression, x => x.NameCI);

        ProtectSymmetric(x => x.Email);
        MapProtectionInfoTo(x => x.Email, BackingStore.Hmac, x => x.EmailHmac);
        MapProtectionInfoTo(x => x.Email, BackingStore.CasingImpression, x => x.EmailCI);

        ProtectHashed(x => x.Password, ExternalSaltProvider.GetSalt, System.Security.Cryptography.HashAlgorithmName.SHA384);
        ProtectSymmetric(x => x.PhoneNumber);
        MapProtectionInfoTo(x => x.PhoneNumber, BackingStore.CasingImpression, x => x.PhoneNumberCI);
        MapProtectionInfoTo(x => x.PhoneNumber, BackingStore.Hmac, x => x.PhoneNumberHmac);

        ProtectSymmetric(x => x.Blank);
        MapProtectionInfoTo(x => x.Blank, BackingStore.CasingImpression, x => x.BlankCI);
        MapProtectionInfoTo(x => x.Blank, BackingStore.Hmac, x => x.BlankHmac);
    }

    private const string ExternalSaltProviderKey = nameof(ExternalSaltProviderKey);
    
    public ExternalSaltProvider ExternalSaltProvider { 
        get => Get<ExternalSaltProvider>(ExternalSaltProviderKey) ?? throw new NullReferenceException();
        set => Set(ExternalSaltProviderKey, value); 
    }
}

public class ExternalSaltProvider
{
    public string GetSalt(Customer customer)
    {
        // This method should return a dynamic salt based on the customer or some other criteria.
        // For simplicity, we can return a static value or generate a unique one.
        return customer.Id.ToString("X").Substring(0, 8); // Example: using part of the customer's ID as a salt
    }
}