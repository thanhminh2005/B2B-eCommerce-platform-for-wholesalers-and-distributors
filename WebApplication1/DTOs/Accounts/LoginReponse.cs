using API.DTOs.Roles;
using System;

namespace API.DTOs.Accounts
{
    public class LoginResponse
    {
        public Guid Id { get; set; }
        public RoleResponse Role { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JwtToken { get; set; }
    }
}
