using CommentService.Domain;
using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using CommentService.Models.CommentModels;
using CommentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CommentService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly ILogger<CommentController> logger;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ListErrors listErrors;
        private readonly AppDbContext appContext;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger, IUserRepository userRepository, IRoleRepository roleRepository, ListErrors listErrors, AppDbContext appContext)
        {
            this.commentRepository = commentRepository;
            this.logger = logger;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.listErrors = listErrors;
            this.appContext = appContext;
        }

        [HttpPost ("create")]
        public async Task<IActionResult> CreateCommentAsync(CommentRequestModel model)
        {
            if (model == null)
                return NoContent();

            if (ModelState.IsValid)
            {
                var comment = model.FromDTO();
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
                var errors = listErrors.GetErrors(this);
                errors.ForEach(error => { logger.LogError("ModelState error: \n" + error); });
                return BadRequest(errors);
            }
        }

        [HttpPut ("update")]
        public async Task<IActionResult> UpdateAsync(UpdateCommentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var comment = await commentRepository.GetCommentByIdAsync(model.CommentId);
                    var user = await userRepository.GetUserByIdAsync(model.UserId);
                    var role = await roleRepository.GetRoleByIdAsync(user.RoleId);

                    if (comment == null || (comment.UserId == model.UserId ? false : !(role.RoleName == Roles.Admin || role.RoleName == Roles.Moderator)))
                        return BadRequest();

                    comment.UpdatedAt = DateTime.Now;
                    comment.CommentText = model.CommentText;
                    await commentRepository.UpdateCommentAsync(comment);

                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError($"UpdateAsync Error: {ex.Message}", ex);
                    return BadRequest();
                }
            }
            else
            {
                var errors = listErrors.GetErrors(this);
                errors.ForEach(error => { logger.LogError("ModelState error: \n" + error); });
                return BadRequest(errors);
            } 
        }

        [HttpGet ("getAllComments")]
        public async Task<List<CommentResponseModel>> GetAllCommentsAsync()
        {
            var comments = await commentRepository.GetAllCommentsAsync();
            var commentsRespose = comments.Select(c => c.ToDTO()).ToList();
            var mainComments = commentsRespose.Where(c => c.ParrentId == "");
            var subList = new List<CommentResponseModel>();
            var result = new List<CommentResponseModel>();

            foreach (var comment in mainComments)
            {
                var commentList = commentsRespose;

                subList = commentsRespose.Where(c => c.CommentId == comment.CommentId).Select(c => new CommentResponseModel() 
                { 
                    CommentId = c.CommentId,
                    ParrentId = c.ParrentId,
                    UserId = c.UserId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Likes = c.Likes,
                    DisLikes = c.DisLikes,
                    Replies = GetReplies(commentList, c.CommentId)
                }).ToList();

                result.AddRange(subList);
            }

            return result;
        }

        [HttpGet("getComment")]
        public async Task<CommentResponseModel> GetCommentAsync(string commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            return comment.ToDTO();
        }

        [HttpDelete ("delete")]
        public async Task<IActionResult> DeleteCommentAsync(string commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync (commentId);
            if (comment != null)
            {
                try
                {
                    await commentRepository.DeleteUserAsync(comment);
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError("DeleteComment Error: {0}", ex.Message);
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        private List<CommentResponseModel> GetReplies(List<CommentResponseModel> model, string parrentId)
        {
            var inner = model;

            return model.Where(x => x.ParrentId == parrentId).Select(c => new CommentResponseModel()
            {
                CommentId = c.CommentId,
                ParrentId = parrentId,
                UserId = c.UserId,
                CommentText = c.CommentText,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Likes = c.Likes,
                DisLikes = c.DisLikes,
                Replies = inner.Where(r => r.ParrentId == c.CommentId).Count() > 0 ? GetReplies(inner, c.CommentId) : null
            }).ToList();
        }

        [HttpPost ("setLike")]
        public async Task<IActionResult> SetLikeAsync(ActionLikeDislikeModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                var comment = await commentRepository.GetCommentByIdAsync(model.CommentId);

                if (comment == null || (comment.Likes.Any(u => u.UserId == model.UserId)
                                    || comment.DisLikes.Any(u => u.UserId == model.UserId)))
                    return BadRequest();

                var like = new Like()
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentId = model.CommentId,
                    Comment = comment,
                    CreateAt = DateTime.Now,
                    UserId = model.UserId
                };

                comment.Likes.Add(like);
                await commentRepository.UpdateCommentAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("SetLike Error: " + ex.Message);
                return BadRequest();
            }
            
        }

        [HttpPost("setDisLike")]
        public async Task<IActionResult> SetDisLikeAsync(ActionLikeDislikeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var comment = await commentRepository.GetCommentByIdAsync(model.CommentId);

                if (comment == null || (comment.Likes.Any(u => u.UserId == model.UserId)
                                    || comment.DisLikes.Any(u => u.UserId == model.UserId)))
                    return BadRequest();

                var disLike = new DisLike()
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentId = model.CommentId,
                    Comment = comment,
                    CreateAt = DateTime.Now,
                    UserId = model.UserId
                };

                comment.DisLikes.Add(disLike);
                await commentRepository.UpdateCommentAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("SetDisLike Error: " + ex.Message);
                return BadRequest();
            }

        }
    }
}
