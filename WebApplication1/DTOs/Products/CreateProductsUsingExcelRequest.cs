using Microsoft.AspNetCore.Http;

namespace API.DTOs.Products
{
    public class CreateProductsUsingExcelRequest
    {
        public IFormFile File { get; set; }
        public string DistributorId { get; set; }
    }
}
