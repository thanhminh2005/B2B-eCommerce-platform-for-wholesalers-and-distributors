using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            RetailerPaymentMethods = new HashSet<RetailerPaymentMethod>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<RetailerPaymentMethod> RetailerPaymentMethods { get; set; }
    }
}
