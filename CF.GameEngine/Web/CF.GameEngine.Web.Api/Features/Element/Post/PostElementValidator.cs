using FluentValidation;

namespace CF.GameEngine.Web.Api.Features.Element.Post;

public class PostElementValidator : AbstractValidator<PostElementCommand>
{
    public PostElementValidator()
    {
        RuleFor(x => x.Element.Id)
            .Equal(Guid.Empty)
            .WithMessage("Element ID must be empty for creation.");
    }
}