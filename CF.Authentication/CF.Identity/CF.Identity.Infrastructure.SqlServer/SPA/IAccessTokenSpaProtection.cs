using CF.Identity.Infrastructure.Features.AccessToken;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.SqlServer.SPA;

internal interface IAccessTokenSpaProtection : IPIIProtection<DbAccessToken>, IAccessTokenProtection;