using API.DTOs.Users;
using System;

namespace API.DTOs.Distributors
{
    public class DistributorDisplayResponse
    {
        public Guid Id { get; set; }
        public UserDisplayResponse User { get; set; }
    }
}
