using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Retailers
{
    public class UpdateRetailerRequest
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
