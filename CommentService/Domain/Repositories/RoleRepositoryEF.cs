using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Domain.Repositories
{
    public class RoleRepositoryEF : IRoleRepository
    {
        public RoleRepositoryEF(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateRoleAsync(Role role)
        {
            await dbContext.Roles.AddAsync(role);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await GetRoleByIdAsync(roleId);
            if (role != null)
            {
                dbContext.Roles.Remove(role);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await dbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId, default);
        }

        public async Task UpdateRoleAsync(Role role)
        {
            if (await dbContext.Roles.AnyAsync(r => r.Id == role.Id))
            {
                dbContext.Roles.Update(role);
                await dbContext.SaveChangesAsync();
            }
        }

        private bool disposed = false;
        private readonly AppDbContext dbContext;

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
