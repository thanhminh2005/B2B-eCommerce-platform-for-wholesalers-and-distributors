using API.Domains;
using API.DTOs.Categories;
using API.Helpers;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Helpers.TreeExtensions;

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

        public async Task<Response<string>> CreateCategory(CreateCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var category = await _unitOfWork.GetRepository<Category>().FirstAsync(c => c.Name.Equals(request.Name));
                if (category == null)
                {
                    var newCategory = _mapper.Map<Category>(request);
                    newCategory.Id = Guid.NewGuid();
                    newCategory.DateCreated = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<Category>().AddAsync(newCategory);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "Category Created");
                }
                else
                {
                    return new Response<string>(message: "Category's name existed");
                }
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<IEnumerable<TreeItem<Category>>>> GetCategories()
        {
            var allCategories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            if (allCategories != null)
            {
                var root = allCategories.GenerateTree(x => x.Id, y => y.ParentId);
                return new Response<IEnumerable<TreeItem<Category>>>(root);
            }
            return new Response<IEnumerable<TreeItem<Category>>>(message: "Empty");
        }

        public async Task<Response<CategoryHierachy>> GetCategoryById(GetCategoryByIdRequest request)
        {

            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Guid.Parse(request.Id));
                if (category != null)
                {
                    var response = _mapper.Map<CategoryHierachy>(category);
                    var result = response;
                    while (response.ParentId != null)
                    {   
                        var parent = _mapper.Map<CategoryHierachy>(await _unitOfWork.GetRepository<Category>().GetByIdAsync((response.ParentId)));
                        response.Parent = parent;
                        response = parent;
                    }
                    return new Response<CategoryHierachy>(_mapper.Map<CategoryHierachy>(result), message: "Success");
                }
            }
            return new Response<CategoryHierachy>(message: "Category not Found");
        }

        public async Task<Response<string>> UpdateCategory(UpdateCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name) && !string.IsNullOrWhiteSpace(request.Id))
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Guid.Parse(request.Id));
                if (category != null)
                {
                    category.Name = request.Name;
                    category.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "Category is updated");
                }
                else if (category == null)
                {
                    return new Response<string>(message: "Category ID is not existed");
                }
                //else if parent not found
            }
            else if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Id))
            {
                return new Response<string>(message: "Name or ID cannot be blanked");
            }
            return new Response<string>(message: "Failed to Create");
        }


    }
}
