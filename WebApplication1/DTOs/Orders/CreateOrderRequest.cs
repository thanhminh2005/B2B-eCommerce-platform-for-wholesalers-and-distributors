namespace API.DTOs.Orders
{
    public class CreateOrderRequest
    {
        public string SessionId { get; set; }
        public string DistributorId { get; set; }
    }
}
