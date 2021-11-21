using System;

namespace API.DTOs.Users
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public DateTime DoB { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BusinessLicense { get; set; }
        public string TaxId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
