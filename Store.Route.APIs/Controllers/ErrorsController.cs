using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;

namespace Store.Route.APIs.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {

        public IActionResult Error(int code)
        {
            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Not found end point"));
        }
    }
}
