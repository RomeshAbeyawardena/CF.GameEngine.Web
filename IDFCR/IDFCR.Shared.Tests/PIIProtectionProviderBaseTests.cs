using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void Test()
    {
        var b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "MyVeryLongApplicationSecret", "MyVeryLongClientApplicationSet");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        var c = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, b.Item1, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
        Assert.That(b.Item2, Is.EqualTo(c.Item2));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "Exactly 16 chars", "Exactly 16 chars");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
    }
    [Test]
    public void Test2()
    {
        var model = new MyProtectionModel(Encoding.UTF8);

        var customer = new Customer
        {
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Password = "SomePassword1",
            PhoneNumber = "0123-456-7890"
        };

        model.Protect(customer);
    }

    public class Customer()
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClientId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RowVersion { get; set; } = null!;
        public string? MetaData { get; set; }
    }

    internal class MyProtectionModel: PIIProtectionBase<Customer>
    {
        protected override string GetKey(Customer entity)
        {
            //using something that should not change or collide
            return Convert.ToBase64String(GenerateKey(entity, 32, ',',Encoding, entity.Id.ToString(), entity.ClientId.ToString()));
        }

        public MyProtectionModel(Encoding encoding) : base(encoding)
        {
            SetMetaData(x => x.MetaData);
            SetRowVersion(x => x.RowVersion);

            ProtectSymmetric(x => x.Name);
            ProtectSymmetric(x => x.Email);
            ProtectHashed(x => x.Password, "Secret", "Salt", System.Security.Cryptography.HashAlgorithmName.SHA384);
            ProtectSymmetric(x => x.PhoneNumber);
        }
    }
}
