using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }

        public Guid? Parent { get; set; }
    }
}
