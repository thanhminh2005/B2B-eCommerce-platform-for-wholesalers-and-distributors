using System;

#nullable disable

namespace API.Domains
{
    public partial class Price
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Product Product { get; set; }
    }
}
