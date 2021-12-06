using API.Warppers;

namespace API.DTOs.Orders
{
    public class GetOrdersRequest : PagedRequest
    {
        public string SessionId { get; set; }
        public string DistributorId { get; set; }
        public string RetailerId { get; set; }
        public string Status { get; set; }
    }
}
