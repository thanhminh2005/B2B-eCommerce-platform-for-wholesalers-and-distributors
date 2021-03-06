using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
