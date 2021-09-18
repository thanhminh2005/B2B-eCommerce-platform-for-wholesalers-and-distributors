using API.Contracts;
using API.DTOs.Categories;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet(ApiRoute.Categories.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetCategory();
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

    }
}
