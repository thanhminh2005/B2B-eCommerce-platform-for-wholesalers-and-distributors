namespace API.DTOs.OrderDetails
{
    public class UpdateOrderDetailRequest
    {
        public string Id { get; set; }
        public double OrderPrice { get; set; }
        public int Quantity { get; set; }
    }
}
