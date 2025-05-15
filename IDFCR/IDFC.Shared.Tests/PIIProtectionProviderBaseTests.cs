using IDFCR.Shared.Abstractions;
using System.Text;

namespace IDFC.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void Test()
    {
        var b = PIIProtectionProviderBase.KeyBuilder(32, ';', Encoding.UTF8, "MyVeryLongApplicationSecret", "MyVeryLongClientApplicationSet");
        Assert.That(b.Length, Is.EqualTo(32));

        b = PIIProtectionProviderBase.KeyBuilder(32, ';', Encoding.UTF8, "shortkey", "shtrky");
        Assert.That(b.Length, Is.EqualTo(32));

        b = PIIProtectionProviderBase.KeyBuilder(32, ';', Encoding.UTF8, "Exactly 16 chars", "Exactly 16 chars");
        Assert.That(b.Length, Is.EqualTo(32));
    }
}
