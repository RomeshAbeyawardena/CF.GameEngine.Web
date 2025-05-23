using FluentValidation;

namespace CF.Identity.Api.Features.Clients.Post;

public class PostClientValidator : AbstractValidator<PostClientCommand>
{
    public PostClientValidator()
    {
        RuleFor(x => x.Client.Name).NotEmpty()
            .WithMessage("Client name is required.")
            .MaximumLength(256)
            .WithMessage("Client name must not exceed 256 characters.");
        RuleFor(x => x.Client.DisplayName).MaximumLength(256);
        RuleFor(x => x.Client.ValidFrom).NotEmpty()
            .WithMessage("Valid from date is required.");
        RuleFor(x => x.Client.Reference).NotEmpty()
            .WithMessage("Client reference is required.")
            .MaximumLength(256)
            .WithMessage("Client reference must not exceed 256 characters.");

        RuleFor(x => x.Client).MustAsync(BeUnique)
            .WithName("Client_not_unique")
            .WithMessage("Client must have a unique name");
    }

    public async Task<bool> BeUnique(EditableClientDto client, CancellationToken cancellationToken)
    {
         client.Reference
    }
}
