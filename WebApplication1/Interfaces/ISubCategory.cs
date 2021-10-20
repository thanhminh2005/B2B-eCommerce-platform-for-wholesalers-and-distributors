using API.DTOs.SubCategories;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ISubCategoryService
    {
        Task<Response<IEnumerable<SubCategoryResponse>>> GetSubCategories(GetSubCategoriesRequest request);
        Task<Response<SubCategoryResponse>> GetSubCategoryById(GetSubCategoryByIdRequest request);
        Task<Response<string>> CreateSubCategory(CreateSubCategoryRequest request);
        Task<Response<string>> UpdateSubCategory(UpdateSubCategoryRequest request);
    }
}
