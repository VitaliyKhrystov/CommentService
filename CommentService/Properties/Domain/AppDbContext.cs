using CommentService.Properties.Domain.Enteties;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Properties.Domain
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role() { Id = new Guid().ToString(), Name = "Admin" },
                new Role() { Id = new Guid().ToString(), Name = "Moderator" },
                new Role() { Id = new Guid().ToString(), Name = "User" }
            });
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User() { Id = new Guid().ToString(), NickName = "Admin", Email = "admin@ukr.net", BirthYear= 1990, Password = "admin2023", Role = Roles.Single(r => r.Name == "Admin") },
                new User() { Id = new Guid().ToString(), NickName = "Moderator", Email = "moderator@ukr.net", BirthYear= 2000, Password = "moderator2023", Role = Roles.Single(r => r.Name == "Moderator") }
            });
        }
    }
}
