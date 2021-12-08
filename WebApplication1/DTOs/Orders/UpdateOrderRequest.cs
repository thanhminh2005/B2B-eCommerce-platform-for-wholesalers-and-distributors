namespace API.DTOs.Orders
{
    public class UpdateOrderRequest
    {
        public string Id { get; set; }
        public double OrderCost { get; set; }
        public int Status { get; set; }
    }
}
