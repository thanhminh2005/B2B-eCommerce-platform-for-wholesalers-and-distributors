using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CustomerRanks
{
    public class CustomerRankResponse
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public Guid MembershipRankId { get; set; }
        public int Threshold { get; set; }
    }
}
