using Locoom.Application.Common.Errors;
using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Persistence;
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

        public AuthenticationResult Register(
            string firstName,
            string lastName,
            string email,
            string password
        )
        {

            // Check if user doesn't exists

            if(_userRepository.GetUserByEmail(email) is not null)
            {
                throw new DuplicateEmailException();
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

        public AuthenticationResult Login(
            string email,
            string password
        )
        {
            // 1. Validate user exists
            if(_userRepository.GetUserByEmail(email) is not User user)
            {
                throw new Exception("Utilisateur et/ou mot de passe incorrect(s)");
            }

            // 2. Validate password is correct
            if(user.Password != password)
            {
                throw new Exception("Utilisateur et/ou mot de passe incorrect(s)");
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
