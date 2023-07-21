using System.ComponentModel.DataAnnotations;

namespace CommentService.Models.ForgotPassport
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
