namespace API.DTOs.Sessions
{
    public class CreateSessionRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
    }
}
