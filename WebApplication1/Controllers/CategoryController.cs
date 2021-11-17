using API.Contracts;
using API.DTOs.Categories;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet(ApiRoute.Categories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetCategoriesRequest request)
        {
            var response = await _categoryService.GetCategories(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Categories.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetCategoryByIdRequest request = new GetCategoryByIdRequest
            {
                Id = id
            };
            var response = await _categoryService.GetCategoryById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.Categories.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.Categories.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            var response = await _categoryService.UpdateCategory(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
