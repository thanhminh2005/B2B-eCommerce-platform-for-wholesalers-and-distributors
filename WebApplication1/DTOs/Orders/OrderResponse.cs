using API.DTOs.Distributors;
using System;

namespace API.DTOs.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public DistributorDisplayResponse Distributor { get; set; }
        public double OrderCost { get; set; }
        public int Status { get; set; }
    }
}
