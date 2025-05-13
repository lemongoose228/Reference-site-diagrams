using Microsoft.EntityFrameworkCore;

namespace DiagramCursVue.Data
{
    public class AppDbContext : DbContext {
        public DbSet<Article> Articles { get; set; }

        public AppDbContext() => Database.EnsureCreated();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }
    }
}
