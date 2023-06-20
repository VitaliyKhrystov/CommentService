using Azure.Core;
using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using CommentService.Services;
using CommentService.Services.EncryptDecryptData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly JWTservice jWTservice;
        private readonly IUserRepository userRepository;
        private readonly IEncryptDecryptData encryptDecryptData;
        private readonly IRoleRepository roleRepository;

        public AccountController(ILogger<AccountController> logger, JWTservice jWTservice, IUserRepository userRepository, IEncryptDecryptData encryptDecryptData, IRoleRepository roleRepository)
        {
            this.logger = logger;
            this.jWTservice = jWTservice;
            this.userRepository = userRepository;
            this.encryptDecryptData = encryptDecryptData;
            this.roleRepository = roleRepository;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            var users = await userRepository.GetAllUsersAsync();
            if (users.Any(u => u.NickName == model.NickName))
                ModelState.AddModelError("", $"User with NickName: {model.NickName} exists!");

            if (users.Any(u => u.Email == model.Email))
                ModelState.AddModelError("", $"User with Email: {model.Email} exists!");

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = Guid.NewGuid().ToString();
                    var userPassword = encryptDecryptData.EncryptDataToBase64(model.Password);
                    var role = await roleRepository.GetRoleByNameAsync(model.RoleName != null ? model.RoleName : Roles.User);
                    var newUser = new User()
                    {
                        Id = userId,
                        NickName = model.NickName,
                        Email = model.Email,
                        Password = userPassword,
                        Role = role,
                        RoleId = role.Id
                    };
                    await userRepository.CreateUserAsync(newUser);

                    return Ok("User created successfully!");
                }
                catch (Exception ex)
                {
                    logger.LogError("Register error: \n" + ex.Message);
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

        [HttpPost ("login")]
        public async Task<IActionResult> LoginAsync(UserLoginModel model)
        {
            var user = await userRepository.GetUserByNickNameAsync(model.NickName);
            if (user == null)
            {
                logger.LogError($"Not found: NickName - {model.NickName}");
                return Unauthorized($"Not found: NickName - {model.NickName}.Please register!");
            }
                
            if (model.Password != encryptDecryptData.DecryptDataFromBase64(user.Password))
                ModelState.AddModelError("", "Password do NOT match!");

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await roleRepository.GetRoleByIdAsync(user.RoleId);
                    var token = jWTservice.CreateJWToken(user.Id, user.NickName, user.Email, role.RoleName.ToString());
                    var refreshToken = jWTservice.GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(JWTconfig.RefreshTokenExpirationDays);
                    await userRepository.UpdateUserAsync(user);

                    var response = new TokenModel
                    {
                       AccessToken = token,
                       RefreshToken = refreshToken
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError("Login error: \n" + ex.Message);
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

        [Authorize]
        [HttpPost ("logout")]
        public async Task<IActionResult> LogOut()
        {
            var userName = this.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var user = await userRepository.GetUserByNickNameAsync(userName);
            if (user == null)
                return BadRequest("Invalid user name");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = default;
            await userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
                return BadRequest("Invalid client request");

            var principal = jWTservice.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            var userRole = principal.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            var userEmail = principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var userId = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var userName = principal.Identity.Name;

            var user = await userRepository.GetUserByNickNameAsync(userName);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid access token or refresh token");

            var newAccessToken = jWTservice.CreateJWToken(userId, userName, userEmail, userRole);
            var newRefreshToken = jWTservice.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(JWTconfig.RefreshTokenExpirationDays);
            await userRepository.UpdateUserAsync(user);

            var response = new TokenModel()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet ("ping")]
        public string Ping()
        {
            return "Pong";
        }
    }
}
