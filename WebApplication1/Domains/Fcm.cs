using System;

#nullable disable

namespace API.Domains
{
    public partial class Fcm
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User User { get; set; }
    }
}
