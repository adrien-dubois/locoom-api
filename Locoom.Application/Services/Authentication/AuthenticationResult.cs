using Locoom.Domain.Entities;

namespace Locoom.Application.Services.Authentication
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
