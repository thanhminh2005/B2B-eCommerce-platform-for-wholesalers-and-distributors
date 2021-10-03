using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class MembershipRank
    {
        public MembershipRank()
        {
            CustomerRanks = new HashSet<CustomerRank>();
            Memberships = new HashSet<Membership>();
        }

        public Guid Id { get; set; }
        public string RankName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<CustomerRank> CustomerRanks { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
