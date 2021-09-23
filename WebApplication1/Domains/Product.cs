using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Prices = new HashSet<Price>();
        }

        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public int MinQuantity { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Category Category { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
    }
}
