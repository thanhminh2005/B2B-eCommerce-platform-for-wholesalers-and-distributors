using System;

#nullable disable

namespace API.Domains
{
    public partial class CustomerRank
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public Guid MembershipRankId { get; set; }
        public int Threshold { get; set; }
        public double DiscountRate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Distributor Distributor { get; set; }
        public virtual MembershipRank MembershipRank { get; set; }
    }
}
