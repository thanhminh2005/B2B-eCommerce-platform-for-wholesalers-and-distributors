using System;
using System.Collections.Generic;

namespace API.DTOs.Categories
{
    public class CategoryHierachy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? Parent { get; set; }
        public IList<CategoryHierachy> SubCategories { get; set; }
    }
}
