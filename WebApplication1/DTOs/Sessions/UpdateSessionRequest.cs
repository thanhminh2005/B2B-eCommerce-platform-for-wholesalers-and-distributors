namespace API.DTOs.Sessions
{
    public class UpdateSessionRequest
    {
        public string Id { get; set; }
        public string PaymentMethodId { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
        public int Status { get; set; }
    }
}
