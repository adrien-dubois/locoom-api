using ErrorOr;

namespace Locoom.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(
                code: "User.DuplicateEmail",
                description: "Cet e-mail est déjà enregistré sur un autre compte.");

            public static Error NotId => Error.NotFound(
                code: "User.NotId",
                description: "Utilisateur inconnu");
        }
    }
}
