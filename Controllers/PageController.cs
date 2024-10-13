using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Project01.Entities;
using Project01.Helpers;
using Project01.Services;
using Project01.Validations;

namespace Project01.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : BaseControllers {
        private readonly ILogger<PageController> logger;
        private readonly IPageService pageService;
        private readonly IValidator<Page> pageValidator;
        private const string SUCCESS = Constant.MESSAGE_SUCCESS_FUNCTION;
        private const string ERROR = Constant.MESSAGE_ERROR_FUNCTION;
        private string EXCEPTION = string.Format(Constant.CONTROLLER_MESSAGE_EXCEPTION, "PageController", "{0}");
        public PageController(
            ILogger<PageController> logger, 
            IPageService pageService,
            IValidator<Page> pageValidator
        ) {
            this.logger = logger;
            this.pageService = pageService;
            this.pageValidator = pageValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10
        ) {
            try {
                var data = await pageService.GetAll(page, pageSize);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions() {
            try {
                var data = await pageService.GetOptions();
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Page item) {
            try {
                if (item == null)
                    return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                var result = pageValidator.Validate(item);
                if (!result.IsValid)
                    return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                var final = await pageService.Add(item);
                return _Ok(SUCCESS, final);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code) {
            try {
                var data = await pageService.GetByCode(code);
                return _Ok(SUCCESS, data);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Page item) {
            try {
                if (item == null)
                    return _NotFound(Constant.CONTROLLER_MESSAGE_BODY_NULL);

                var result = pageValidator.Validate(item);
                if (!result.IsValid)
                    return _BadRequest(ERROR, result.Errors.Select(x => x.ErrorMessage).ToList());

                var final = await pageService.Update(item);
                return _Ok(SUCCESS, final);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Remove(string code) {
            try {
                var data = await pageService.GetByCode(code);
                if (data == null)
                    return _NotFound(Constant.SERVICE_MESSAGE_NOT_FOUND);

                var result = await pageService.Remove(data);
                return _Ok(SUCCESS, result);
            } catch (Exception e) {
                logger.LogError(string.Format(EXCEPTION, e));
                return _BadRequest(ERROR, null);
            }
        }
    }
}