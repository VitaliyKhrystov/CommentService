using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Domain.Repositories
{
    public class UserRepositoryEF : IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepositoryEF(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task CreateUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, default);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (await dbContext.Users.AnyAsync(u => u.Id == user.Id))
            {
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
