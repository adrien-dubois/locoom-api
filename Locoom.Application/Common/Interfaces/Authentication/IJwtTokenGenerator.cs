namespace Locoom.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenratorToken(Guid userId, string firstName, string lastName);

    }
}
