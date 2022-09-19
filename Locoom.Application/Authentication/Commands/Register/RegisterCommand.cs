using ErrorOr;
using Locoom.Application.Authentication.Common;
using MediatR;

namespace Locoom.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest<ErrorOr<AuthenticationResult>>;
}
