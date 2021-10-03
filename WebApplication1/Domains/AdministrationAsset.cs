using System;

#nullable disable

namespace API.Domains
{
    public partial class AdministrationAsset
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
