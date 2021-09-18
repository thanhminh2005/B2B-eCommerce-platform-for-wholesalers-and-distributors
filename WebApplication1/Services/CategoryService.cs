using API.Domains;
using API.DTOs.Categories;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Response<string>> CreateRole(CreateCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<IEnumerable<CategoryResponse>>> GetCategory()
        {
            var Categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            if (Categories != null)
            {
                return new Response<IEnumerable<CategoryResponse>>(_mapper.Map<IEnumerable<CategoryResponse>>(Categories), message: "Success");
            }
            return new Response<IEnumerable<CategoryResponse>>(message: "Empty");
        }

        public async Task<Response<CategoryResponse>> GetCategoryById(GetCategoryByIdRequest request)
        {
            
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Guid.Parse(request.Id));
               
                if (category != null)
                {
                    return new Response<CategoryResponse>(_mapper.Map<CategoryResponse>(category), message: "Success");
                }
            }
            return new Response<CategoryResponse>(message: "Category not Found");
        }


    }
}
