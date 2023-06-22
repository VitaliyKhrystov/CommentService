namespace CommentService.Domain.Enteties
{
    public class ActionLikeDislike
    {
        public string Id { get; set; }
        public string CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public class Like:ActionLikeDislike { }
    public class DisLike:ActionLikeDislike { }
}
