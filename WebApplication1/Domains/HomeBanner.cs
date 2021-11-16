using System;

#nullable disable

namespace API.Domains
{
    public partial class HomeBanner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        public string Link { get; set; }
        public int Position { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
