namespace API.DTOs.OrderDetails
{
    public class CreateOrderDetailRequest
    {
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public double OrderPrice { get; set; }
        public int Quantity { get; set; }
    }
}
