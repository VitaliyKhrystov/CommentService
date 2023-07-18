using CommentService.Domain;
using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using CommentService.Models.CommentModels;
using CommentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        private readonly IActionRepository actionRepository;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger, IUserRepository userRepository, IRoleRepository roleRepository, ListErrors listErrors, AppDbContext appContext, IActionRepository actionRepository)
        {
            this.commentRepository = commentRepository;
            this.logger = logger;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.listErrors = listErrors;
            this.appContext = appContext;
            this.actionRepository = actionRepository;
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
                    var user = await userRepository.GetUserByIdAsync(model.UserId);
                    comment.UserNickName = user.NickName;
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
                errors.AsParallel().ForAll(err => logger.LogError("ModelState error: \n" + $"{err.Key} => {err.Value}"));
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
                errors.AsParallel().ForAll(err => logger.LogError("ModelState error: \n" + $"{err.Key} => {err.Value}"));
                return BadRequest(errors);
            } 
        }

        [AllowAnonymous]
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
                    UserNickName = c.UserNickName,
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

        [AllowAnonymous]
        [HttpGet("getCommentById")]
        public async Task<CommentResponseModel> GetCommentByIdAsync(string commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            return comment.ToDTO();
        }

        [HttpDelete ("delete/{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync(string commentId)
        {
            var comments = await commentRepository.GetAllCommentsAsync();
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            var commentsList = comments.Select(c => c.ToDTO()).ToList();

            if (comment != null)
            {
                try
                {
                    var listReplies = GetReplies(commentsList, commentId);

                    var list = new List<Comment>();
                    list.Add(comment);

                    if (listReplies != null)
                        GetRepliesList(listReplies, in list);

                    await commentRepository.DeleteCommentsAsync(list);
                    
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

        [HttpPost ("setLike")]
        public async Task<IActionResult> SetLikeAsync(ActionLikeDislikeModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                var comment = await commentRepository.GetCommentByIdAsync(model.CommentId);

                if (comment == null)
                    return BadRequest();

                var like = new Like()
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentId = model.CommentId,
                    Comment = comment,
                    CreateAt = DateTime.Now,
                    UserId = model.UserId
                };

                await actionRepository.CreateAsync(like);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("SetLike Error: " + ex.Message);
                return BadRequest();
            }
            
        }

        [HttpPost("deleteLike")]
        public async Task<IActionResult> DeleteLikeAsync(ActionLikeDislikeModel model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var comments = await commentRepository.GetAllCommentsAsync();
                var comment = comments.FirstOrDefault(c => c.CommentId == model.CommentId);

                if (comment == null)
                    return NoContent();

                var Like = comment.Likes.FirstOrDefault(d => d.UserId == model.UserId, default);

                if (Like != null)
                {
                    await actionRepository.DeleteAsync(Like);
                    return Ok();
                }
                else
                    return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError("DeleteLike Error: " + ex.Message);
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

                if (comment == null)
                    return BadRequest();

                var disLike = new DisLike()
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentId = model.CommentId,
                    Comment = comment,
                    CreateAt = DateTime.Now,
                    UserId = model.UserId
                };

                await actionRepository.CreateAsync(disLike);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("SetDisLike Error: " + ex.Message);
                return BadRequest();
            }

        }

        [HttpPost("deleteDisLike")]
        public async Task<IActionResult> DeleteDisLikeAsync(ActionLikeDislikeModel model)
        {
            if (model == null)
                return BadRequest();

            try
            {
                var comments = await commentRepository.GetAllCommentsAsync();
                var comment = comments.FirstOrDefault(c => c.CommentId == model.CommentId);

                if (comment == null)
                    return NoContent();

                var disLike = comment.DisLikes.FirstOrDefault(d => d.UserId == model.UserId, default);

                if (disLike != null)
                {
                    await actionRepository.DeleteAsync(disLike);
                    return Ok();
                }
                else
                    return NoContent();
                    
            }
            catch (Exception ex)
            {
                logger.LogError("DeleteDisLike Error: " + ex.Message);
                return BadRequest();
            }

        }

        private void GetRepliesList(List<CommentResponseModel> replies, in List<Comment> list)
        {
            foreach (var reply in replies)
            {
                list.Add(reply.FromDTO());
                if (reply.Replies != null)
                {
                    GetRepliesList(reply.Replies, in list);
                }
            }
        }

        private List<CommentResponseModel> GetReplies(List<CommentResponseModel> model, string parrentId)
        {
            var inner = model;

            return model.Where(x => x.ParrentId == parrentId).Select(c => new CommentResponseModel()
            {
                TopicURL= c.TopicURL,
                CommentId = c.CommentId,
                ParrentId = parrentId,
                UserId = c.UserId,
                UserNickName = c.UserNickName,
                CommentText = c.CommentText,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Likes = c.Likes,
                DisLikes = c.DisLikes,
                Replies = inner.Where(r => r.ParrentId == c.CommentId).Count() > 0 ? GetReplies(inner, c.CommentId) : null
            }).ToList();
        }
    }
}
