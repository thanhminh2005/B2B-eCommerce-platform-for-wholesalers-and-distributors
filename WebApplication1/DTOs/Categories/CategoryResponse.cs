using API.DTOs.SubCategories;
using System;

namespace API.DTOs.Categories
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SubCategoryResponse SubCategory { get; set; }
    }
}
