using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Links;
using IDFCR.Shared.Http.Results;
using System.Text.Json;

namespace IDFCR.Shared.Http.Tests;

[TestFixture]
internal class HypermediaResultTests
{
    [Test]
    public void T1()
    {
        var expectedItem = new Customer(Guid.Parse("3ed72f33-6fc0-478e-ac94-260b9f63dfc4"), Guid.Parse("5491f701-caef-4f51-84bc-5b6bd750c0ef"), "John", "Doe", "394423943", "22 Sanderstreet");
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

        var expectedDate = new DateTime(2025, 5, 26, 20, 29, 40, DateTimeKind.Utc);

        item.AddLink("self", new Link($"api/customer/{expectedItem.Id}", "GET", "Customer"));
        item.AddMeta("created", expectedDate);

        var result = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
        Assert.That(result, Is.Not.Null.Or.Empty, "Result should not be null or empty.");
        Assert.That(result, Is.EqualTo("{\r\n  \"id\": \"3ed72f33-6fc0-478e-ac94-260b9f63dfc4\",\r\n  \"titleId\": \"5491f701-caef-4f51-84bc-5b6bd750c0ef\",\r\n  \"name\": \"John\",\r\n  \"email\": \"Doe\",\r\n  \"phoneNumber\": \"394423943\",\r\n  \"address\": \"22 Sanderstreet\",\r\n  \"_links\": {\r\n    \"self\": {\r\n      \"Href\": \"api/customer/3ed72f33-6fc0-478e-ac94-260b9f63dfc4\",\r\n      \"Method\": \"GET\",\r\n      \"Type\": \"Customer\"\r\n    }\r\n  },\r\n  \"_meta\": {\r\n    \"created\": \"2025-05-26T20:29:40Z\"\r\n  }\r\n}"));

        expectedItem = new Customer(Guid.Parse("3ed72f33-6fc0-478e-ac94-260b9f63dfc4"), Guid.Parse("5491f701-caef-4f51-84bc-5b6bd750c0ef"), "John", null, null, null);
        item = new Hypermedia<Customer>(expectedItem);


    }
}
