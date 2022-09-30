using ErrorOr;
using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Persistence;
using Locoom.Application.Authentication.Common;
using Locoom.Domain.Entities;
using Locoom.Domain.Common.Errors;
using MediatR;
using Locoom.Application.Authentication.Queries.Login;

namespace Locoom.Application.Authentication.Commands.Register
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (_userRepository.GetUserByEmail(query.Email) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            // Verify the hash password with BCrypt
            if(!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            {
                return Errors.Authentication.InvalidCredentials;
            }

            var token = _jwtTokenGenerator.GeneratorToken(user);


            return new AuthenticationResult(
                user,
                token
            );
        }
    }
}

// 12:15