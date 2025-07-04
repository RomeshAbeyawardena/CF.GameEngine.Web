﻿using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.SqlServer.SPA;

internal interface IClientSpaProtection : IPIIProtection<DbClient>, IClientProtection;
