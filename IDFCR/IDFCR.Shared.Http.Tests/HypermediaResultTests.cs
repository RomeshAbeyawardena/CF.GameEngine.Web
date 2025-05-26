using IDFCR.Shared.Http.Links;
using System.Text.Json;

namespace IDFCR.Shared.Http.Tests;

[TestFixture]
internal class HypermediaResultTests
{
    [Test]
    public void T1()
    {
        var expectedItem = new Customer(Guid.NewGuid(), Guid.NewGuid(), "John", "Doe", "394423943", "22 Sanderstreet");
        var item = new Hypermedia<Customer>(expectedItem);
        
        Assert.That(item.TryGetValue(nameof(Customer.Address), out var addressValue), Is.True);
        Assert.That(addressValue, Is.EqualTo(expectedItem.Address));

        Assert.That(item.TryGetValue(nameof(Customer.Email), out var emailValue), Is.True);
        Assert.That(emailValue, Is.EqualTo(expectedItem.Email));

        Assert.That(item.TryGetValue(nameof(Customer.Name), out var firstNameValue), Is.True);
        Assert.That(firstNameValue, Is.EqualTo(expectedItem.Name));

        Assert.That(item.TryGetValue(nameof(Customer.PhoneNumber), out var phoneNumber), Is.True);
        Assert.That(phoneNumber, Is.EqualTo(expectedItem.PhoneNumber));

        Assert.That(item.TryGetValue(nameof(Customer.Id), out var idValue), Is.True);
        Assert.That(idValue, Is.EqualTo(expectedItem.Id));

        Assert.That(item.TryGetValue(nameof(Customer.TitleId), out var titleId), Is.True);
        Assert.That(titleId, Is.EqualTo(expectedItem.TitleId));

        item.AddLink("self", new Link($"api/customer/{expectedItem.Id}", "GET", "Customer"));
        item.AddMeta("created", DateTime.UtcNow);

        var m = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });

    }
}
