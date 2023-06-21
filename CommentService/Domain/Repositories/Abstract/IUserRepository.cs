using CommentService.Domain.Enteties;

namespace CommentService.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByNickNameAsync(string nickName);
        Task<User> GetUserByIdAsync(string id);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(string nickName);
    }
}
