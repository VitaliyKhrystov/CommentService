using CommentService.Domain.Enteties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommentService.Domain
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> logger;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
        {
            this.logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRole = new Role() { Id = Guid.NewGuid().ToString(), Name = "Admin" };
            logger.LogInformation("RoleId: " + adminRole.Id + "; Name: " + adminRole.Name);

            var moderatorRole = new Role() { Id = Guid.NewGuid().ToString().ToString(), Name = "Moderator" };
            logger.LogInformation("RoleId: " + moderatorRole.Id + "; Name: " + moderatorRole.Name);

            var userRole = new Role() { Id = Guid.NewGuid().ToString(), Name = "User" };
            logger.LogInformation("RoleId: " + userRole.Id + "; Name: " + userRole.Name);


            modelBuilder.Entity<Role>().HasData(new List<Role>() { adminRole, moderatorRole, userRole });

            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User() { Id = Guid.NewGuid().ToString(), NickName = "Admin", Email = "admin@ukr.net", BirthYear= 1990, Password = "admin2023", RoleId = adminRole.Id },
                new User() { Id = Guid.NewGuid().ToString(), NickName = "Moderator", Email = "moderator@ukr.net", BirthYear= 2000, Password = "moderator2023", RoleId = moderatorRole.Id }
            });
        }
    }
}
