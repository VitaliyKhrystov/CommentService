using CommentService.Domain.Enteties;

namespace CommentService.Models.CommentModels
{
    public class CommentResponseModel
    {
        public string CommentId { get; set; }
        public string? ParrentId { get; set; }
        public string UserId { get; set; }
        public string? CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Like>? Likes { get; set; }
        public List<DisLike>? DisLikes { get; set; }
        public List<CommentResponseModel> Replies { get; set; }
    }
}
