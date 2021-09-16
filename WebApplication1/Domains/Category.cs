using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? Parent { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
