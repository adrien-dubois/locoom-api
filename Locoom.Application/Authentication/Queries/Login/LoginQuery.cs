using ErrorOr;
using Locoom.Application.Authentication.Common;
using MediatR;

namespace Locoom.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<ErrorOr<AuthenticationResult>> ;
}
