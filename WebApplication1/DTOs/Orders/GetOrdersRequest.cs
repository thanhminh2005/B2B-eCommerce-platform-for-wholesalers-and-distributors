using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Orders
{
    public class GetOrdersRequest
    {
        public string SessionId { get; set; }
        public int Status { get; set; }
    }
}
