using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public Guid OrderDetailId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}
