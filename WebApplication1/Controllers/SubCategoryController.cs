using API.Contracts;
using API.DTOs.SubCategories;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService categoryService)
        {
            _subCategoryService = categoryService;
        }

        [HttpGet(ApiRoute.SubCategories.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetSubCategoriesRequest request)
        {
            var response = await _subCategoryService.GetSubCategories(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.SubCategories.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetSubCategoryByIdRequest request = new GetSubCategoryByIdRequest
            {
                Id = id
            };
            var response = await _subCategoryService.GetSubCategoryById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.SubCategories.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSubCategoryRequest request)
        {
            var response = await _subCategoryService.CreateSubCategory(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.SubCategories.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateSubCategoryRequest request)
        {
            var response = await _subCategoryService.UpdateSubCategory(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
