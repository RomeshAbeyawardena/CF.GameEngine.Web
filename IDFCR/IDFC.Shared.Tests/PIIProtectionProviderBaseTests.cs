using IDFCR.Shared.Abstractions;
using System.Text;

namespace IDFC.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void Test()
    {
        var b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "MyVeryLongApplicationSecret", "MyVeryLongClientApplicationSet");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));

        var c = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, b.Item1, "shortkey", "shtrky");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
        Assert.That(b.Item2, Is.EqualTo(c.Item2));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, null, "Exactly 16 chars", "Exactly 16 chars");
        Assert.That(b.Item2, Has.Length.EqualTo(32));
    }
}
