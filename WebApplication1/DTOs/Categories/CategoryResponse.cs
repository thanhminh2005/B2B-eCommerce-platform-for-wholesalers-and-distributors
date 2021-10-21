using API.DTOs.SubCategories;
using System;
using System.Collections.Generic;

namespace API.DTOs.Categories
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryResponse> SubCategories { get; set; }
    }
}
