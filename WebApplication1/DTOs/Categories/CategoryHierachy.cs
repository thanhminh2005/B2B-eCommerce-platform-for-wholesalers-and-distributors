using System;

namespace API.DTOs.Categories
{
    public class CategoryHierachy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public CategoryHierachy Parent { get; set; }
    }
}
