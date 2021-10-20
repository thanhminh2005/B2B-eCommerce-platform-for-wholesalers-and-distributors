using System;

namespace API.DTOs.SubCategories
{
    public class SubCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
