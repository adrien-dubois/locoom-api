using Locoom.Application.Common.Interfaces.Persistence;
using Locoom.Domain.Entities;

namespace Locoom.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new();
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _users.Add(user);
            _context.Add<User>(user);
            _context.SaveChanges();
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(user => user.Email == email);
        }
    }
}
