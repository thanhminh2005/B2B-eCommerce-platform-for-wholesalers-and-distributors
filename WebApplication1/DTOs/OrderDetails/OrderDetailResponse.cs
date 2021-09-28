using API.DTOs.Products;

namespace API.DTOs.OrderDetails
{
    public class OrderDetailResponse
    {
        public string Id { get; set; }
        public ProductResponse Product { get; set; }
        public string OrderId { get; set; }
        public double OrderPrice { get; set; }
        public int Quantity { get; set; }
    }
}
