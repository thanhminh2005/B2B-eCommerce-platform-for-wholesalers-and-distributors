using System;

#nullable disable

namespace API.Domains
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }

        public virtual User User { get; set; }
    }
}
