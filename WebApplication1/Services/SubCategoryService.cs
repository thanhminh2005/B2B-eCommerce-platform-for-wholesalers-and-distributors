using API.Domains;
using API.DTOs.SubCategories;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateSubCategory(CreateSubCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var category = await _unitOfWork.GetRepository<SubCategory>().FirstAsync(c => c.Name.Equals(request.Name));
                if (category == null)
                {
                    var newSubCategory = _mapper.Map<SubCategory>(request);
                    newSubCategory.Id = Guid.NewGuid();
                    newSubCategory.DateCreated = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<SubCategory>().AddAsync(newSubCategory);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "SubCategory Created");
                }
                else
                {
                    return new Response<string>(message: "SubCategory's name existed");
                }
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<IEnumerable<SubCategoryResponse>>> GetSubCategories(GetSubCategoriesRequest request)
        {
            IEnumerable<SubCategory> categories = null;
            if (string.IsNullOrWhiteSpace(request.CategoryId))
            {
                categories = await _unitOfWork.GetRepository<SubCategory>().GetAllAsync();
            }
            else
            {
                categories = await _unitOfWork.GetRepository<SubCategory>().GetAsync(x => x.CategoryId.Equals(Guid.Parse(request.CategoryId)));
            }
            if (categories.Any())
            {
                return new Response<IEnumerable<SubCategoryResponse>>(_mapper.Map<IEnumerable<SubCategoryResponse>>(categories), message: "Succeed");
            }
            return new Response<IEnumerable<SubCategoryResponse>>(message: "Empty");
        }

        public async Task<Response<SubCategoryResponse>> GetSubCategoryById(GetSubCategoryByIdRequest request)
        {
            var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(Guid.Parse(request.Id));
            if (category != null)
            {
                return new Response<SubCategoryResponse>(_mapper.Map<SubCategoryResponse>(category), message: "Succeed");
            }
            return new Response<SubCategoryResponse>(message: "Not Found");
        }



        //public async Task<Response<IEnumerable<TreeItem<SubCategoryResponse>>>> GetCategories()
        //{
        //    var allCategories = await _unitOfWork.GetRepository<SubCategory>().GetAllAsync();
        //    if (allCategories != null)
        //    {
        //        var allCatergoryResponse = _mapper.Map<IEnumerable<SubCategoryResponse>>(allCategories);
        //        var root = allCatergoryResponse.GenerateTree(x => x.Id, y => y.ParentId);
        //        return new Response<IEnumerable<TreeItem<SubCategoryResponse>>>(root);
        //    }
        //    return new Response<IEnumerable<TreeItem<SubCategoryResponse>>>(message: "Empty");
        //}

        //public async Task<Response<SubCategoryHierachy>> GetSubCategoryById(GetSubCategoryByIdRequest request)
        //{

        //    if (!string.IsNullOrWhiteSpace(request.Id))
        //    {
        //        var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(Guid.Parse(request.Id));
        //        if (category != null)
        //        {
        //            var response = _mapper.Map<SubCategoryHierachy>(category);
        //            var result = response;
        //            while (response.ParentId != null)
        //            {
        //                var parent = _mapper.Map<SubCategoryHierachy>(await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync((response.ParentId)));
        //                response.Parent = parent;
        //                response = parent;
        //            }
        //            return new Response<SubCategoryHierachy>(_mapper.Map<SubCategoryHierachy>(result), message: "Success");
        //        }
        //    }
        //    return new Response<SubCategoryHierachy>(message: "SubCategory not Found");
        //}

        public async Task<Response<string>> UpdateSubCategory(UpdateSubCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name) && !string.IsNullOrWhiteSpace(request.Id))
            {
                var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(Guid.Parse(request.Id));
                if (category != null)
                {
                    category.Name = request.Name;
                    category.CategoryId = Guid.Parse(request.CategoryId);
                    category.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<SubCategory>().UpdateAsync(category);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "SubCategory is updated");
                }
                else if (category == null)
                {
                    return new Response<string>(message: "SubCategory ID is not existed");
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
