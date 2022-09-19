using Locoom.Domain.Entities;

namespace Locoom.Application.Services.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
