using IDFCR.Utility.Shared.Extensions;
using Microsoft.Extensions.Primitives;

namespace IDFCR.Utility.Shared.Tests;

public class Tests
{
    private static void AssertDictionary(IDictionary<string, StringValues> dictionary, string expectedKey, StringValues expectedValue)
    {
        Assert.Multiple(() =>
        {
            Assert.That(dictionary.ContainsKey(expectedKey), Is.True, $"Dictionary does not contain key: {expectedKey}");
            Assert.That(dictionary[expectedKey], Is.EqualTo(expectedValue), $"Value for key '{expectedKey}' does not match expected value.");
        });
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var args = ArgumentParser.GetArguments(["-single_arg", "value", "---single-argument", "argument value", "--flag", "--another-flag"]);
        AssertDictionary(args, "single_arg", "value");
        AssertDictionary(args, "single-argument", "argument value");
        AssertDictionary(args, "flag", bool.TrueString);
        AssertDictionary(args, "another-flag", bool.TrueString);

        args = ArgumentParser.GetArguments(["-single_arg", "value", "---single-argument", "argument value", "--another-argument", "my other value"]);
        AssertDictionary(args, "single_arg", "value");
        AssertDictionary(args, "single-argument", "argument value");
        AssertDictionary(args, "another-argument", "my other value");

        args = ArgumentParser.GetArguments(["-single_arg", "--value", "--single-argument", "--argument_value", "--another-argument", "--test"]);
        AssertDictionary(args, "single_arg", bool.TrueString);
        AssertDictionary(args, "single-argument", bool.TrueString);
        AssertDictionary(args, "argument_value", bool.TrueString);

        args = ArgumentParser.GetArguments(["--tag", "blue", "--tag", "red", "--tag", "green", "--tag", "yellow", "--tag", "orange"]);
        AssertDictionary(args, "tag", new StringValues(["blue","red", "green", "yellow", "orange"]));
    }
}
