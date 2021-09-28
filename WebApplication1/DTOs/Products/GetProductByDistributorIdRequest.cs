using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Products
{
    public class GetProductByDistributorIdRequest: PagedRequest
    {
        public string DistributorId { get; set; }
    }
}
