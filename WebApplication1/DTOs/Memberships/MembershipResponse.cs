using System;

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
