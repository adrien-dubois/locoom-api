﻿using ErrorOr;
using Locoom.Application.Services.Authentication.Common;

namespace Locoom.Application.Services.Authentication.Commands
{
    public interface IAuthenticationCommandService
    {
        ErrorOr<AuthenticationResult> Register(
            string firstName,
            string lastName,
            string email,
            string password
        );
    }
}
