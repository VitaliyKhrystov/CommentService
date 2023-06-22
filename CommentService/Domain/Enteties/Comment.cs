
namespace CommentService.Domain.Enteties
{
    public class Comment
    {
        public string? TopicURL { get; set; }
        public string CommentId { get; set; }
        public string? ParrentId { get; set; }
        public string UserId { get; set; }
        public string? CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Like>? Likes { get; set; }
        public List<DisLike>? DisLikes { get; set; }

        public Comment()
        {
            Likes = new List<Like>();
            DisLikes = new List<DisLike>();
        }
    }
}
