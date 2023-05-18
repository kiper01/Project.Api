using Microsoft.EntityFrameworkCore;
using Project.Core.Entities;

namespace Project.DAL
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<UserToTeams> UserToTeams { get; set; }
        public DbSet<Programs> Programs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(System.Console.WriteLine);
        }
    }
}
