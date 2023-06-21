using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class UpdateCommentModel
    {
        [Required]
        public string CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
