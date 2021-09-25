using System;

namespace API.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}
