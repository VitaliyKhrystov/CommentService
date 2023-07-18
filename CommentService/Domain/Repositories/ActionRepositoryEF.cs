using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;

namespace CommentService.Domain.Repositories
{
    public class ActionRepositoryEF : IActionRepository
    {
        private readonly AppDbContext dbContext;

        public ActionRepositoryEF(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(ActionLikeDislike action)
        {
            if (action is Like)
                await dbContext.Likes.AddAsync(action as Like);
            else if (action is DisLike)
                await dbContext.DisLikes.AddAsync(action as DisLike);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ActionLikeDislike action)
        {
            if (action is Like)
                dbContext.Likes.Remove(action as Like);
            else if (action is DisLike)
                dbContext.DisLikes.Remove(action as DisLike);
            await dbContext.SaveChangesAsync();
        }

    }
}
