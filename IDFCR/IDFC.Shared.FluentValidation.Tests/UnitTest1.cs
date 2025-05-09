using FluentValidation;
using FluentValidation.Results;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using MediatR.Pipeline;

namespace IDFC.Shared.FluentValidation.Tests;

public class Tests
{
    internal record Customer();
    internal record TestEmptyUnitRequest : IUnitRequest;
    internal record TestUnitRequest : IUnitRequest<Customer>;
    internal record TestCollectionRequest : IUnitRequestCollection<Customer>;
    internal record TestPagedRequest : IUnitPagedRequest<Customer>;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var sut_0 = new UnitExceptionHandler<TestEmptyUnitRequest,
            IUnitResult, ValidationException>();

        var stateMachine_0 = new RequestExceptionHandlerState<IUnitResult>();

        sut_0.Handle(new TestEmptyUnitRequest(), new ValidationException(""), stateMachine_0, CancellationToken.None);

        Assert.That(stateMachine_0.Handled, Is.True);

        var sut0 = new UnitExceptionHandler<TestUnitRequest, IUnitResult<Customer>, ValidationException>();

        var stateMachine0 = new RequestExceptionHandlerState<IUnitResult<Customer>>();

        sut0.Handle(new TestUnitRequest(), new ValidationException(""), stateMachine0, CancellationToken.None);

        Assert.That(stateMachine0.Handled, Is.True);

        var sut = new UnitExceptionHandler<TestCollectionRequest, IUnitResultCollection<Customer>, ValidationException>();

        var stateMachine = new RequestExceptionHandlerState<IUnitResultCollection<Customer>>();

        sut.Handle(new TestCollectionRequest(), new ValidationException(""), stateMachine, CancellationToken.None);

        Assert.That(stateMachine.Handled, Is.True);

        var sut1 = new UnitExceptionHandler<TestPagedRequest, IUnitPagedResult<Customer>, ValidationException>();

        var stateMachine1 = new RequestExceptionHandlerState<IUnitPagedResult<Customer>>();

        sut1.Handle(new TestPagedRequest(), new ValidationException(""), stateMachine1, CancellationToken.None);

        Assert.That(stateMachine1.Handled, Is.True);
    }

    [Test]
    public void ValidationErrors_AreAttached_AsMeta()
    {
        // Arrange
        var failures = new List<ValidationFailure>
    {
        new("FirstName", "First name is required."),
        new("Age", "Age must be over 18.")
    };

        var validationException = new ValidationException(failures);

        var sut = new UnitExceptionHandler<TestUnitRequest, IUnitResult<Customer>, ValidationException>();
        var state = new RequestExceptionHandlerState<IUnitResult<Customer>>();

        // Act
        sut.Handle(new TestUnitRequest(), validationException, state, CancellationToken.None);

        // Assert
        Assert.That(state.Handled, Is.True);

        var result = state.Response as IUnitResult;
        Assert.That(result, Is.Not.Null);

        Assert.That(result.ContainsKey("FirstName"), Is.True);
        Assert.That(result["FirstName"], Is.EqualTo("First name is required."));

        Assert.That(result.ContainsKey("Age"), Is.True);
        Assert.That(result["Age"], Is.EqualTo("Age must be over 18."));
    }

}
