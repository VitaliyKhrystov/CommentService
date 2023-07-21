using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace CommentService.Models
{
    public class ResetPassword
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int ConfirmationNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
