using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
