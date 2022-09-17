using Locoom.Domain.Entities;

namespace Locoom.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GeneratorToken(User user);

    }
}
