﻿using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.AccessRoles;

public class AccessRoleDto : MappableBase<IAccessRole>, IAccessRole
{
    protected override IAccessRole Source => this;

    public string? Description { get; set; }
    public Guid ClientId { get; set; }
    public string Key { get; set; } = null!;
    public string? DisplayName { get; set; }
    public Guid Id { get; set; }

    public override void Map(IAccessRole source)
    {
        ClientId = source.ClientId;
        Key = source.Key;
        DisplayName = source.DisplayName;
        Description = source.Description;
        Id = source.Id;
    }
}
