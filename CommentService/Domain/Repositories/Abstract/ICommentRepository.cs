using CommentService.Domain.Enteties;

namespace CommentService.Domain.Repositories.Abstract
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentByIdAsync(string commentId);
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task DeleteCommentAsync(Comment comment);
        Task DeleteCommentsAsync(IEnumerable<Comment> comments);
    }
}
