using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.SqlServer.SPA;

public interface IClientProtection : IPIIProtection<DbClient>;
