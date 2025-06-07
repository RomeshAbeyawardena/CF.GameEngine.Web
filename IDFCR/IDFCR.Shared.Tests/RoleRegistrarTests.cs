using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;

namespace IDFCR.Shared.Tests;

internal class LocalRoles : RoleRegistrarBase
{
    public LocalRoles()
    {
        Prefix = "local_";
        TryRegisterRole("read", RoleCategory.Read, b => { });
        TryRegisterRole("admin", RoleCategory.Write, b => { });
    }
}

internal class GlobalRoles : RoleRegistrarBase
{
    public GlobalRoles()
    {
        Prefix = "global_";
        TryRegisterRole("read", RoleCategory.Read, b => { });
        TryRegisterRole("write", RoleCategory.Write, b => { });
    }
}


[TestFixture]
internal class RoleRegistrarTests
{
    [SetUp]
    public void ClearRegistrars()
    {
        RoleRegistrar.globalRegistrars.Value.Clear();
    }


    [Test]
    public void List_ShouldReturnRoles_FromRegistrarAndGlobal()
    {
        // Arrange
        RoleRegistrar.RegisterGlobal<GlobalRoles>();

        // Act
        var result = RoleRegistrar.List<LocalRoles>().ToList();

        // Assert
        Assert.That(result, Does.Contain("global_read"));
        Assert.That(result, Does.Contain("local_admin"));
    }

    [Test]
    public void RegisterGlobal_ShouldNotDuplicate_WhenSameRegistrarIsRegisteredTwice()
    {
        // Arrange
        RoleRegistrar.RegisterGlobal<GlobalRoles>();
        RoleRegistrar.RegisterGlobal<GlobalRoles>(); // Try again

        // Act
        var count = RoleRegistrar.GlobalRegistrars.Count(r => r is GlobalRoles);

        // Assert
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void List_WithCategory_ShouldReturnOnlyMatchingRoles()
    {
        // Act
        var result = RoleRegistrar.List<LocalRoles>(RoleCategory.Read).ToList();

        // Assert
        Assert.That(result, Has.Exactly(1).EqualTo("local_read"));
    }

}
