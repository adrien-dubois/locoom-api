﻿using ErrorOr;

namespace Locoom.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {

            public static Error InvalidCredentials => Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "Utilisateur et/ou mot de passe incorrect(s)");
        }
    }
}
