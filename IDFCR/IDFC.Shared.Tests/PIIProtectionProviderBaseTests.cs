using IDFCR.Shared.Abstractions;
using System.Text;

namespace IDFC.Shared.Tests;

[TestFixture]
internal class PIIProtectionProviderBaseTests
{
    [Test]
    public void Test()
    {
        var b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, "MyVeryLongApplicationSecret", "MyVeryLongClientApplicationSet");
        Assert.That(b, Has.Length.EqualTo(32));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, "shortkey", "shtrky");
        Assert.That(b, Has.Length.EqualTo(32));

        b = PIIProtectionProviderBase.GenerateKey(32, ';', Encoding.UTF8, "Exactly 16 chars", "Exactly 16 chars");
        Assert.That(b, Has.Length.EqualTo(32));
    }
}
