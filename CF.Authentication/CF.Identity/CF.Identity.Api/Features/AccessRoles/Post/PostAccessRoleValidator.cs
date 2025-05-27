using FluentValidation;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public class PostAccessRoleValidator : AbstractValidator<PostAccessRoleCommand>
{
    public PostAccessRoleValidator()
    {
        RuleFor(x => x.AccessRole.Id)
            .Empty()
            .WithMessage("Id must be empty for a new access role.");
    }
}
