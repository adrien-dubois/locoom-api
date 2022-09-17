using Locoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locoom.Infrastructure.Persistance
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
