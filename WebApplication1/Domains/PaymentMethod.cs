using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
