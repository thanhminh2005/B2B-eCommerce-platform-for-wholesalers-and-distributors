using API.DTOs.MembershipRanks;
using API.DTOs.Retailers;
using System;

namespace API.DTOs.Memberships
{
    public class DistributorMembershipResponse
    {
        public Guid Id { get; set; }
        public RetailerDisplayResponse Retailer { get; set; }
        public MembershipRankResponse MembershipRank { get; set; }
        public int Point { get; set; }
    }
}
