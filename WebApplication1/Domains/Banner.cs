using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Banner
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Distributor Distributor { get; set; }
    }
}
