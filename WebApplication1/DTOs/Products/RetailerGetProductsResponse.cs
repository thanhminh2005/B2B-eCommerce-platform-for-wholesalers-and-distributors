using API.DTOs.Categories;
using API.DTOs.Prices;
using System;
using System.Collections.Generic;

namespace API.DTOs.Products
{
    public class RetailerGetProductsResponse
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        //public Guid CategoryId { get; set; }
        public CategoryResponse Category { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int MinQuantity { get; set; }
        public List<PriceResponse> ListPrice { get; set; }
        public int Status { get; set; }
    }
}
