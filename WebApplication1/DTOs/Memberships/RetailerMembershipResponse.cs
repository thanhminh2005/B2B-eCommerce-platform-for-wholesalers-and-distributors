using API.DTOs.Distributors;
using API.DTOs.MembershipRanks;
using System;

namespace API.DTOs.Memberships
{
    public class RetailerMembershipResponse
    {
        public Guid Id { get; set; }
        public DistributorDisplayResponse Distributor { get; set; }
        public MembershipRankResponse MembershipRank { get; set; }
        public int Point { get; set; }
    }
}
