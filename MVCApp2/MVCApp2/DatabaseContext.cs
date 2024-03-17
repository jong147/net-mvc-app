using Microsoft.EntityFrameworkCore;
using MVCApp2.Models;

namespace MVCApp2
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Clients> Clients { get; set; } = null!;
    }
}
