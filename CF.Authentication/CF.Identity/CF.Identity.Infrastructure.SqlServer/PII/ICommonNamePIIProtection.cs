﻿using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.SqlServer.PII;

public interface ICommonNamePIIProtection : IPIIProtection<DbCommonName>;
