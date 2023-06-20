using CommentService.Domain.Enteties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class RegisterModel
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
        [ValidateYear(1923, 2123)]
        public int BirthYear { get; set; }
        public Roles RoleName { get; set; }
    }
}
