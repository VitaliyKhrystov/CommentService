using CommentService.Models;
using CommentService.Properties.Domain.Enteties;

namespace CommentService.Properties.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByIdAsync(string userId);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(string userId);
    }
}
