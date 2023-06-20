using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommentService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly ILogger<CommentController> logger;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
        {
            this.commentRepository = commentRepository;
            this.logger = logger;
        }

        [HttpPost ("create")]
        public async Task<IActionResult> CreateComment(CommentModel model)
        {
            if (model == null)
                return NoContent();

            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    CommentId = Guid.NewGuid().ToString(),
                    ParrentId = model.ParrentId,
                    UserId = model.UserId,
                    CommentText = model.CommentText,
                    CreatedAt= DateTime.Now,
                    TopicURL= model.TopicURL
                };
                try
                {
                    await commentRepository.CreateCommentAsync(comment);
                    return Ok("Comment created successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateComment Error: " + ex.Message);
                    return BadRequest();
                }
            }
            else
            {
                var errors = new List<ModelError>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error);
                        logger.LogError("ModelState error: \n" + error);
                    }
                }
                return BadRequest(errors);
            }
        }
    }
}
