using API.Warppers;

namespace API.DTOs.OrderDetails
{
    public class GetOrderDetailsRequest : PagedRequest
    {
        public string OrderId { get; set; }
        public double? OrderPrice { get; set; }
        public int? Quantity { get; set; }
    }
}
