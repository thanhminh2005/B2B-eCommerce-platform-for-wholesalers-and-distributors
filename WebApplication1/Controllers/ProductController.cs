using API.Contracts;
using API.DTOs.Products;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiRoute.Products.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetProductByIdRequest request = new GetProductByIdRequest
            {
                Id = id
            };
            var response = await _productService.GetProductById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.GetDistributor)]
        public async Task<IActionResult> GetDistributor(string id)
        {
            GetProductByDistributorIdRequest request = new GetProductByDistributorIdRequest
            {
                DistributorId = id
            };
            var response = await _productService.GetProductByDistributorId(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.Filter)]
        public async Task<IActionResult> Filter([FromQuery] GetProductsWithFilterRequest request)
        {
            var response = await _productService.GetProductsWithFilter(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var response = await _productService.CreateProduct(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.Products.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            
            var response = await _productService.UpdateProduct(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpDelete(ApiRoute.Products.Delete)]
        public async Task<IActionResult> Delete([FromBody] RemoveProductRequest request)
        {

            var response = await _productService.RemoveProduct(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        
    }
}
