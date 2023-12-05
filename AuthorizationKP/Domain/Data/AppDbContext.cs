using AuthorizationKP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationKP.Domain.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Users> Users { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }

        
    }
}
