using ErrorOr;
using Locoom.Application.Authentication.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Locoom.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PasswordConfirmation) : IRequest<ErrorOr<AuthenticationResult>>;
}
