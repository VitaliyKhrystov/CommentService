using CommentService.Properties.Domain.Enteties;

namespace CommentService.Models
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string BirthYear { get; set; }
    }
}
