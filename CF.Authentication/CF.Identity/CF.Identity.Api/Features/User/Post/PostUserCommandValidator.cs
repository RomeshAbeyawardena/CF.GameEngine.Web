using CF.Identity.Api.Endpoints.Users.Post;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Get;
using FluentValidation;
using IDFCR.Shared.Extensions;
using MediatR;
using System.Xml;

namespace CF.Identity.Api.Features.User.Post;

public class PostUserCommandValidator : AbstractValidator<PostUserCommand>
{
    public PostUserCommandValidator()
    {
        RuleFor(x => x.User.Id).Equal(Guid.Empty).WithMessage("User ID must be empty.");
    }
}
