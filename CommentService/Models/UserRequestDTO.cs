using CommentService.Domain.Enteties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class UserRequestDTO
    {
        [Required]
        public string NickName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [ValidateYear(1923, 2123)]
        public string BirthYear { get; set; }
    }
}
