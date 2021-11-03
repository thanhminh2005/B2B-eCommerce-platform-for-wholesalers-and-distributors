using API.Warppers;

namespace API.DTOs.Sessions
{
    public class GetSessionsRequest : PagedRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
