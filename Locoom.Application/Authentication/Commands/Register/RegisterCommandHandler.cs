using ErrorOr;
using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Persistence;
using Locoom.Application.Authentication.Common;
using Locoom.Domain.Entities;
using Locoom.Domain.Common.Errors;
using MediatR;

namespace Locoom.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (_userRepository.GetUserByEmail(command.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            // Create user ( generate unique Id) & persist do DB

            var user = new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password
            };

            _userRepository.Add(user);

            // Create Jwt Token

            var token = _jwtTokenGenerator.GeneratorToken(user);

            return new AuthenticationResult(
                user,
                token
            );
        }
    }
}