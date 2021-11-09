using API.DTOs.Users;
using System;

namespace API.DTOs.Retailers
{
    public class RetailerDisplayResponse
    {
        public Guid Id { get; set; }
        public UserDisplayResponse User { get; set; }
    }
}
