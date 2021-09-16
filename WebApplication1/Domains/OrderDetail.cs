using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public double OrderPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
