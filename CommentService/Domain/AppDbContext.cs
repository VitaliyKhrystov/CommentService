using CommentService.Domain.Enteties;
using CommentService.Services.EncryptDecryptData;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Domain
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> logger;
        private readonly IEncryptDecryptData encryptDecryptData;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger, IEncryptDecryptData encryptDecryptData) : base(options)
        {
            this.logger = logger;
            this.encryptDecryptData = encryptDecryptData;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var adminRole = new Role() { Id = Guid.NewGuid().ToString(), RoleName = Models.Roles.Admin };
            logger.LogInformation("RoleId: " + adminRole.Id + "; Name: " + adminRole.RoleName);

            var moderatorRole = new Role() { Id = Guid.NewGuid().ToString().ToString(), RoleName = Models.Roles.Moderator};
            logger.LogInformation("RoleId: " + moderatorRole.Id + "; Name: " + moderatorRole.RoleName);

            var userRole = new Role() { Id = Guid.NewGuid().ToString(), RoleName = Models.Roles.User };
            logger.LogInformation("RoleId: " + userRole.Id + "; Name: " + userRole.RoleName);


            modelBuilder.Entity<Role>().HasData(new List<Role>() { adminRole, moderatorRole, userRole });

            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User() { Id = Guid.NewGuid().ToString(), NickName = "Admin", Email = "admin@ukr.net", BirthYear= 1990, Password = encryptDecryptData.EncryptDataToBase64("admin2023"), RoleId = adminRole.Id },
                new User() { Id = Guid.NewGuid().ToString(), NickName = "Moderator", Email = "moderator@ukr.net", BirthYear= 2000, Password = encryptDecryptData.EncryptDataToBase64("moderator2023"), RoleId = moderatorRole.Id }
            });
        }
    }
}
