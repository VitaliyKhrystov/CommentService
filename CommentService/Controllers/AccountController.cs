using CommentService.Domain;
using CommentService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly AppDbContext appDbContext;
        private readonly JWTservice jWTservice;

        public AccountController(ILogger<AccountController> logger, AppDbContext appDbContext, JWTservice jWTservice)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
            this.jWTservice = jWTservice;
        }


    }
}
