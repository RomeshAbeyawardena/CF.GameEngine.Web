using FluentValidation;

namespace CF.Identity.Api.Features.User.Post;

public class PostUserCommandValidator : AbstractValidator<PostUserCommand>
{
    public PostUserCommandValidator()
    {
        RuleFor(x => x.User.Id).Equal(Guid.Empty).WithMessage("User ID must be empty.");
    }
}
