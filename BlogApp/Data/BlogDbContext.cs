using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<BlogCategory> BlogCategories { get; set; }
    }
}
