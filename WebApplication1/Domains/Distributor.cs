using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Distributor
    {
        public Distributor()
        {
            Banners = new HashSet<Banner>();
            Memberships = new HashSet<Membership>();
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Banner> Banners { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
