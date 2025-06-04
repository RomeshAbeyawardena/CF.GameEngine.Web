using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Links;
using IDFCR.Shared.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;

namespace IDFCR.Shared.Http.Tests;

internal class TestHttpResponse(HttpContext context) : HttpResponse, IDisposable
{
    private readonly MemoryStream _stringWriter = new();
    public override HttpContext HttpContext => context;
    public override int StatusCode { get; set; }
    public override IHeaderDictionary Headers => new HeaderDictionary();
    public override Stream Body { get => _stringWriter; set => throw new NotSupportedException(); }
    public override long? ContentLength { get; set; }
    public override string? ContentType { get; set; }
    public override IResponseCookies Cookies { get; }
    public override bool HasStarted { get; }

    public void Dispose()
    {
        _stringWriter.Dispose();
    }

    public override string ToString()
    {
        return Encoding.UTF8.GetString(_stringWriter.ToArray());
    }

    public override void OnCompleted(Func<object, Task> callback, object state)
    {
        throw new NotImplementedException();
    }

    public override void OnStarting(Func<object, Task> callback, object state)
    {
        throw new NotImplementedException();
    }

    public override void Redirect([StringSyntax("Uri")] string location, bool permanent)
    {
        throw new NotImplementedException();
    }
}

internal class TestHttpContext : HttpContext, IDisposable
{
    private readonly IServiceProvider serviceProvider;
    private readonly TestHttpResponse testHttpResponse;
    
    public TestHttpContext(IServiceProvider services)
    {
        serviceProvider = services;
        testHttpResponse = new TestHttpResponse(this);
        Items = new Dictionary<object, object?>();
    }

    // Implement necessary members of HttpContext for testing purposes
    public override IFeatureCollection Features { get; }
    public override HttpRequest Request { get; }
    public override HttpResponse Response => testHttpResponse;
    public override ConnectionInfo Connection { get; }
    public override WebSocketManager WebSockets { get; }
    public override ClaimsPrincipal User { get; set; }
    public override IDictionary<object, object?> Items { get; set; }
    public override IServiceProvider RequestServices { get => serviceProvider; set => throw new NotSupportedException(); }
    public override CancellationToken RequestAborted { get; set; }
    public override string TraceIdentifier { get; set; }
    public override ISession Session { get; set; }

    public override void Abort()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        testHttpResponse.Dispose();
    }
}

public class TestLinkGenerator : LinkGenerator
{
    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        return "a path";
    }

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return "a path";
    }

    public override string? GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        return "a path";
    }

    public override string? GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return "a path";
    }
}


[TestFixture]
internal class EntryWrapperTests
{
    [Test]
    public async Task Test()
    {
        var sut = new Customer(Guid.NewGuid(), Guid.NewGuid(), "John", "jdoe@gmail.com", "011234", "34232 ASDA MANE");
        var apiCollectionResult = new ApiCollectionResult<Customer>(
            [sut], 200, true);

        var serviceCollection = new ServiceCollection()
            .AddSingleton(TimeProvider.System)
            .AddSingleton<ILinkBuilder<Customer>,CustomerLinkBuilder>()
            .AddSingleton<LinkGenerator>(new TestLinkGenerator());

        using var testHttpContext = new TestHttpContext(serviceCollection.BuildServiceProvider());

        await apiCollectionResult.ExecuteAsync(testHttpContext);
    }
}
