namespace API.DTOs.Sessions
{
    public class GetSessionsRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
        public double TotalCost { get; set; }
        public int Status { get; set; }
    }
}
