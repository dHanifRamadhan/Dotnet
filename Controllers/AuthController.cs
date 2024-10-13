using Microsoft.AspNetCore.Mvc;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;
using Project01.Services;

namespace Project01.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseControllers {
        private readonly IUserService userService;
        private readonly ILogger<AuthController> logger;
        private const string SUCCESS = Constant.MESSAGE_SUCCESS_FUNCTION;
        private const string ERROR = Constant.MESSAGE_ERROR_FUNCTION;
        private string EXCEPTION = string.Format(Constant.CONTROLLER_MESSAGE_EXCEPTION, "AuthController", "{0}");
        public AuthController(IUserService userService, ILogger<AuthController> logger) {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthorizationRequest item) {
            try {
                var data = await userService.Auth(item);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(EXCEPTION, e);
                return _BadRequest(string.Format(EXCEPTION, e.Message), null);
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check() {
            try {
                var user = HttpContext.Items["User"];
                return _Ok(SUCCESS, user);
            } catch (Exception e) {
                logger.LogError(EXCEPTION, e);
                return _BadRequest(string.Format(EXCEPTION, e.Message), null);
            }
        }
    }
}