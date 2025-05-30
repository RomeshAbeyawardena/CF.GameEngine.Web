using IDFCR.Shared.Abstractions.Cryptography;
using System.Text;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void GeneralEncryptionTests()
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
    public void ModelProtectionTests()
    {
        var model = new MyProtectionModel(Encoding.UTF8);

        var customer = new Customer
        {
            Name = "John Doe",
            Email = "John.Doe@gmail.com",
            Password = "SomePassword1",
            PhoneNumber = "0123-456-7890"
        };

        var ip = model.Protect(customer);

        Assert.That(customer.Name, Is.Not.EqualTo("John Doe"));
        Assert.That(customer.Email, Is.Not.EqualTo("John.Doe@gmail.com"));
        Assert.That(customer.PhoneNumber, Is.Not.EqualTo("0123-456-7890"));
        
        //kills off internal caches, the model will be relied upon for the decryption process and the application (test) manages the key
        model = new MyProtectionModel(Encoding.UTF8);
        
        model.Unprotect(customer);

        Assert.That(customer.Name, Is.EqualTo("John Doe"));
        Assert.That(customer.Email, Is.EqualTo("John.Doe@gmail.com"));
        Assert.That(customer.PhoneNumber, Is.EqualTo("0123-456-7890"));

        Assert.That(model.VerifyHashUsing(customer, x => x.Password, "SomePassword1"), Is.True);
    }

    [Test]
    public void ModelProtectionTestsWOutCiAndHmac()
    {
        var model = new MyProtectionModelWOutHmacAndCi(Encoding.UTF8);

        var customer = new Customer
        {
            Name = "John Doe",
            Email = "John.Doe@gmail.com",
            Password = "SomePassword1",
            PhoneNumber = "0123-456-7890"
        };

        var ip = model.Protect(customer);

        Assert.That(customer.Name, Is.Not.EqualTo("John Doe"));
        Assert.That(customer.Email, Is.Not.EqualTo("John.Doe@gmail.com"));
        Assert.That(customer.PhoneNumber, Is.Not.EqualTo("0123-456-7890"));

        //kills off internal caches, the model will be relied upon for the decryption process and the application (test) manages the key
        model = new MyProtectionModelWOutHmacAndCi(Encoding.UTF8);

        model.Unprotect(customer);

        Assert.That(customer.Name, Is.EqualTo("John Doe".ToUpperInvariant()));
        Assert.That(customer.Email, Is.EqualTo("John.Doe@gmail.com".ToUpperInvariant()));
        Assert.That(customer.PhoneNumber, Is.EqualTo("0123-456-7890"));

        Assert.That(model.VerifyHashUsing(customer, x => x.Password, "SomePassword1"), Is.True);
    }

    public class Customer()
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClientId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string NameHmac { get; set; } = null!;
        public string NameCI { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string EmailHmac { get; set; } = null!;
        public string EmailCI { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PhoneNumberCI { get; set; } = null!;
        public string PhoneNumberHmac { get; set; } = null!;
        public string RowVersion { get; set; } = null!;
        public string? MetaData { get; set; }
    }

    internal class MyProtectionModelWOutHmacAndCi : PIIProtectionBase<Customer>
    {
        protected override string GetKey(Customer entity)
        {
            //using something that should not change or collide
            return GenerateKey(entity, 32, ',', entity.Id.ToString("X").Substring(0, 15), entity.ClientId.ToString("X"));
        }

        public MyProtectionModelWOutHmacAndCi(Encoding encoding) : base(encoding)
        {
            SetMetaData(x => x.MetaData);
            SetRowVersion(x => x.RowVersion);

            ProtectSymmetric(x => x.Name);
            
            ProtectSymmetric(x => x.Email);
            
            ProtectHashed(x => x.Password, "Salt", System.Security.Cryptography.HashAlgorithmName.SHA384);
            ProtectSymmetric(x => x.PhoneNumber);
        }
    }

    internal class MyProtectionModel: PIIProtectionBase<Customer>
    {
        protected override string GetKey(Customer entity)
        {
            //using something that should not change or collide
            return GenerateKey(entity, 32, ',', entity.Id.ToString("X").Substring(0,15), entity.ClientId.ToString("X"));
        }

        public MyProtectionModel(Encoding encoding) : base(encoding)
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
}
