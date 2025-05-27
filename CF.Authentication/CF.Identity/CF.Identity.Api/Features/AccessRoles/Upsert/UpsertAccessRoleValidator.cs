using CF.Identity.Api.Features.Clients.Get;
using FluentValidation;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR;

namespace CF.Identity.Api.Features.AccessRoles.Upsert
{
    public class UpsertAccessRoleValidator 
        : AbstractValidator<UpsertAccessRoleCommand>
    {
        private readonly IMediator _mediator;
        public UpsertAccessRoleValidator(IMediator mediator)
        {
            _mediator = mediator;
            RuleFor(x => x.AccessRole.Key)
                .NotEmpty()
                .WithMessage("Key is required.")
                .MaximumLength(50)
                .WithMessage("Key must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_-]+$")
                .WithMessage("Key can only contain alphanumeric characters, underscores, and hyphens.");

            RuleFor(x => x.AccessRole.DisplayName)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.AccessRole.DisplayName)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.AccessRole).MustAsync(HaveValidClient)
                .WithName("ClientId")
                .WithMessage("Client or Client ID must be a valid value")
                .WithErrorCode(Errorcodes.Conflict);
        }
        public async Task<bool> HaveValidClient(
            EditableAccessRoleDto model, CancellationToken cancellationToken)
        {
            if(model.ClientId != default)
            {
                var result = (await _mediator.Send(new FindClientQueryById(model.ClientId, true), cancellationToken)).GetResultOrDefault();

                return result is not null;
            }

            if (string.IsNullOrWhiteSpace(model.Client))
            {
                return false;
            }

            var clientResult = (await _mediator.Send(new FindClientQuery(model.Client, Bypass: true), cancellationToken)).GetOneOrDefault();

            if(clientResult is null)
            {
                return false;
            }

            model.ClientId = clientResult.Id;

            return true;
        }
    }
}
