using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Repository.Data.Contexts;

namespace Store.Route.APIs.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")] //GET: /api/buggy/notfound
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await _context.Brands.FindAsync(100);
            if (brand is null) return NotFound(new ApiErrorResponse(404));

            return Ok(brand);            
        }

        [HttpGet("servererror")] //GET: /api/buggy/servererror
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);
            var brandToString =  brand.ToString(); //null refrence exception

            return Ok(brand);
        }

        [HttpGet("badrequest")] //GET: /api/buggy/badrequest
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse (400));
        }

        [HttpGet("badrequest/{id}")] //GET: /api/buggy/badrequest
        public async Task<IActionResult> GetBadRequestError(int id) //Validation Error
        {
            return Ok();
        }

        [HttpGet("unauthorized")] //GET: /api/buggy/unauthorized
        public async Task<IActionResult> GetUnauthorizedError(int id)
        {
            return Unauthorized(new ApiErrorResponse(401));
        }

    }
}
