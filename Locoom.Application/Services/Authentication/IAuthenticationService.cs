using OneOf;

namespace Locoom.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        OneOf<AuthenticationResult, > Register(
            string firstName,
            string lastName,
            string email,
            string password
        );

        OneOf<AuthenticationResult> Login(
            string email,
            string password
        );
    }
}
