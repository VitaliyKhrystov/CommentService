using CommentService.Domain.Enteties;
using CommentService.Models;

namespace CommentService.Domain.Repositories.Abstract
{
    public interface IRoleRepository
    {
        Task CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<List<Role>> GetAllRolesAsync();
        Task DeleteRoleAsync(string roleId);
    }
}
