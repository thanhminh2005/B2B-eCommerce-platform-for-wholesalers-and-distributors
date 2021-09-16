using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Users
{
    public class UpdateUserProfileRequest
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string DoB { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
