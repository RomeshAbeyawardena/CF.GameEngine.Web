using IDFCR.Shared.Abstractions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class CasingImpressionTests
{
    [Test]
    public void CasingImpression_stores_impression_and_applies_it_correctly()
    {
        var myUniquelyCasedUsername = "AlbertBohemianFedoraSAMsun";

        var impression = CasingImpression.Calculate(myUniquelyCasedUsername);

        var result = CasingImpression.Restore(myUniquelyCasedUsername.ToUpperInvariant(), impression);
        Assert.That(result, Is.EqualTo(myUniquelyCasedUsername));
    }

    [Test]
    public void Test_with_extremely_long_string()
    {
        var tooLongToSupportCasingRestoration = "ThisIsFarTooLongAndTooComplicatedToCalculateAndStoreWhyEvenBotherWhoTheHellHasTimeToRememberThatWellIdontYouObviouslyhaveFarTooMuchTimeOnYourhandsToHaveAUsernameThislongAndItsTakingMeForeverToTypeInHereAmIDoneYetItsTakingTooLongToReachtwo-hundred-and-fifty-six-characters";
        Assert.Throws<NotSupportedException>(() => CasingImpression.Calculate(tooLongToSupportCasingRestoration));
    }
}
