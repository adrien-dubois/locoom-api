using ErrorOr;
using Locoom.Application.Services.Authentication.Common;

namespace Locoom.Application.Services.Authentication.Queries
{
    public interface IAuthenticationQueryService
    {
        ErrorOr<AuthenticationResult> Login(
            string email,
            string password
        );
    }
}
