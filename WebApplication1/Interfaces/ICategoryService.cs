using System;
using API.Warppers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Categories;
using API.Domains;

namespace API.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<IEnumerable<CategoryResponse>>> GetCategories();
        Task<Response<CategoryResponse>> GetCategoryById(GetCategoryByIdRequest request);
        Task<Response<string>> CreateCategory(CreateCategoryRequest request);
        Task<Response<string>> UpdateCategory(UpdateCategoryRequest request);
    }
}
