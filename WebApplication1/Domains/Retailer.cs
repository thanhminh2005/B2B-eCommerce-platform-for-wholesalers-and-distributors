using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Retailer
    {
        public Retailer()
        {
            Feedbacks = new HashSet<Feedback>();
            RetailerPaymentMethods = new HashSet<RetailerPaymentMethod>();
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<RetailerPaymentMethod> RetailerPaymentMethods { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
