using CommentService.Domain.Enteties;

namespace CommentService.Domain.Repositories.Abstract
{
    public interface IActionRepository
    {
        Task CreateAsync(ActionLikeDislike action);
        Task DeleteAsync(ActionLikeDislike action);

    }
}
