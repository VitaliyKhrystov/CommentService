using CommentService.Domain.Enteties;
using CommentService.Models;

namespace CommentService.Domain.Repositories.Abstract
{
    public interface IRoleRepository
    {
        Task CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task<Role> GetRoleByNameAsync(Roles role);
        Task<Role> GetRoleByIdAsync(string id);
        Task<List<Role>> GetAllRolesAsync();
        Task DeleteRoleAsync(Roles role);
    }
}
