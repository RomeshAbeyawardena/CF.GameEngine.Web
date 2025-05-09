using FluentValidation;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;
using NSubstitute;

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

        sut_0.Handle(new TestEmptyUnitRequest(), new ValidationException(""), new MediatR.Pipeline.RequestExceptionHandlerState<IUnitResult>(), CancellationToken.None);

        var sut0 = new UnitExceptionHandler<TestUnitRequest, IUnitResult<Customer>, ValidationException>();

        sut0.Handle(new TestUnitRequest(), new ValidationException(""), new MediatR.Pipeline.RequestExceptionHandlerState<IUnitResult<Customer>>(), CancellationToken.None);

        var sut = new UnitExceptionHandler<TestCollectionRequest, IUnitResultCollection<Customer>, ValidationException>();

        sut.Handle(new TestCollectionRequest(), new ValidationException(""), new MediatR.Pipeline.RequestExceptionHandlerState<IUnitResultCollection<Customer>>(), CancellationToken.None);

        var sut1 = new UnitExceptionHandler<TestPagedRequest, IUnitPagedResult<Customer>, ValidationException>();

        sut1.Handle(new TestPagedRequest(), new ValidationException(""), new MediatR.Pipeline.RequestExceptionHandlerState<IUnitPagedResult<Customer>>(), CancellationToken.None);

        Assert.Pass();
    }
}
