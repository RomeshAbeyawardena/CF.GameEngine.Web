using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests.TestModels;

internal class CustomerProtectionModel : PIIProtectionBase<Customer>
{
    protected override string GetKey(Customer entity)
    {
        //using something that should not change or collide
        return GenerateKey(entity, 32, ',', entity.Id.ToString("X").Substring(0, 15), entity.ClientId.ToString("X"));
    }

    public CustomerProtectionModel(Encoding encoding) : base(encoding)
    {
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);

        ProtectSymmetric(x => x.Name);
        MapProtectionInfoTo(x => x.Name, BackingStore.Hmac, x => x.NameHmac);
        MapProtectionInfoTo(x => x.Name, BackingStore.CasingImpression, x => x.NameCI);

        ProtectSymmetric(x => x.Email);
        MapProtectionInfoTo(x => x.Email, BackingStore.Hmac, x => x.EmailHmac);
        MapProtectionInfoTo(x => x.Email, BackingStore.CasingImpression, x => x.EmailCI);

        ProtectHashed(x => x.Password, "Salt", System.Security.Cryptography.HashAlgorithmName.SHA384);
        ProtectSymmetric(x => x.PhoneNumber);
        MapProtectionInfoTo(x => x.PhoneNumber, BackingStore.CasingImpression, x => x.PhoneNumberCI);
        MapProtectionInfoTo(x => x.PhoneNumber, BackingStore.Hmac, x => x.PhoneNumberHmac);
    }
}