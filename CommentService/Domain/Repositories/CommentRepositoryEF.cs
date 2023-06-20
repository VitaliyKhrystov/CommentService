using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Domain.Repositories
{
    public class CommentRepositoryEF : ICommentRepository, IDisposable
    {
        private readonly AppDbContext dbContext;
        public CommentRepositoryEF(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await dbContext.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await dbContext.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(string commentId)
        {
            return await dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId, default);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            if (await dbContext.Comments.AnyAsync(c => c.CommentId == comment.CommentId))
            {
                dbContext.Comments.Update(comment);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string commentId)
        {
            var comment = await GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                dbContext.Comments.Remove(comment);
                await dbContext.SaveChangesAsync();
            }
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
