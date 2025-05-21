using CF.Identity.Api.Endpoints.Users.Post;
using CF.Identity.Api.Features.Clients.Get;
using FluentValidation;
using IDFCR.Shared.Extensions;
using MediatR;

namespace CF.Identity.Api.Features.User.Post;

public class PostUserCommandValidator : AbstractValidator<PostUserCommand>
{
    private readonly IMediator _mediator;
    public PostUserCommandValidator(IMediator mediator)
    {
        _mediator = mediator;
        
        RuleFor(x => x.User.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.User.EmailAddress).NotEmpty().WithMessage("Email address is required.");
        RuleFor(x => x.User.HashedPassword).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.User.Firstname).NotEmpty().WithMessage("Firstname is required.");
        RuleFor(x => x.User.Lastname).NotEmpty().WithMessage("Lastname is required.");
        RuleFor(x => x.User.PrimaryTelephoneNumber).NotEmpty().WithMessage("Primary telephone number is required.");
        RuleFor(x => x).MustAsync(HaveValidClientAsync);
    }

    public async Task<bool> HaveValidClientAsync(PostUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (user.ClientId != Guid.Empty)
        {
            var client = (await _mediator.Send(new FindClientByIdQuery(user.ClientId), cancellationToken)).GetResultOrDefault();
            return client is not null;
        }

        if (string.IsNullOrWhiteSpace(user.Client))
        {
            return false;
        }

        var result = (await _mediator.Send(new FindClientQuery(user.Client), cancellationToken)).GetOneOrDefault();

        if(result is null)
        {
            return false;
        }

        user.ClientId = result.Id;
        return true;
    }
}
