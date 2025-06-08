using IDFCR.Shared.Extensions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class StringExtensionTests
{
    [TestCase("mary-jean", "Mary-Jean")]
    public void NormaliseName_as_expected(string value, string expectedValue)
    {
        Assert.That(value.NormaliseName(), Is.EqualTo(expectedValue));
    }

    [TestCase("MyHTMLParser", "myHTMLParser")]
    [TestCase("SomeClass", "someClass")]
    [TestCase("SomeOtherClass", "someOtherClass")]
    [TestCase("MyAJAXResult", "myAJAXResult")]
    [TestCase("AJAXResult", "ajaxResult")]
    public void ToCamelCasePreservingAcronyms_as_expected(string value, string expectedValue)
    {
        Assert.That(value.ToCamelCasePreservingAcronyms(), Is.EqualTo(expectedValue));
    }
}
