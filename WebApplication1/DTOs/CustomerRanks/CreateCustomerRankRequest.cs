using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CustomerRanks
{
    public class CreateCustomerRankRequest
    {
        public string DistributorId { get; set; }
        public string MembershipRankId { get; set; }
        public int Threshold { get; set; }
    }
}
