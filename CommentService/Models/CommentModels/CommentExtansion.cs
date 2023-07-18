using CommentService.Domain.Enteties;

namespace CommentService.Models.CommentModels
{
    public static class CommentExtansion
    {
        public static Comment FromDTO(this CommentRequestModel model)
        {
            return new Comment()
            {
                CommentId = Guid.NewGuid().ToString(),
                ParrentId = model.ParrentId,
                UserId = model.UserId,
                CommentText = model.CommentText,
                CreatedAt = DateTime.Now,
                TopicURL = model.TopicURL
            };
        }

        public static CommentResponseModel ToDTO(this Comment comment)
        {
            return new CommentResponseModel
            {
                TopicURL = comment.TopicURL,
                CommentId = comment.CommentId,
                ParrentId = comment.ParrentId,
                UserId = comment.UserId,
                UserNickName= comment.UserNickName,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                Likes = comment.Likes,
                DisLikes = comment.DisLikes,
                UpdatedAt = comment.UpdatedAt
            };
        }

        public static Comment FromDTO(this CommentResponseModel  comment)
        {
            return new Comment
            {
                TopicURL = comment.TopicURL,
                CommentId = comment.CommentId,
                ParrentId = comment.ParrentId,
                UserId = comment.UserId,
                UserNickName = comment.UserNickName,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                Likes = comment.Likes,
                DisLikes = comment.DisLikes,
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
