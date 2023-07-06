using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using CommentService.Models.UserModels;
using CommentService.Services.EncryptDecryptData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<UserController> logger;
        private readonly IEncryptDecryptData encryptDecryptData;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<UserController> logger, IEncryptDecryptData encryptDecryptData)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
            this.encryptDecryptData = encryptDecryptData;
        }

        [HttpGet ("getUserById")]
        public async Task<UserResponseModel> GetUserByIdAsync([FromQuery] string userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user != null)
            {
                try
                {
                    var userResponse = user.ToDTO();
                    var role = await roleRepository.GetRoleByIdAsync(user.RoleId);
                    userResponse.RoleName = role.RoleName.ToString();

                    return userResponse;
                }
                catch (Exception ex)
                {
                    logger.LogError("GetUserById: " + ex.Message);
                    return new UserResponseModel();
                }
            }
            return new UserResponseModel();
        }

        [HttpGet]
        public async Task<List<UserResponseModel>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();

            if (users != null)
            {
                try
                {
                    var roles = await roleRepository.GetAllRolesAsync();
                    var usersResponse = users.Select(x => x.ToDTO()).ToList();

                    return usersResponse;
                }
                catch (Exception ex)
                {
                    logger.LogError("GetAllUsers: " + ex.Message);
                    return new List<UserResponseModel>();
                }
            }
            return new List<UserResponseModel>();
        }

        [HttpPost ("updateUser")]
        public async Task<IActionResult> UpdateUserAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userRepository.GetUserByNickNameAsync(model.NickName);
                    user.NickName = model.NickName;
                    user.Email = model.Email;
                    user.Password = encryptDecryptData.EncryptDataToBase64(model.Password);
                    user.BirthYear = model.BirthYear;

                    await userRepository.UpdateUserAsync(user);

                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError("UpdateUser: " + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpDelete ("deleteUser")]
        public async Task<IActionResult> DeleteUserAsync(string nickName)
        {
            try
            {
                await userRepository.DeleteUserAsync(nickName);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("DeleteUser: " + ex.Message);
                return BadRequest();
            }
        }
    }
}
