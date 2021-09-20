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

        public async Task<Response<IEnumerable<CategoryHierachy>>> GetCategory()
        {
            List<CategoryHierachy> items = new List<CategoryHierachy>();

            var allCategories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            //get all parent categories
            List<Category> parentCategories = allCategories.Where(c => c.Parent == null).ToList();

            if (allCategories != null)
            {
                foreach (var cat in parentCategories)
                {
                    
                    //add the parent category to the item list
                    items.Add(new CategoryHierachy{ ID = cat.Id, Name = cat.Name 
                        , SubCategories = GenerateSub((IList<Category>)allCategories, cat, items)
                    });
                 
                }
                return new Response<IEnumerable<CategoryHierachy>>(_mapper.Map<IEnumerable<CategoryHierachy>>(items), message: "Success");
            }
            return new Response<IEnumerable<CategoryHierachy>>(message: "Empty");
        }
        
        private IList<CategoryHierachy> GenerateSub(IList<Category> allCats, Category parent, IList<CategoryHierachy> items)
        {
            var SubCategory = allCats.Where(c => c.Parent == parent.Id );
            Console.WriteLine(parent.Name);
            foreach (var cat in SubCategory)
            {
                    items.Add(new CategoryHierachy { ID = cat.Id, Name = cat.Name, SubCategories = GenerateSub(allCats, cat, items) });
            }
            return items;
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

        public async Task<Response<string>> UpdateCategory(UpdateCategoryRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name) && !string.IsNullOrWhiteSpace(request.Id))
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Guid.Parse(request.Id));
                if (category != null)
                {
                    category.Name = request.Name;
                    category.DateCreated = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "Category is updated");
                }
                else if(category == null)
                {
                    return new Response<string>(message: "Category ID is not existed");
                }
                //else if parent not found
            }
            else if(string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Id))
            {
                return new Response<string>(message: "Name or ID cannot be blanked");
            }
            return new Response<string>(message: "Failed to Create");
        }
    }
}
