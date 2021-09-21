using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Categories
{
    public class CategoryHierachy
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IList<CategoryHierachy> SubCategories { get; set; }
    }
}
