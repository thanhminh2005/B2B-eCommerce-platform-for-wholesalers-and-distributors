using API.Domains;
using API.DTOs.Categories;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;
using static API.Helpers.TreeExtensions;

namespace API.Interfaces
{
    public interface ICategoryService
    {
        Task<Response<IEnumerable<TreeItem<Category>>>> GetCategories();
        Task<Response<CategoryHierachy>> GetCategoryById(GetCategoryByIdRequest request);
        Task<Response<string>> CreateCategory(CreateCategoryRequest request);
        Task<Response<string>> UpdateCategory(UpdateCategoryRequest request);
    }
}
