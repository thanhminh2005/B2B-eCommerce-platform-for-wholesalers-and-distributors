using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Memberships
{
    public class MembershipResponse
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public Guid DistributorId { get; set; }
        public Guid MembershipRankId { get; set; }
        public int Point { get; set; }
    }
}
