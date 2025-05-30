using IDFCR.Shared.Exceptions;

namespace IDFCR.Shared.Tests;

public class EntityAlreadyExistsExceptionTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        var exception = new EntityAlreadyExistsException(entityType: "TestEntity", id: 1);
        Assert.That(exception.Message, Is.EqualTo("An entity of TestEntity already exists with '1'"));

        IExposableException exposableException = exception;
        Assert.That(exposableException.Message, Is.EqualTo("An entity of TestEntity already exists"));
        Assert.That(exposableException.Details, Is.EqualTo("Id: '1' in 'TestEntity'"));

        exception = new EntityAlreadyExistsException(entityType: "TestEntityDto", id: 1);
        Assert.That(exception.Message, Is.EqualTo("An entity of TestEntity already exists with '1'"));
    }
}
