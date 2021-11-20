using System;
using System.Collections.Generic;

#nullable disable

namespace API.Domains
{
    public partial class User
    {
        public User()
        {
            Distributors = new HashSet<Distributor>();
            Fcms = new HashSet<Fcm>();
            Notifications = new HashSet<Notification>();
            Retailers = new HashSet<Retailer>();
        }

        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string DisplayName { get; set; }
        public DateTime DoB { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public Guid? ActivationCode { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<Fcm> Fcms { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Retailer> Retailers { get; set; }
    }
}
