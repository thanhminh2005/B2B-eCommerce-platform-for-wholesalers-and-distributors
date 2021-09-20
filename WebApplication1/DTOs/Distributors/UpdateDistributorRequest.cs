using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Distributors
{
    public class UpdateDistributorRequest
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
