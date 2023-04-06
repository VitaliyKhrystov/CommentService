using CommentService.Models;
using CommentService.Properties.Domain.Enteties;

namespace CommentService.Properties.Domain.Repositories.Abstract
{
    public interface IRoleRepository
    {
        Task CreateRole(RoleRequestDTO user);
        void UpdateRole(RoleRequestDTO user);
        Task<Role> GetRoleById(string userId);
        Task<List<Role>> GetAllRoles();
        Task DeleteRole(string userId);
    }
}
