using System;

namespace API.DTOs.Users
{
    public class UserDisplayResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessLicense { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
    }
}
