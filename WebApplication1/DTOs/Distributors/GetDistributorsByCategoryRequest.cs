namespace API.DTOs.Distributors
{
    public class GetDistributorsByCategoryRequest
    {
        public bool? IsActive { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
    }
}
