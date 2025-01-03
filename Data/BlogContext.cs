using Microsoft.EntityFrameworkCore;
using BlogApi.Models;

namespace BlogApi.Data
{
      public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}