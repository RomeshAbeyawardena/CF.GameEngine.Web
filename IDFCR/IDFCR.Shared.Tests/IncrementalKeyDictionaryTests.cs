using IDFCR.Shared.Abstractions;

namespace IDFCR.Shared.Tests;

[TestFixture]
internal class IncrementalKeyDictionaryTests
{
    public static void AssertValues<T>(IIncrementalKeyDictionary<T> source, string key, T value)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(source.TryGetValue(key, out var actualValue), Is.True, "Key should exist in the dictionary.");
            Assert.That(actualValue, Is.EqualTo(value), "Value for the key should match the expected value.");
        }
    }

    [Test]
    public void DictionaryTest()
    {
        IncrementalKeyDictionary<object> sut = [];
        sut.Add("Moo", "Moo");
        sut.Add("Moo", "Moo1");
        sut.Add("Moo", "Moo2");
        sut.Add("Moo", "Moo3");

        AssertValues(sut, "Moo", "Moo");
        AssertValues(sut, "Moo_1", "Moo1");
        AssertValues(sut, "Moo_2", "Moo2");
        AssertValues(sut, "Moo_3", "Moo3");
    }
}
