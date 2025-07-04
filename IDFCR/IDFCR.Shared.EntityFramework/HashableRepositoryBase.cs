﻿using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Filters;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Hashers;
using Microsoft.EntityFrameworkCore;

namespace IDFCR.Shared.EntityFramework;

/// <summary>
/// Represnts a repository that's entity is hashable.
/// </summary>
/// <typeparam name="TAbstraction"></typeparam>
/// <typeparam name="TDb"></typeparam>
/// <typeparam name="T"></typeparam>
/// <param name="context"></param>
public abstract class HashableRepositoryBase<TDbContext, TAbstraction, TDb, T>(TimeProvider timeProvider, TDbContext context, IUniqueContentHasher uniqueContentHash) 
    : RepositoryBase<TDbContext, TAbstraction, TDb, T>(timeProvider, context)
    where T : class, IMappable<TAbstraction>, IIdentifer, IVerifiable
    where TDb : class, IMappable<TAbstraction>, IIdentifer, IVerifiable
    where TDbContext : DbContext
{
    protected override void BeforeUpdate(TDb db, T source)
    {
        if (db is IVerifiable verifiableDb && source is IVerifiable verifiableSource)
        {
            if (!verifiableDb.IsHashValid(verifiableSource))
            {
                throw new EntityHashMismatchException(typeof(TDb));
            }
        }
    }

    protected override bool IsHandled(Exception exception)
    {
        return exception is EntityHashMismatchException;
    }

    protected override void OnAdd(TDb db, T source)
    {
        base.OnAdd(db, source);
        db.UpdateHash(uniqueContentHash.CreateHash);
    }

    protected override void OnUpdate(TDb db, T source)
    {
        base.OnUpdate(db, source);
        db.UpdateHash(uniqueContentHash.CreateHash);
    }
}
