using System;

#nullable disable

namespace API.Domains
{
    public partial class Report
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid RetailerId { get; set; }
        public Guid DistributorId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalExpense { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Distributer Distributor { get; set; }
        public virtual Order Order { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}
