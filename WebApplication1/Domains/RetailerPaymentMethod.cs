using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class RetailerPaymentMethod
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string Detail { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}
