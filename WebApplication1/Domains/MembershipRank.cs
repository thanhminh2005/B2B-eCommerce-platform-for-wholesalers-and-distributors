using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class MembershipRank
    {
        public MembershipRank()
        {
            Memberships = new HashSet<Membership>();
        }

        public Guid Id { get; set; }
        public string RankName { get; set; }
        public int Threshold { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
