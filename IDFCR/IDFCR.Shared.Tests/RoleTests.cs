using IDFCR.Shared.Abstractions.Roles;

namespace IDFCR.Shared.Tests;

[RoleRequirement(nameof(InvalidAction))]
internal class MyRoleRequiredClassWithWrongFieldType
{
    public static readonly string InvalidAction = "not a delegate";
}

[RoleRequirement("SomeActionThatDontExist")]
internal class MyImproperlyConfiguredRoleRequiredClass { }


[RoleRequirement(nameof(Action))]
internal class MyRoleRequiredClass
{
    public static readonly Func<IEnumerable<string>> Action = () => ["action1", "action2"];
    public bool Bypass { get; set; }
}


[RoleRequirement(nameof(Action))]
internal class MyRoleRequiredClassWithoutBypass
{
    public static readonly Func<IEnumerable<string>> Action = () => ["action1", "action2"];
}

[TestFixture]
internal class RoleTests
{
    //☺️ path
    [Test]
    public void GetRoleRequirement_ReturnsCorrectRoleRequirement()
    {
        var roleRequirement = RoleRequirementAttributeReader.GetRoleRequirement(new MyRoleRequiredClass() 
            { Bypass = true }
        );
        Assert.That(roleRequirement, Is.Not.Null);
        Assert.That(roleRequirement.RoleRequirementType, Is.EqualTo(RoleRequirementType.Some));
        var items = roleRequirement.Roles;
        Assert.That(items, Contains.Item("action1"));
        Assert.That(items, Contains.Item("action2"));
        Assert.That(roleRequirement.Bypass, Is.True);
    }

    [Test]
    public void GetRoleRequirementWithoutBypass_ReturnsCorrectRoleRequirement()
    {
        var roleRequirement = RoleRequirementAttributeReader.GetRoleRequirement(new MyRoleRequiredClassWithoutBypass()
        );
        Assert.That(roleRequirement, Is.Not.Null);
        Assert.That(roleRequirement.RoleRequirementType, Is.EqualTo(RoleRequirementType.Some));
        var items = roleRequirement.Roles;
        Assert.That(items, Contains.Item("action1"));
        Assert.That(items, Contains.Item("action2"));
        Assert.That(roleRequirement.Bypass, Is.False);
    }
    //😟 path
    [Test]
    public void GetRoleRequirement_InvalidAction_Throws()
    {
        var ex = Assert.Throws<NullReferenceException>(() =>
        {
            _ = RoleRequirementAttributeReader.GetRoleRequirement(new MyImproperlyConfiguredRoleRequiredClass());
        });

        Assert.That(ex!.Message, Does.Contain("No public static field named 'SomeActionThatDontExist'"));
    }

    [Test]
    public void GetRoleRequirement_WrongFieldType_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            _ = RoleRequirementAttributeReader.GetRoleRequirement(new MyRoleRequiredClassWithWrongFieldType());
        });

        Assert.That(ex!.Message, Does.Contain("must be of type Func<IEnumerable<string>>"));
    }

}
