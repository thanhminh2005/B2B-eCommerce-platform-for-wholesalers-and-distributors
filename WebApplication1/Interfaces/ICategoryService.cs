using API.DTOs.Categories;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

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
