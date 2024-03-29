﻿using CommentService.Domain.Enteties;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Models;
using CommentService.Models.ForgotPassport;
using CommentService.Models.UserModels;
using CommentService.Services;
using CommentService.Services.EncryptDecryptData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ListErrors listErrors;
        private readonly EmailSender emailSender;
        private static int confirmationNumber;

        public AccountController(ILogger<AccountController> logger, JWTservice jWTservice, IUserRepository userRepository, IEncryptDecryptData encryptDecryptData, IRoleRepository roleRepository, ListErrors listErrors, EmailSender emailSender)
        {
            this.logger = logger;
            this.jWTservice = jWTservice;
            this.userRepository = userRepository;
            this.encryptDecryptData = encryptDecryptData;
            this.roleRepository = roleRepository;
            this.listErrors = listErrors;
            this.emailSender = emailSender;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterModel model)
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
                        RoleId = role.Id,
                        BirthYear= model.BirthYear
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
                var errors = listErrors.GetErrors(this);
                errors.AsParallel().ForAll(err => logger.LogError("ModelState error: \n" + $"{err.Key} => {err.Value}"));
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
                return BadRequest($"Not found: NickName - {model.NickName}. Please register!");
            }
                
            if (model.Password != encryptDecryptData.DecryptDataFromBase64(user.Password))
                ModelState.AddModelError("Password", "Password do NOT match!");

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
                var errors = listErrors.GetErrors(this);
                errors.AsParallel().ForAll(err => logger.LogError("ModelState error: \n" + $"{err.Key} => {err.Value}"));
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
                return BadRequest("The principal is empty");

            var userName = principal.Identity.Name;
            var user = await userRepository.GetUserByNickNameAsync(userName);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var userRole = principal.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            var newAccessToken = jWTservice.CreateJWToken(user.Id, userName, user.Email, userRole.ToString());

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

        [HttpPost ("forgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userRepository.GetUserByEmailAsync(model.Email);
                    if (user != null)
                    {
                        confirmationNumber = new Random().Next(1000, 1000000);
                        emailSender.Sender("confirmation email", $"Confirmation number: {confirmationNumber}", model.Email);
                        var response = new ForgotPasswordResponse()
                        {
                            UserId = user.Id,
                            ConfirmationNumber = confirmationNumber
                        };

                        return Ok(response);
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError("ForgotPassword Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("User can't be found!");
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPassword model)
        {
            if (ModelState.IsValid && confirmationNumber == model.ConfirmationNumber)
            {
                try
                {
                    var user = await userRepository.GetUserByIdAsync(model.UserId);
                    if(user != null)
                    {
                        user.Password = encryptDecryptData.EncryptDataToBase64(model.Password);
                        await userRepository.UpdateUserAsync(user);
                        return Ok();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError ("ResetPassword Error: " + ex.Message);
                }
            }
            return BadRequest("Incorrect confirmation data!");
        }
    }
}
