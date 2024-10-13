using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;
using Project01.Services;
using Project01.Validations;

namespace Project01.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseControllers {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IPageService pageService;
        private readonly IAccessService accessService;
        private readonly IValidator<User> userValidator;
        private MysqlContext context;
        private string ERROR = Constant.MESSAGE_ERROR_FUNCTION;
        private string SUCCESS = Constant.MESSAGE_SUCCESS_FUNCTION;
        private string EXCEPTION = string.Format(Constant.CONTROLLER_MESSAGE_EXCEPTION, "UsersController", "{0}");
        public UserController(
            ILogger<UserController> logger, 
            IUserService userService,
            IRoleService roleService,
            IPageService pageService,
            IAccessService accessService,
            IValidator<User> userValidator,
            MysqlContext context
        ) {
            this.logger = logger;
            this.userService = userService;
            this.roleService = roleService;
            this.pageService = pageService;
            this.accessService = accessService;
            this.userValidator = userValidator;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10
        ) {
            try {
                var data = await userService.GetAll(page, pageSize);
                return _Ok(SUCCESS, data);
            } catch(Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User item) {
            try {
                if (item == null)
                    return _NotFound(Constant.CONTROLLER_MESSAGE_BODY_NULL);

                if ((await roleService.GetByCode(item.RoleCode)) == null)
                    return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                var result = userValidator.Validate(item);
                if (!result.IsValid)
                    return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                item.Password = AuthPassword.Hash(item.Password);
                
                var final = await userService.Add(item);
                return _Ok(SUCCESS, final);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code) {
            try {
                var data = await userService.GetByCode(code);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User item) {
            try {
                if (item == null)
                    return _NotFound(Constant.CONTROLLER_MESSAGE_BODY_NULL);

                if ((await roleService.GetByCode(item.RoleCode)) == null)
                    return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                var result = userValidator.Validate(item);
                if (!result.IsValid)
                    return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                item.Password = AuthPassword.Hash(item.Password);

                var final = await userService.Update(item);
                return _Ok(SUCCESS, "final");
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code) {
            try {
                var result = await userService.GetByCode(code);
                if (result == null)
                    return _NotFound(string.Format(Constant.SERVICE_MESSAGE_NOT_FOUND));

                var data = await userService.Remove(result);    
                return _Ok(SUCCESS, data);
            } catch (Exception e)  {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }
    }
}