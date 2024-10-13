using System.Data.Common;
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
    public class RoleController : BaseControllers {
        private readonly MysqlContext context;
        private readonly ILogger<RoleController> logger;
        private readonly IRoleService roleService;
        private readonly IPageService pageService;
        private readonly IAccessService accessService;
        private readonly IValidator<Role> roleValidator;
        private readonly IValidator<Page> pageValidator;
        private const string SUCCESS = Constant.MESSAGE_SUCCESS_FUNCTION;
        private const string ERROR = Constant.MESSAGE_ERROR_FUNCTION;
        private string EXCEPTION = string.Format(Constant.CONTROLLER_MESSAGE_EXCEPTION, "RoleController", "{0}");
        public RoleController(
            MysqlContext context,
            ILogger<RoleController> logger, 
            IRoleService roleService,
            IPageService pageService,
            IAccessService accessService,
            IValidator<Role> roleValidator,
            IValidator<Page> pageValidator
        ) {
            this.context = context;
            this.logger = logger;
            this.roleService = roleService;
            this.pageService = pageService;
            this.accessService = accessService; 
            this.roleValidator = roleValidator;
            this.pageValidator = pageValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10
        ) {
            try {
                var data = await roleService.GetAll(page, pageSize);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions() {
            try {
                var data = await roleService.GetOptions();
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RolePageRequest item) {
            try {
                if (item == null)
                    return _NotFound(Constant.CONTROLLER_MESSAGE_BODY_NULL);

                string? final;

                using (var transaction = context.Database.BeginTransaction()) {
                    context.Database.UseTransaction(transaction.GetDbTransaction());

                    Role newRole = new Role {
                        RoleName = item.RoleName, 
                    };

                    var result = roleValidator.Validate(newRole);
                    if (!result.IsValid)
                        return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                    final = await roleService.Add(newRole);

                    List<Access>? listAccess = new List<Access>();
                    foreach (var value in item.PageActions) {
                        Access newAccess = new Access {
                            RoleCode = final,
                            PageCode = value.PageCode,
                            Actions = value.Actions
                        };

                        var pageExist = await pageService.GetByCode(value.PageCode);
                        if (pageExist == null)
                            return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                        listAccess.Add(newAccess);
                    }

                    await accessService.Add(listAccess);
                    transaction.Commit();
                    
                    return _Ok(SUCCESS);
                }
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                if (context.Database.CurrentTransaction != null)
                    context.Database.CurrentTransaction.Rollback();

                return _BadRequest(ERROR, null);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code) {
            try {
                var data = await roleService.GetByCode(code);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RolePageRequest item) {
            try {
                if (item == null)
                    return _NotFound(Constant.CONTROLLER_MESSAGE_BODY_NULL);

                using (var transaction = context.Database.BeginTransaction()) {
                    context.Database.UseTransaction(transaction.GetDbTransaction());

                    Role newRole = new Role {
                        RoleCode = item.RoleCode,
                        RoleName = item.RoleName, 
                    };

                    var result = roleValidator.Validate(newRole);
                    if (!result.IsValid)
                        return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                    await roleService.Update(newRole);

                    List<Access>? listAccess = new List<Access>();
                    foreach (var value in item.PageActions) {
                        Access newAccess = new Access {
                            RoleCode = newRole.RoleCode,
                            PageCode = value.PageCode,
                            Actions = value.Actions
                        };

                        var pageExist = await pageService.GetByCode(value.PageCode);
                        if (pageExist == null)
                            return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                        listAccess.Add(newAccess);
                    }

                    List<Access> oldList = await accessService.GetByRoleCode(newRole.RoleCode);
                    await accessService.Remove(oldList);

                    await accessService.Add(listAccess);
                    transaction.Commit();
                    
                    return _Ok(SUCCESS);
                }
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                if (context.Database.CurrentTransaction != null)
                    context.Database.CurrentTransaction.Rollback();

                return _BadRequest(ERROR, null);
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code) {
            try {
                var data = await roleService.GetByCode(code);
                if (data == null)
                    return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                var result = await roleService.Remove(data);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }
    }
}