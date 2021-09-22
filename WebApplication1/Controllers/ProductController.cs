using API.Contracts;
using API.DTOs.Products;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
