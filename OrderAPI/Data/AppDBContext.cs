using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

    }
}
