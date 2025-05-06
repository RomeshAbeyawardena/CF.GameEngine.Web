using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace IDFCR.Shared.Http.Tests;

public record Customer(Guid Id, string Name, string Email, string PhoneNumber, string Address);

public class CustomerLinkBuilder : LinkBuilder<Customer>
{
    public CustomerLinkBuilder()
    {
        AddSelf("customer/{id}", expressions: [x => x.Id]);
    }
}


public class MockLinkGenerator : LinkGenerator
{
    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
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
    public void Test1()
    {
        var builder = new CustomerLinkBuilder();
        var generator = builder.Build(new MockLinkGenerator());
        var links = generator.GenerateLinks(new(Guid.Parse("5a33c641-60dd-47da-be0e-74b5498f3bc1"), "John", "john.doe@customer.net", "0021943322", "32 Baker street"));
    }
}
