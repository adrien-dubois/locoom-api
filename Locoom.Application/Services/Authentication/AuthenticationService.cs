using ErrorOr;
using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Persistence;
using Locoom.Domain.Common.Errors;
using Locoom.Domain.Entities;

namespace Locoom.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public ErrorOr<AuthenticationResult> Register(
            string firstName,
            string lastName,
            string email,
            string password
        )
        {

            // Check if user doesn't exists

            if(_userRepository.GetUserByEmail(email) is not null)
            {
                return Errors.User.DuplicateEmail; 
            }

            // Create user ( generate unique Id) & persist do DB

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            _userRepository.Add(user);

            // Create Jwt Token

            var token = _jwtTokenGenerator.GeneratorToken(user);

            return new AuthenticationResult(
                user,
                token
            );
        }

        public ErrorOr<AuthenticationResult> Login(
            string email,
            string password
        )
        {
            // 1. Validate user exists
            if(_userRepository.GetUserByEmail(email) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            // 2. Validate password is correct
            if(user.Password != password)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            // 3. Create Jwt Token
            var token = _jwtTokenGenerator.GeneratorToken(user);


            return new AuthenticationResult(
                user,
                token
            );
        }

    }
}
