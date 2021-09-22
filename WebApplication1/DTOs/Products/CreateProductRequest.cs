using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Products
{
    public class CreateProductRequest
    {
        public string DistributorId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int MinQuantity { get; set; }
        
    }
}
