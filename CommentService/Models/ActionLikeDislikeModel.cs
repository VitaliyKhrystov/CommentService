using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class ActionLikeDislikeModel
    {
        [Required]
        public string CommentId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
