using CommentService.Domain.Enteties;
using CommentService.Models;

namespace CommentService.Domain.Repositories.Abstract
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
