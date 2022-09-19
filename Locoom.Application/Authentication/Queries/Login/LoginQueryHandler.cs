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
    public class LoginCommandHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            if (_userRepository.GetUserByEmail(query.Email) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }

            if (user.Password != query.Password)
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