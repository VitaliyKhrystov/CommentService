using CommentService.Models;

namespace CommentService.Properties.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task CreateUser(UserRequestDTO user);
        void UpdateUser(UserRequestDTO user);
        Task<UserResponseDTO> GetUserById(string userId);
        Task<List<UserResponseDTO>> GetAllUsers();
        Task DeleteUser(string userId);
    }
}
