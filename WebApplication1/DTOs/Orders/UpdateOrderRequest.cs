using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Orders
{
    public class UpdateOrderRequest
    {
        public string Id { get; set; }
        public string SessionId { get; set; }
        public double OrderCost { get; set; }
        public int Status { get; set; }
    }
}
