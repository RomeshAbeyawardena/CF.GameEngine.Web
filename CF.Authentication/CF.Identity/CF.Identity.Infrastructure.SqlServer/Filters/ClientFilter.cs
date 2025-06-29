﻿using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Extensions;
using LinqKit;

namespace CF.Identity.Infrastructure.SqlServer.Filters;

internal class ClientFilter(IClientFilter filter) : MappableFilterBase<IClientFilter, DbClient>(filter), IClientFilter
{
    protected override IClientFilter Source => this;
    public string? Key { get; set; }
    public bool ShowAll { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }

    public override void Map(IClientFilter source)
    {
        Key = source.Key;
        ShowAll = source.ShowAll;
        ValidFrom = source.ValidFrom;
        ValidTo = source.ValidTo;
    }

    public override ExpressionStarter<DbClient> ApplyFilter(ExpressionStarter<DbClient> query, IClientFilter filter)
    {
        if (!ShowAll)
        {
            query = query.And(x => !x.SuspendedTimestampUtc.HasValue);
        }

        if (!string.IsNullOrWhiteSpace(filter.Key))
        {
            query = query.And(x => x.Reference == filter.Key || x.Name == filter.Key);
        }

        return query.FilterValidity(filter);
    }
}
