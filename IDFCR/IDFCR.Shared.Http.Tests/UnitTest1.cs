using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace IDFCR.Shared.Http.Tests;

public record Customer(Guid Id, Guid TitleId, string Name, string Email, string PhoneNumber, string Address);

public class CustomerLinkBuilder : LinkBuilder<Customer>
{
    public CustomerLinkBuilder() : base(new DefaultLinkKeyDirective(LinkKeyDirectiveOptions.Default))
    {
        AddSelf("customer/{id}", expressions: [x => x.Id]);
        AddLink("title/{TitleId}", expressions: [x => x.TitleId]);
    }
}


public class DeferredCustomerLinkBuilder : DeferredLinkBuilder<Customer>
{
    public DeferredCustomerLinkBuilder() : base(new DefaultLinkKeyDirective(LinkKeyDirectiveOptions.Default))
    {
        AddDeferredSelfLink("GetCustomerById", expressions: [x => x.Id]);
        AddDeferredLink("GetTitleById", expressions: [x => x.TitleId]);
    }
}

public class MockLinkGenerator : LinkGenerator
{
    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        if (address is string strAddress)
        {
            return "";
        }
        throw new NotImplementedException();
    }

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        if(address is string)
        {
            if (address.Equals("GetCustomerById"))
            {
                if (values.TryGetValue("id", out var id))
                {
                    return $"customer/{id}";
                }
            }

            if (address.Equals("GetTitleById"))
            {
                if (values.TryGetValue("titleid", out var id))
                {
                    return $"title/{id}";
                }
            }
        }
        throw new NotImplementedException();
    }

    public override string? GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public override string? GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        throw new NotImplementedException();
    }
}

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_using_custom_route()
    {
        var builder = new CustomerLinkBuilder();
        var generator = builder.Build(new MockLinkGenerator());
        var id = Guid.Parse("5a33c641-60dd-47da-be0e-74b5498f3bc1");
        var titleId = Guid.Parse("b9229284-6894-417e-b4a6-af8cc99eb5b0");
        var links = generator.GenerateLinks(new(id, titleId, "John", "john.doe@customer.net", "0021943322", "32 Baker street"));

        Assert.That(links, Has.Count.EqualTo(2));
        Assert.That(links.TryGetValue("_self", out var self), Is.True);
        Assert.That(self, Is.Not.Null);
        Assert.That(self.Href, Is.EqualTo($"customer/{id}"));
        Assert.That(links.TryGetValue("title", out var title), Is.True);
        Assert.That(title, Is.Not.Null);
        Assert.That(title.Href, Is.EqualTo($"title/{titleId}"));
    }

    [Test]
    public void Test_using_aspnet_routes()
    {
        var builder = new DeferredCustomerLinkBuilder();
        var generator = builder.Build(new MockLinkGenerator());
        var id = Guid.Parse("5a33c641-60dd-47da-be0e-74b5498f3bc1");
        var titleId = Guid.Parse("b9229284-6894-417e-b4a6-af8cc99eb5b0");
        var links = generator.GenerateLinks(new(id, titleId, "John", "john.doe@customer.net", "0021943322", "32 Baker street"));

        Assert.That(links, Has.Count.EqualTo(2));
        Assert.That(links.TryGetValue("_self", out var self), Is.True);
        Assert.That(self, Is.Not.Null);
        Assert.That(self.Href, Is.EqualTo($"customer/{id}"));
        Assert.That(links.TryGetValue("title", out var title), Is.True);
        Assert.That(title, Is.Not.Null);
        Assert.That(title.Href, Is.EqualTo($"title/{titleId}"));
    }
}
