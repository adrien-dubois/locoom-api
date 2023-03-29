using Locoom.Domain.Entities;

namespace Locoom.Application.Authentication.Common
{
    public record AuthenticationResult(User User, string Token);
}
