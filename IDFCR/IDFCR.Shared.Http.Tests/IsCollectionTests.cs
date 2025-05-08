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

        Assert.That(myList.GetType().IsCollection(out var g), Is.True);
        Assert.That(myOtherList.GetType().IsCollection(out g), Is.True);
        Assert.That(anotherList.GetType().IsCollection(out g), Is.True);
        Assert.That(anotherHashList.GetType().IsCollection(out g), Is.True);
    }
}
