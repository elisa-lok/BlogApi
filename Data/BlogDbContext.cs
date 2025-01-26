using Microsoft.EntityFrameworkCore;
using BlogApi.Models;
using BlogApi.Data.Configurations;

namespace BlogApi.Data
{
      public class BlogDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BlogDbContext(DbContextOptions<BlogDbContext> options, IConfiguration configuration) 
        : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<RequestLog> RequestLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = _configuration.GetSection("Database:ConnectionStrings:DefaultConnection").Value;

            //var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlite(connectionString);
            }
            else
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }
        }
    }
}