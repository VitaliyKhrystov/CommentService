using CommentService.Domain.Enteties;

namespace CommentService.Models.UserModels
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; }
        public string RoleName { get; set; }
    }
}
