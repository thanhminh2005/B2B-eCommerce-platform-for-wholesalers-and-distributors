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
            Products = new HashSet<Product>();
            Reports = new HashSet<Report>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Banner> Banners { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
