namespace IDFCR.Shared.Http.Tests;

[TestFixture]
internal class HypermediaResultTests
{
    [Test]
    public void T1()
    {
        var item = new Hypermedia<Customer>(new Customer(Guid.NewGuid(), Guid.NewGuid(), "John", "Doe", "394423943", "22 Sanderstreet"));

    }
}
