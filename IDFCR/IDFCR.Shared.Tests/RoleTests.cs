using IDFCR.Shared.Abstractions.Roles;

namespace IDFCR.Shared.Tests;

[RoleRequirement(nameof(Action))]
internal class MyRoleRequiredClass
{
    public static readonly Func<IEnumerable<string>> Action = () => ["action1", "action2"];
}

[TestFixture]
internal class RoleTests
{
    [Test]
    public void GetRoleRequirement_ReturnsCorrectRoleRequirement()
    {
        var roleRequirement = RoleRequirementAttributeReader.GetRoleRequirement<MyRoleRequiredClass>();
        Assert.That(roleRequirement, Is.Not.Null);
        Assert.That(roleRequirement.RoleRequirementType, Is.EqualTo(RoleRequirementType.Some));
        var items = roleRequirement.Roles;
        Assert.That(items, Contains.Item("action1"));
        Assert.That(items, Contains.Item("action2"));
    }
}
