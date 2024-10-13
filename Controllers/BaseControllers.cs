using Microsoft.AspNetCore.Mvc;

namespace Project01.Controllers {
    public class BaseControllers : ControllerBase {
        protected IActionResult _Ok(string message) {
            return Ok(new {
                code = 200,
                message = message
            });
        }

        protected IActionResult _Ok(string message, Object data) {
            return Ok(new {
                code = 200,
                message = message,
                results = data
            });
        }

        protected IActionResult _NotFound(string message) {
            return NotFound(new {
                code = 404,
                message = message
            });
        }

        protected IActionResult _BadRequest(string message, Object? result) {
            return BadRequest(new {
                code = 400,
                message = message,
                result = result
            });
        }
    }
}