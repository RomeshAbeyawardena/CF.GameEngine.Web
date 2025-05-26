using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Extensions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class PagingTests
{
    [Test]
    public void T()
    {
        var myPager = new PagedQuery(5, 1);

        var conventionalPager = myPager.ToConventional();

        Assert.That(conventionalPager, Is.Not.Null);
        Assert.That(conventionalPager.Take, Is.EqualTo(5));
        Assert.That(conventionalPager.Skip, Is.EqualTo(5));
    }
}
