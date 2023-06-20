using CommentService.Domain.Enteties;
using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class CommentModel
    {
        public string? TopicURL { get; set; }
        public string? ParrentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string? CommentText { get; set; }
    }
}
