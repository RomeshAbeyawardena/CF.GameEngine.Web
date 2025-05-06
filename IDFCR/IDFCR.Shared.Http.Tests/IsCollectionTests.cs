using IDFCR.Shared.Http.Extensions;
using System.Collections.ObjectModel;

namespace IDFCR.Shared.Http.Tests;

[TestFixture]
internal class IsCollectionTests
{
    [Test]
    public void Test()
    {
        var myList = new List<string>();
        var myOtherList = Array.Empty<string>();
        var anotherList = new Collection<string>();
        var anotherHashList = new HashSet<string>();

        Assert.That(myList.GetType().IsCollection(), Is.True);
        Assert.That(myOtherList.GetType().IsCollection(), Is.True);
        Assert.That(anotherList.GetType().IsCollection(), Is.True);
        Assert.That(anotherHashList.GetType().IsCollection(), Is.True);
    }
}
