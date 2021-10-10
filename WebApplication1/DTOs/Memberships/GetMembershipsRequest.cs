using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Memberships
{
    public class GetMembershipsRequest : PagedRequest
    {
        public string RetailerId { get; set; }
        public string DistributorId { get; set; }
        public string MembershipRankId { get; set; }
    }
}
