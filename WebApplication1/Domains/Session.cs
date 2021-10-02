using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Session
    {
        public Session()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Retailer Retailer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
