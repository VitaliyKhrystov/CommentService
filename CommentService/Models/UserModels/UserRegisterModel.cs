using CommentService.Domain.Enteties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentService.Models.UserModels
{
    public class UserRegisterModel
    {
        [Required]
        public string NickName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [ValidateYear()]
        public int BirthYear { get; set; }
        public Roles RoleName { get; set; }
    }
}
