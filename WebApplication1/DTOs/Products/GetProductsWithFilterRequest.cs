using API.Warppers;

namespace API.DTOs.Products
{
    public class GetProductsWithFilterRequest : PagedRequest
    {
        public string SearchValue { get; set; }
        public string DistributorId { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public bool? SortByDate { get; set; }
        public bool? SortByPrice { get; set; }
        public bool? IsAscending { get; set; }
        public int? Status { get; set; }
    }
}
