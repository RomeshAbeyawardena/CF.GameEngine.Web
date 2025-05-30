using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests.TestModels;

internal class CustomerProtectionModelWOutHmacAndCi : PIIProtectionBase<Customer>
{
    protected override string GetKey(Customer entity)
    {
        //using something that should not change or collide
        return GenerateKey(entity, 32, ',', entity.Id.ToString("X").Substring(0, 15), entity.ClientId.ToString("X"));
    }

    public CustomerProtectionModelWOutHmacAndCi(Encoding encoding) : base(encoding)
    {
        SetMetaData(x => x.MetaData);
        SetRowVersion(x => x.RowVersion);

        ProtectSymmetric(x => x.Name);

        ProtectSymmetric(x => x.Email);

        ProtectHashed(x => x.Password, "Salt", System.Security.Cryptography.HashAlgorithmName.SHA384);
        ProtectSymmetric(x => x.PhoneNumber);
    }
}