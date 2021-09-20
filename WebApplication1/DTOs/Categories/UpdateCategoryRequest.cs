using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Categories
{
    public class UpdateCategoryRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        //update parent only
        //public string? Parent { get; set; }
    }
}
