using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.AccessRoles
{
    public class EditableAccessRoleDto : MappableBase<IAccessRole>, IAccessRole
    {
        protected override IAccessRole Source => this;
        public Guid ClientId { get; set; }
        public string? Client { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public Guid Id { get; set; }
        public override void Map(IAccessRole source)
        {
            ClientId = source.ClientId;
            Key = source.Key;
            DisplayName = source.DisplayName;
            Id = source.Id;
        }
    }
}
