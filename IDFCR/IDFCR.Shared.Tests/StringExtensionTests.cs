using IDFCR.Shared.Extensions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class StringExtensionTests
{
    [TestCase("mary-jean", "Mary-Jean")]
    public void Test(string value, string expectedValue)
    {
        Assert.That(value.NormaliseName(), Is.EqualTo(expectedValue));
    }
}
