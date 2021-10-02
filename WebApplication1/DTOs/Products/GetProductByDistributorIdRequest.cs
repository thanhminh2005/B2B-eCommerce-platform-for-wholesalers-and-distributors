using API.Warppers;

namespace API.DTOs.Products

{
    public class GetProductByDistributorIdRequest : PagedRequest
    {
        public string DistributorId { get; set; }
    }
}
