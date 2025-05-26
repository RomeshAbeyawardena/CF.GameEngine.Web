using IDFCR.Shared.Abstractions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class CasingImpressionTests
{
    [TestCase("AlbertBohemianFedoraSAMsun")]
    [TestCase("admin@identity.co")]
    [TestCase("Admin@identity.co")]
    [TestCase("admin@Identity.co")]
    [TestCase("Admin@Identity.co")]
    public void CasingImpression_stores_impression_and_applies_it_correctly(string expected)
    {
        var impression = CasingImpression.Calculate(expected);

        var result = CasingImpression.Restore(expected.ToUpperInvariant(), impression);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Test_with_extremely_long_string()
    {
        var tooLongToSupportCasingRestoration = "ThisIsFarTooLongAndTooComplicatedToCalculateAndStoreWhyEvenBotherWhoTheHellHasTimeToRememberThatWellIdontYouObviouslyhaveFarTooMuchTimeOnYourhandsToHaveAUsernameThislongAndItsTakingMeForeverToTypeInHereAmIDoneYetItsTakingTooLongToReachtwo-hundred-and-fifty-six-characters";
        
        Assert.Throws<NotSupportedException>(() => CasingImpression.Calculate(tooLongToSupportCasingRestoration));

        tooLongToSupportCasingRestoration = new string('A', 256);

        Assert.DoesNotThrow(() => CasingImpression.Calculate(tooLongToSupportCasingRestoration));
    }
}
