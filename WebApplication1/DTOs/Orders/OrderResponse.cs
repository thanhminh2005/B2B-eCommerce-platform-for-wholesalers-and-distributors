using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public double OrderCost { get; set; }
        public int Status { get; set; }
    }
}
