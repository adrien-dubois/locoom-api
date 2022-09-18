using ErrorOr;

namespace Locoom.Domain.Common.Errors
{
    public static class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(
                code: "User.DuplicateEmail",
                description: "Cet e-mail est déjà enregistré sur un autre compte.");

            public static Error InvalidCredentials => Error.Conflict(
                code: "User.InvalidCredentials",
                description: "Utilisateur et/ou mot de passe incorrect(s)");
        }
    }
}
