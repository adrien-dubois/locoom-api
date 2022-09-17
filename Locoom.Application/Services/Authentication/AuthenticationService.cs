using Locoom.Application.Common.Interfaces.Authentication;

namespace Locoom.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public AuthenticationResult Register(
            string firstName,
            string lastName,
            string email,
            string password
        )
        {

            // Check if user already exists

            // Create user ( generate unique Id)

            // Create Jwt Token

            var token = _jwtTokenGenerator.GenratorToken(userId, firstName, lastName);

            Guid userId = Guid.NewGuid();
            return new AuthenticationResult(
                userId,
                firstName,
                lastName,   
                email,
                token
            );
        }

        public AuthenticationResult Login(
            string email,
            string password
        )
        {
            return new AuthenticationResult(
                Guid.NewGuid(),
                "firstName",
                "lastName",
                email,
                "token"
            );
        }

    }
}
