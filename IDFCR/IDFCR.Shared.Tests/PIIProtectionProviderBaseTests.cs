using IDFCR.Shared.Abstractions.Cryptography;
using IDFCR.Shared.Tests.TestModels;
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
        var model = new CustomerProtectionModel(Encoding.UTF8);

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
        model = new CustomerProtectionModel(Encoding.UTF8);
        
        model.Unprotect(customer);

        Assert.That(customer.Name, Is.EqualTo("John Doe"));
        Assert.That(customer.Email, Is.EqualTo("John.Doe@gmail.com"));
        Assert.That(customer.PhoneNumber, Is.EqualTo("0123-456-7890"));

        Assert.That(model.VerifyHashUsing(customer, x => x.Password, "SomePassword1"), Is.True);

        Assert.That(model.VerifyHmacUsing(customer, x => x.Name, "john doe"), Is.True);
    }

    [Test]
    public void ModelProtectionTests_with_dynamic_salt()
    {
        var model = new CustomerProtectionModelWithDynamicSalt(Encoding.UTF8);
        model.ExternalSaltProvider = new ExternalSaltProvider();
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
        model = new CustomerProtectionModelWithDynamicSalt(Encoding.UTF8);
        model.ExternalSaltProvider = new ExternalSaltProvider();

        model.Unprotect(customer);

        Assert.That(customer.Name, Is.EqualTo("John Doe"));
        Assert.That(customer.Email, Is.EqualTo("John.Doe@gmail.com"));
        Assert.That(customer.PhoneNumber, Is.EqualTo("0123-456-7890"));

        Assert.That(model.VerifyHashUsing(customer, x => x.Password, "SomePassword1"), Is.True);

        Assert.That(model.VerifyHmacUsing(customer, x => x.Name, "john doe"), Is.True);
    }

    [Test]
    public void ModelProtectionTestsWOutCiAndHmac()
    {
        var model = new CustomerProtectionModelWOutHmacAndCi(Encoding.UTF8);

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
        model = new CustomerProtectionModelWOutHmacAndCi(Encoding.UTF8);

        model.Unprotect(customer);

        Assert.That(customer.Name, Is.EqualTo("John Doe".ToUpperInvariant()));
        Assert.That(customer.Email, Is.EqualTo("John.Doe@gmail.com".ToUpperInvariant()));
        Assert.That(customer.PhoneNumber, Is.EqualTo("0123-456-7890"));

        Assert.That(model.VerifyHashUsing(customer, x => x.Password, "SomePassword1"), Is.True);
    }
}
