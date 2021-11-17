using API.DTOs.Products;
using System;

namespace API.DTOs.OrderDetails
{
    public class OrderDetailResponse
    {
        public Guid Id { get; set; }
        public ProductResponse Product { get; set; }
        public Guid OrderId { get; set; }
        public double OrderPrice { get; set; }
        public int Quantity { get; set; }
    }
}
