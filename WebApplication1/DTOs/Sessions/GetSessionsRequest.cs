namespace API.DTOs.Sessions
{
    public class GetSessionsRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
