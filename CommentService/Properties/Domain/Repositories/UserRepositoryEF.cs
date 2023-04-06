using CommentService.Models;
using CommentService.Properties.Domain.Repositories.Abstract;

namespace CommentService.Properties.Domain.Repositories
{
    public class UserRepositoryEF : IUserRepository
    {
        public Task CreateUser(UserRequestDTO user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserResponseDTO>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDTO> GetUserById(string userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserRequestDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
