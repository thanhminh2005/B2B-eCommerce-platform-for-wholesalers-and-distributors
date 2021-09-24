using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Products
{
    public class GetProductsWithFilterRequest : PagedRequest
    {
        public string SearchValue { get; set; }
        public string DistributorId { get; set; }
        public string CategoryId { get; set; }
        public int Status { get; set; }
    }
}
