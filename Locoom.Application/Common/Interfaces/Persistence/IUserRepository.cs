using Locoom.Domain.Entities;

namespace Locoom.Application.Common.Interfaces.Persistence
{
     public interface IUserRepository
    {
        User? GetUserByEmail(string email);
        void Add(User user);
        IEnumerable<User> GetAll();
    }
}
