using API.DTOs.Prices;
using API.DTOs.SubCategories;
using System;
using System.Collections.Generic;

namespace API.DTOs.Products
{
    public class RetailerGetProductsResponse
    {
        public Guid Id { get; set; }
        public Guid ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public SubCategoryResponse SubCategory { get; set; }
        public string Distributor { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int MinQuantity { get; set; }
        public List<PriceResponse> ListPrice { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
    }
}
