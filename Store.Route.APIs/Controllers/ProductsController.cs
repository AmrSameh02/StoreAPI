using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.Core.DTOs.Products;
using Store.Route.Core.Helper;
using Store.Route.Core.Services.Contract;
using Store.Route.Core.Specifications.Products;

namespace Store.Route.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet] //GET BaseUrl/api/products
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)
        {
            var result = await _productService.GetAllProductAsync(productSpec);
            return Ok(result);
        }


        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")] //GET BaseUrl/api/products/brands
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }


        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")]//GET BaseUrl/api/products/types
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] //GET BaseUrl/api/products
        public async Task<ActionResult<ProductDto>> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _productService.GetProductById(id.Value);
            if (result is null) return NotFound(new ApiErrorResponse(404, $"The product With Id: {id} not found at DB"));

            return Ok(result);
        }

    }
}
