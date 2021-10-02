using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Membership
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public Guid DistributorId { get; set; }
        public Guid MembershipRankId { get; set; }
        public int Point { get; set; }

        public virtual Distributor Distributor { get; set; }
        public virtual MembershipRank MembershipRank { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}
