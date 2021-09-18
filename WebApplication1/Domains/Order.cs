using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Reports = new HashSet<Report>();
        }

        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public double OrderCost { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Session Session { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
