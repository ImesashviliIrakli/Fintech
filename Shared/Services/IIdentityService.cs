﻿namespace Shared.Services;

public interface IIdentityService
{
    Task<int?> ValidateCompanyCredentialsAsync(string apiKey, string apiSecret);
}

